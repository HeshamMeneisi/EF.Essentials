namespace EF.Essentials.Config
{
    public class KeyStoreConfig: IServiceConfig
    {
        public string BaseUrl { set; get; }
        public bool Bypass { get; set; }
    }
}
