using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GenericCompany.Common.Encryption;
using Serilog;

namespace GenericCompany.Common.Communication
{
    public interface IApi
    {
        public IApiConfiguration Configuration { get; }
    }

    public interface IApiConfiguration
    {
        public Dictionary<string, string> DefaultHeaders { get; }
    }

    public class SdkApi<TApi> where TApi: class
    {
        private readonly IServiceTokenFactory _tokenFactory;
        /// <summary>
        /// this has to be dynamic as different auto-generated packages have different namespaces
        /// </summary>
        private readonly dynamic _api;

        public SdkApi(TApi instance, IServiceTokenFactory tokenFactory)
        {
            _tokenFactory = tokenFactory;
            _api = instance ?? throw new NoNullAllowedException(nameof(instance));

            if (_api.Configuration == null || _api.Configuration.DefaultHeaders == null)
            {
                throw new InvalidCastException($"{nameof(TApi)} ({instance.GetType().FullName}) must be a valid cast to {nameof(IApi)}");
            }
        }

        public TApi Instance {
            get
            {
                RenewServiceToken();
                return _api as TApi;
            }
        }

        private void RenewServiceToken()
        {
            try {
                SetToken(_tokenFactory.GenerateJwtToken());
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"failed to get service token for api: {_api.GetType().FullName}", ex);
            }
        }

        private void SetToken(string token)
        {
            _api.Configuration.DefaultHeaders["Authorization"] = $"Bearer {token}";
        }

        public async Task ExecuteWithClientToken(Func<TApi, Task> action, string clientToken)
        {
            SetToken(clientToken);
            await action(_api);
            RenewServiceToken();
        }

        public async Task<T> ExecuteWithClientToken<T>(Func<TApi, Task<T>> action, string clientToken)
        {
            SetToken(clientToken);
            var result = await action(_api);
            RenewServiceToken();
            return result;
        }
    }
}
