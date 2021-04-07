using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EF.Essentials.Config;
using EF.Essentials.KeyStore;
using EF.Essentials.KeyStore.Models;
using Microsoft.Extensions.Options;
using KeyNotFoundException = EF.Essentials.KeyStore.KeyNotFoundException;

namespace EF.Essentials.Encryption
{
    public interface ISignatureKeyResolver
    {
        Task<string> ResolveKey(string keyId);
    }

    public class SignatureKeyResolver : ISignatureKeyResolver
    {
        private readonly IKeyStoreClient _keyStoreClient;
        private readonly Dictionary<string, StoredKey> _cache = new();
        private readonly SecurityConfig _config;

        public SignatureKeyResolver(IKeyStoreClient keyStoreClient, IOptions<SecurityConfig> options)
        {
            _keyStoreClient = keyStoreClient;
            _config = options.Value;
        }

        public async Task<string> ResolveKey(string keyId)
        {
            if (_cache.ContainsKey(keyId))
            {
                var stored = _cache[keyId];
                if (!string.IsNullOrEmpty(stored.Key) &&
                    DateTime.UtcNow - stored.TimeStamp < TimeSpan.FromSeconds(_config.SignatureTtl)) return stored.Key;
                _cache.Remove(keyId);
            }

            var response = await _keyStoreClient.Keys.GetAsync(keyId);
            var result = response switch
            {
                KeyCreatedResponse keyCreatedResponse => keyCreatedResponse.Body,
                _ => throw new KeyStore.KeyNotFoundException($"key: '{keyId}' not found")
            };
            _cache[keyId] = new StoredKey()
            {
                Key = result,
                TimeStamp = DateTime.UtcNow
            };
            return result;
        }
    }

    internal class StoredKey
    {
        public string Key { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
