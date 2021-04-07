namespace EF.Essentials.Config
{
    public class CorsConfig
    {
        public bool Enabled { get; set; }
        public string[] AllowedOrigins { get; set; } = new[] {"*"};
        public string[] AllowedHeaders { get; set; } = {"authorization", "content-type", "x-ms-command-name"};
    }
}
