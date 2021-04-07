using System.Web;
using EF.Essentials.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EF.Essentials.UnitTests
{
    [TestClass]
    public class UrlBuilderTests
    {
        [TestMethod]
        public void TestFullUrl()
        {
            Assert.AreEqual("https://test.example.com/a/b/c?foo=a&bar=b",
                new UrlBuilder().WithDomain("example.com").WithSubdomain("test").WithPath("a/b").WithPath("c").WithAttribute("foo", "a").WithAttribute("bar", "b").Build());
        }

        [TestMethod]
        public void TestEncoding()
        {
            string text = "1-2+3/5!@#$%^&*()_+";
            Assert.IsTrue(HttpUtility
                .UrlDecode(new UrlBuilder()
                    .WithBase("https://test.com")
                    .WithAttribute("test", text).Build()
                ).Contains(text));
        }
    }
}
