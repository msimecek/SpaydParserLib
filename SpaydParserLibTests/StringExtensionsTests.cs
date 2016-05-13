using NUnit.Framework;

namespace SpaydParserLib.Test
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void ExtractValuePairs_ReturnsPairs_WhenValid()
        {
            string spayd = "KEY:VALUE*KEY2:VALUE2";

            var result = spayd.ExtractValuePairs('*', ':', true);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.ContainsKey("KEY"));
            Assert.IsTrue(result.ContainsKey("KEY2"));
            Assert.AreEqual("VALUE", result["KEY"]);
            Assert.AreEqual("VALUE2", result["KEY2"]);
        }

        [Test]
        public void ExtractValuePairs_ReturnsPairs_IgnoreCase()
        {
            string spayd = "KEY:value*Key2:Value2*key3:valuE3";

            var result = spayd.ExtractValuePairs('*', ':', true);

            Assert.IsTrue(result.ContainsKey("KEY"));
            Assert.IsTrue(result.ContainsKey("KEY2"));
            Assert.IsTrue(result.ContainsKey("KEY3"));

            Assert.AreEqual("value", result["KEY"]);
            Assert.AreEqual("Value2", result["KEY2"]);
            Assert.AreEqual("valuE3", result["KEY3"]);
        }
    }
}
