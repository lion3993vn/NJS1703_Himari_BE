using HimariServer.Service.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class HuggingFaceTokenizerService : IHuggingFaceTokenizerService
    {
        private Dictionary<string, int> _vocab;
        private List<string> _merges;
        private int _padId;
        private int _unkId;
        private string _unkToken = "[UNK]";
        private string _padToken = "[PAD]";
        private readonly string _basePath;

        public HuggingFaceTokenizerService()
        {
            _basePath = AppDomain.CurrentDomain.BaseDirectory;
            var tokenizerPath = Path.Combine(_basePath, "EmbeddingModels", "tokenizer.json");
            
            try
            {
                if (File.Exists(tokenizerPath))
                {
                    var json = File.ReadAllText(tokenizerPath);
                    var tokenizerConfig = JObject.Parse(json);

                    _vocab = tokenizerConfig["model"]?["vocab"]?.ToObject<Dictionary<string, int>>() ?? new Dictionary<string, int>();
                    _merges = tokenizerConfig["model"]?["merges"]?.ToObject<List<string>>() ?? new List<string>();

                    _padId = _vocab.TryGetValue(_padToken, out int padId) ? padId : 0;
                    _unkId = _vocab.TryGetValue(_unkToken, out int unkId) ? unkId : 0;
                }
                else
                {
                    InitializeDefaultValues();
                }
            }
            catch (Exception ex)
            {
                InitializeDefaultValues();
            }
        }
        
        private void InitializeDefaultValues()
        {
            _vocab = new Dictionary<string, int>();
            _merges = new List<string>();
            _padId = 0;
            _unkId = 1;
        }

        private string NormalizeVietnameseText(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            text = text.ToLowerInvariant();

            text = text.Replace("  ", " ").Trim();

            return text;
        }

        public (long[] InputIds, long[] AttentionMask) Tokenize(string text, int maxLength)
        {
            List<long> inputIds = new List<long>();

            try
            {
                // First normalize the text for Vietnamese
                text = NormalizeVietnameseText(text ?? string.Empty);

                // Process multi-word n-grams first before falling back to individual words
                string[] words = text.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < words.Length; i++)
                {
                    // Try phrase of 3 words first, then 2 words, then single word
                    bool tokenFound = false;

                    // Try 3-gram if possible
                    if (i + 2 < words.Length)
                    {
                        string trigram = $"{words[i]} {words[i + 1]} {words[i + 2]}";
                        if (_vocab.TryGetValue(trigram, out int trigramId))
                        {
                            inputIds.Add(trigramId);
                            i += 2; // Skip the next two words
                            tokenFound = true;
                            continue;
                        }
                    }

                    // Try 2-gram if possible
                    if (i + 1 < words.Length)
                    {
                        string bigram = $"{words[i]} {words[i + 1]}";
                        if (_vocab.TryGetValue(bigram, out int bigramId))
                        {
                            inputIds.Add(bigramId);
                            i += 1; // Skip the next word
                            tokenFound = true;
                            continue;
                        }
                    }

                    // Try single word
                    if (_vocab.TryGetValue(words[i], out int id))
                    {
                        inputIds.Add(id);
                        tokenFound = true;
                    }
                    else
                    {
                        // Fall back to character-by-character but keep word boundaries
                        foreach (char c in words[i])
                        {
                            string charStr = c.ToString();
                            inputIds.Add(_vocab.TryGetValue(charStr, out int charId) ? charId : _unkId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                inputIds.Add(_unkId);
            }

            
            if (inputIds.Count > maxLength)
                inputIds = inputIds.Take(maxLength).ToList();
            else
                inputIds.AddRange(Enumerable.Repeat((long)_padId, maxLength - inputIds.Count));

            var attentionMask = inputIds.Select(id => id == _padId ? 0L : 1L).ToArray();
            return (inputIds.ToArray(), attentionMask);
        }
    }
}
