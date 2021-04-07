using EF.Essentials.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EF.Essentials.UnitTests
{
    [TestClass]
    public class DataHelperTest
    {
        [TestMethod]
        public void TestBracketComparison()
        {
            Assert.IsTrue(DataHelper.CheckBracketsIntersect("1-2", "2-3"));
            Assert.IsTrue(DataHelper.CheckBracketsIntersect("3-10", "5-7"));
            Assert.IsTrue(DataHelper.CheckBracketsIntersect("2-7", "5-20"));
            Assert.IsTrue(DataHelper.CheckBracketsIntersect("5+", "3-5"));
            Assert.IsTrue(DataHelper.CheckBracketsIntersect("20+", "16-25"));
            Assert.IsTrue(DataHelper.CheckBracketsIntersect("5-10", "10-12"));
            Assert.IsTrue(DataHelper.CheckBracketsIntersect("5", "2-12"));
            Assert.IsTrue(DataHelper.CheckBracketsIntersect("1-3", "2"));
            Assert.IsFalse(DataHelper.CheckBracketsIntersect("20+", "10-19"));
            Assert.IsFalse(DataHelper.CheckBracketsIntersect("10-15", "5-8"));
            Assert.IsFalse(DataHelper.CheckBracketsIntersect("5-10", "11-15"));
            Assert.IsFalse(DataHelper.CheckBracketsIntersect("1-3", "5"));
            Assert.IsFalse(DataHelper.CheckBracketsIntersect("3-7", "1"));
            Assert.IsFalse(DataHelper.CheckBracketsIntersect("1-3", "0"));
            Assert.IsFalse(DataHelper.CheckBracketsIntersect("0", "4-9"));
            Assert.IsFalse(DataHelper.CheckBracketsIntersect("0-2", "5-9"));
        }

        [TestMethod]
        public void TestGetValidExt()
        {
            Assert.AreEqual(".jpeg", DataHelper.GetValidExt("image/jpeg", new []{".jpeg", ".png"}));
            Assert.AreEqual(".png", DataHelper.GetValidExt("image/png", new []{".jpg", ".png"}));
            Assert.AreEqual(".svg", DataHelper.GetValidExt("image/svg+xml", new []{".svg"}));
        }
    }
}
