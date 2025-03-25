using ChromaDB.Client;
using DeepSeek.Core.Models;
using DeepSeek.Core;
using HimariServer.Service.BusinessModels.ProductModels;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.SettingModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace HimariServer.Service.Services.Implements
{
    public class ChromaService : IChromaService
    {
        private readonly IOnnxEmbeddingService _onnxEmbeddingService;
        private readonly ChromaConfigurationOptions _configOptions;
        private readonly ChromaClient _client;
        private readonly string _collections;
        private readonly int nResults;
        private readonly IDeepseekService _deepseekService;
        private ChromaCollectionClient _collectionClient;
        private readonly HttpClient _httpClient;

        public ChromaService(IOnnxEmbeddingService onnxEmbeddingService, IOptions<ChromaDBSettings> chromaDBSettings, IDeepseekService deepseekService, IHttpClientFactory httpClientFactory)
        {
            nResults = 5;
            _onnxEmbeddingService = onnxEmbeddingService;
            _configOptions = new ChromaConfigurationOptions(uri: chromaDBSettings.Value.URL);
            _httpClient = httpClientFactory.CreateClient();
            _client = new ChromaClient(_configOptions, _httpClient);
            _collections = chromaDBSettings.Value.Collections;
            _deepseekService = deepseekService;
            InitializeCollection(_httpClient).Wait();
        }

        public async Task<List<ProductChatModel>> QuerySimilarProducts(string queryText)
        {
            try
            {
                if (string.IsNullOrEmpty(queryText))
                {
                    return new List<ProductChatModel>();
                }

                var deepseekResponse = await _deepseekService.FormatMessageUser(queryText);

                float[] queryEmbedding = await _onnxEmbeddingService.GenerateEmbedding(deepseekResponse);

                if (queryEmbedding == null || queryEmbedding.All(x => Math.Abs(x) < 1e-6))
                {
                    return new List<ProductChatModel>();
                }

                var queryEmbeddingMemory = new ReadOnlyMemory<float>(queryEmbedding);

                // Query ChromaDB for similar products
                var result = await _collectionClient.Query(
                    queryEmbeddings: [queryEmbeddingMemory], // Adjusted to match API expectation
                    nResults: nResults,
                    include: ChromaQueryInclude.Metadatas | ChromaQueryInclude.Documents | ChromaQueryInclude.Distances);

                // Process results
                List<ProductChatModel> allResults = new List<ProductChatModel>();
                if (result != null && result.Count > 0)
                {
                    foreach (var entryList in result)
                    {
                        foreach (var entry in entryList)
                        {
                            if (entry.Metadata != null)
                            {

                                double similarity;

                                if (entry.Distance >= 0 && entry.Distance <= 2)
                                {
                                    similarity = 1 - (entry.Distance / 2);
                                }
                                else
                                {
                                    similarity = Math.Max(0, Math.Min(1, 1 / (1 + entry.Distance)));
                                }

                                var metadata = entry.Metadata;

                                if (!metadata.TryGetValue("id", out object idObj) || idObj == null)
                                {
                                    continue;
                                }

                                int productId;
                                if (!int.TryParse(idObj.ToString(), out productId))
                                {
                                    continue;
                                }

                                string productName = metadata.TryGetValue("name", out object nameObj) && nameObj != null
                                    ? nameObj.ToString()
                                    : string.Empty;


                                allResults.Add(new ProductChatModel
                                {
                                    Id = productId,
                                    ProductName = productName,
                                    Similarity = similarity
                                });
                            }
                        }
                    }
                }

                var sortedResults = allResults.OrderByDescending(r => r.Similarity).Take(nResults).ToList();
                return sortedResults;
            }
            catch (Exception ex)
            {
                return new List<ProductChatModel>();
            }
        }

        private async Task InitializeCollection(HttpClient httpClient)
        {
            try
            {
                var collection = await _client.GetOrCreateCollection(_collections);
                _collectionClient = new ChromaCollectionClient(collection, _configOptions, httpClient);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task AddProductsToChromaDB(List<ProductRAGModel> products)
        {
            if (products == null || products.Count == 0)
            {
                return;
            }

            var ids = products.Select(p => p.Id.ToString()).ToList();

            var documents = products.Select(p =>
                $"{p.ProductName} {p.Description} {p.BrandName} {p.Symptomp} {p.BodyPart}").ToList();

            var metadatas = products.Select(p => new Dictionary<string, object>
                {
                    { "id", p.Id },
                    { "name", p.ProductName ?? string.Empty },
                    { "description", p.Description ?? string.Empty },
                    { "brand", p.BrandName ?? string.Empty },
                    { "symptoms", p.Symptomp ?? string.Empty },
                    { "bodyPart", p.BodyPart ?? string.Empty }
                }).ToList();

            var embeddings = new List<ReadOnlyMemory<float>>();

            foreach (var product in products)
            {
                string textToEmbed = $"Sản phẩm tên {product.ProductName} có mô tả như sau {product.Description} thuộc thương hiệu {product.BrandName} có thể chữa trị các triệu chứng {product.Symptomp} thuộc {product.BodyPart}";

                // Generate embedding using ONNX model
                float[] embedding = await _onnxEmbeddingService.GenerateEmbedding(textToEmbed);

                // Add to embeddings list
                embeddings.Add(new ReadOnlyMemory<float>(embedding));
            }

            await _collectionClient.Add(ids, embeddings: embeddings, documents: documents, metadatas: metadatas);
        }
    }
}
