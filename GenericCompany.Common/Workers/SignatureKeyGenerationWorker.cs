using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using GenericCompany.Common.Config;
using GenericCompany.Common.Encryption;
using GenericCompany.Common.KeyStore;
using GenericCompany.Common.KeyStore.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GenericCompany.Common.Workers
{
    // ReSharper disable once ClassNeverInstantiated.Global This is Worker and will be initialized by framework.
    public class SignatureKeyGenerationWorker : BackgroundService
    {
        private readonly ILogger<SignatureKeyGenerationWorker> _logger;
        private readonly ISignatureKeyContainer _signatureKeyContainer;
        private readonly IKeyStoreClient _keyStoreClient;
        private readonly IMetrics _metrics;
        private SecurityConfig _config;

        public SignatureKeyGenerationWorker(ILogger<SignatureKeyGenerationWorker> logger, IOptions<SecurityConfig> options,
            ISignatureKeyContainer signatureKeyContainer, IKeyStoreClient keyStoreClient, IMetrics metrics)
        {
            _logger = logger;
            _signatureKeyContainer = signatureKeyContainer;
            _keyStoreClient = keyStoreClient;
            _metrics = metrics;
            _config = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation($"Worker running at {DateTime.Now}");

                    try
                    {
                        var key = SigningKey.Create();
                        var response = await SaveKey(key.PublicKey, stoppingToken);
                        switch (response)
                        {
                            case KeyCreatedResponse createdResponse:
                                key.KeyId = createdResponse.Id;
                                _signatureKeyContainer.Key = key;
                                break;
                            case ProblemDetails p:
                                throw new Exception($"{p.Status} {p.TitleProperty} {p.Detail}");
                            default:
                                throw new Exception("unhandled error");
                        }

                        await Task.Delay(TimeSpan.FromSeconds(_config.SignatureTtl - 30), stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "failed to obtain signature key");
                        await Task.Delay(TimeSpan.FromSeconds(5));
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "caught exception");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }

        private async Task<object> SaveKey(string publicKey, CancellationToken stoppingToken)
        {
            return await _keyStoreClient.Keys.AddAsync(new CreateKeyRequest
            {
                Body = publicKey
            }, stoppingToken);
        }
    }
}
