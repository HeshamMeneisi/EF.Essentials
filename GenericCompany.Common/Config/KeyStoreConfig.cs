namespace GenericCompany.Common.Config
{
    public class KeyStoreConfig: IServiceConfig
    {
        public string BaseUrl { set; get; }
        public bool Bypass { get; set; }
    }
}
