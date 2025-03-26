using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.SettingModels;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _cache;
        private readonly RedisSettings _settings;
        private readonly JsonSerializerOptions _jsonOptions;

        // Constant key to store list of all cache keys
        private const string CACHE_KEYS_LIST = "redis_cache_keys";

        public RedisService(
            IDistributedCache cache,
            IOptions<RedisSettings> settings,
            IConfiguration configuration)
        {
            _cache = cache;
            _settings = settings.Value;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value, _jsonOptions);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(_settings.DefaultExpiryMinutes)
            };

            // Serialize and set the value
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(value, _jsonOptions), options);

            // Add the key to the list of cache keys
            await TrackCacheKey(key);
        }

        private async Task TrackCacheKey(string key)
        {
            // Get existing cache keys
            var existingKeysJson = await _cache.GetStringAsync(CACHE_KEYS_LIST);
            var cacheKeys = existingKeysJson == null
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(existingKeysJson, _jsonOptions);

            // Add the key if it doesn't exist
            if (!cacheKeys.Contains(key))
            {
                cacheKeys.Add(key);
                await _cache.SetStringAsync(
                    CACHE_KEYS_LIST,
                    JsonSerializer.Serialize(cacheKeys, _jsonOptions)
                );
            }
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
            await RemoveCacheKeyFromList(key);
        }

        private async Task RemoveCacheKeyFromList(string keyToRemove)
        {
            // Get existing cache keys
            var existingKeysJson = await _cache.GetStringAsync(CACHE_KEYS_LIST);
            if (existingKeysJson == null) return;

            var cacheKeys = JsonSerializer.Deserialize<List<string>>(existingKeysJson, _jsonOptions);

            // Remove the specific key
            cacheKeys.RemoveAll(k => k == keyToRemove);

            // Update the list of cache keys
            await _cache.SetStringAsync(
                CACHE_KEYS_LIST,
                JsonSerializer.Serialize(cacheKeys, _jsonOptions)
            );
        }

        public async Task ClearAllCachedKeys()
        {
            // Get all cached keys
            var existingKeysJson = await _cache.GetStringAsync(CACHE_KEYS_LIST);
            if (existingKeysJson == null) return;

            var cacheKeys = JsonSerializer.Deserialize<List<string>>(existingKeysJson, _jsonOptions);

            // Remove each cached key
            foreach (var key in cacheKeys)
            {
                await _cache.RemoveAsync(key);
            }

            // Clear the list of cache keys
            await _cache.RemoveAsync(CACHE_KEYS_LIST);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _cache.GetStringAsync(key) != null;
        }
    }
}
