namespace GenericCompany.Common.Config
{
    public class SecurityConfig
    {
        public int SignatureTtl { get; set; } = 3600;
        public double TokenTtl { get; set; } = 15 * 60;
    }
}