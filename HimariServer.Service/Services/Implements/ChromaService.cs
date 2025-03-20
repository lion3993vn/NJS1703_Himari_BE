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
    }
}
