using System.Threading.Tasks;
using EF.Essentials.KeyStore;
using EF.Essentials.KeyStore.Models;

namespace EF.Essentials.Communication
{
    public class CrossServiceLock
    {
        private readonly IKeyStoreClient _keyStoreClient;

        public CrossServiceLock(IKeyStoreClient keyStoreClient)
        {
            _keyStoreClient = keyStoreClient;
        }

        public async Task<LockingResponse> Lock(string key)
        {
            var response = await _keyStoreClient.Keys.AddAsync(new CreateKeyRequest
            {
                Body = key
            });

            return response switch
            {
                KeyCreatedResponse keyCreatedResponse =>
                    new LockingResponse
                    {
                        Acquired = true
                    },
                _ =>
                    new LockingResponse
                    {
                        Acquired = false
                    }
            };
        }

        public class LockingResponse
        {
            public bool Acquired { get; set; }
        }
    }
}
