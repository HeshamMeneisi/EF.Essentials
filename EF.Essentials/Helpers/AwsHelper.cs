using Amazon;
using Amazon.S3;
using EF.Essentials.Config;
using Moq;
using Serilog;

namespace EF.Essentials.Helpers
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
