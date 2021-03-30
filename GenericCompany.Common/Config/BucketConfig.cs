namespace GenericCompany.Common.Config
{
    public class BucketConfig
    {
        public string SecretKey { set; get; }
        public string AccessKeyId { set; get; }
        public string BucketName { set; get; }
        public string BucketRegion { get; set; }
        public int SecondsTillSignatureExpires { get; set; }
        public bool IsMockup { get; set; } = true;
    }
}
