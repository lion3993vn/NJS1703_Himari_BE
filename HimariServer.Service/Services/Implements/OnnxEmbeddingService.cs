using HimariServer.Service.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class OnnxEmbeddingService : IOnnxEmbeddingService
    {
        private readonly InferenceSession? _session;
        private readonly int _maxSequenceLength = 19;
        private readonly int _embeddingDimension = 384;
        private readonly string _basePath;
        private readonly Random _random;
        private readonly IHuggingFaceTokenizerService _tokenizer;

        public OnnxEmbeddingService(IHuggingFaceTokenizerService tokenizer)
        {
            _random = new Random();
            _basePath = AppDomain.CurrentDomain.BaseDirectory;
            _tokenizer = tokenizer;

            try
            {
                var modelPath = Path.Combine(_basePath, "EmbeddingModels", "viet-embedding.onnx");
                if (File.Exists(modelPath))
                {
                    _session = new InferenceSession(modelPath);
                }
                else
                {
                    _session = null;
                }
            }
            catch (Exception ex)
            {
                _session = null;
            }
        }

        public async Task<float[]> GenerateEmbedding(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return await GenerateDefaultEmbedding();
            }

            try
            {
                // Check if session was initialized correctly
                if (_session == null)
                {
                    return await GenerateDefaultEmbedding();
                }

                var (inputIds, attentionMask) = PreprocessText(text);

                var inputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("input_ids", inputIds),
                    NamedOnnxValue.CreateFromTensor("attention_mask", attentionMask)
                };

                using (var results = _session.Run(inputs))
                {
                    var output = results.First().AsTensor<float>().ToArray();
                    return await NormalizeVector(output);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating embedding: {ex.Message}");
                return await GenerateDefaultEmbedding();
            }
        }

        private async Task<float[]> GenerateDefaultEmbedding()
        {
            var embedding = new float[_embeddingDimension];

            for (int i = 0; i < embedding.Length; i++)
            {
                embedding[i] = (float)_random.NextDouble() * 2 - 1;
            }

            return await NormalizeVector(embedding);
        }

        private async Task<float[]> NormalizeVector(float[] vector)
        {
            if (vector == null || vector.Length == 0)
                return new float[_embeddingDimension];

            float magnitude = (float)Math.Sqrt(vector.Select(x => x * x).Sum());

            if (magnitude <= 1e-6)
                return new float[vector.Length];

            return vector.Select(x => x / magnitude).ToArray();
        }

        private (Tensor<long>, Tensor<long>) PreprocessText(string text)
        {
            var tokenized = _tokenizer.Tokenize(text, _maxSequenceLength);
            var inputIds = new DenseTensor<long>(tokenized.InputIds, new[] { 1, _maxSequenceLength });
            var attentionMask = new DenseTensor<long>(tokenized.AttentionMask, new[] { 1, _maxSequenceLength });

            return (inputIds, attentionMask);
        }

        private (Tensor<long>, Tensor<long>) CreateDefaultTensors()
        {
            var defaultIds = new long[_maxSequenceLength];
            var defaultMask = new long[_maxSequenceLength];

            defaultIds[0] = 1;
            defaultMask[0] = 1;

            var inputIds = new DenseTensor<long>(defaultIds, new[] { 1, _maxSequenceLength });
            var attentionMask = new DenseTensor<long>(defaultMask, new[] { 1, _maxSequenceLength });

            return (inputIds, attentionMask);
        }
    }
}
