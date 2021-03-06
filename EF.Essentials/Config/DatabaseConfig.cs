namespace EF.Essentials.Config
{
    public class DatabaseConfig
    {
        public string Host { set; get; }
        public int Port { set; get; }
        public string Name { set; get; }
        public string User { set; get; }
        public string Password { set; get; }
        public bool AutoMigrate { set; get; }
    }
}
