using Amazon;
using Amazon.S3;
using GenericCompany.Common.Config;
using Moq;
using Serilog;

namespace GenericCompany.Common.Helpers
{
    public static class AwsHelper
    {
        public static AmazonS3Client GetS3Client(BucketConfig s3Config)
        {
            if (s3Config.IsMockup)
            {
                Log.Warning("Mocking S3 bucket");
                return new Mock<AmazonS3Client>("mockup", "mockup", RegionEndpoint.AFSouth1).Object;
            }
            return new AmazonS3Client(s3Config.AccessKeyId, s3Config.SecretKey,
                RegionEndpoint.GetBySystemName(s3Config.BucketRegion));
        }
    }
}
