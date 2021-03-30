using System.Text.RegularExpressions;
using GenericCompany.Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericCompany.Common.UnitTests
{
    [TestClass]
    public class GeneralHelpersTest
    {
        [TestMethod]
        public void TestStringGen()
        {
            for (int i = 0; i < 100; i++)
            {
                var str = GeneralHelpers.RandomAlphanum(i);
                Assert.AreEqual(i, str.Length);
                Assert.IsTrue(Regex.IsMatch(str, "^[a-zA-Z0-9]*$"));
            }
        }
    }
}
