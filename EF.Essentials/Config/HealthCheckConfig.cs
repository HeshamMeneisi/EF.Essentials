namespace EF.Essentials.Config
{
    public class HealthCheckConfig
    {
        public MemoryCheckConfig MemoryCheckConfig { get; set; }
        public ConnectionToDbCheckConfig ConnectionToDbCheckConfig { get; set; }
        public string Endpoint { get; set; } = "/healthcheck";
    }

    public class ConnectionToDbCheckConfig
    {
        public bool Enabled { get; set; } = true;
    }

    public class MemoryCheckConfig
    {
        public bool Enabled { get; set; } = true;
        public long MemoryUsageThresholdMb { get; set; } = 50;
    }
}
