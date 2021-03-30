namespace GenericCompany.Common.Config
{
    public class CorsConfig
    {
        public bool Enabled { get; set; }
        public string[] AllowedOrigins { get; set; }
        public string[] AllowedHeaders { get; set; } = {"authorization", "content-type", "x-ms-command-name"};
    }
}
