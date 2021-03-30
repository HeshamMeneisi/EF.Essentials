using GenericCompany.Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericCompany.Common.UnitTests
{
    [TestClass]
    public class UuidGenerationTest
    {
        [TestMethod]
        public void TestCreatesRandomString()
        {
            Assert.IsInstanceOfType(DataHelper.GenerateUuid(), typeof(string));
            Assert.AreNotEqual(DataHelper.GenerateUuid(), DataHelper.GenerateUuid());
        }
    }
}
