using NUnit.Framework;
using SpaydParserLib.Enums;
using System;
using System.Collections.Generic;

namespace SpaydParserLib.Test
{
    [TestFixture]
    public class SindParserTests
    {
        [Test]
        public void TryGetAmount_WhenInvalid([Values("1A23")] string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("AM", value));

            // Act
            var amount = parser.TryGetAmount();

            // Assert
            Assert.IsTrue(parser.GetErrors().Count > 0);
        }

        [Test]
        public void TryGetTaxPerformance_WhenValid([Values("0", "1", "2")]string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("TP", value));

            // Act
            TaxPerformance expected = (TaxPerformance)Int32.Parse(value);
            var result = parser.TryGetTaxPerformance();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TryGetTaxPerformance_WhenInvalid([Values("A", "3", "65465465465321321321")]string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("TP", value));

            // Act
            var result = parser.TryGetTaxPerformance();

            // Assert
            Assert.IsTrue(parser.GetErrors().Count > 0);
        }

        private Dictionary<string, string> CreateTestCase(string key, string value)
        {
            return new Dictionary<string, string> { { key, value } };
        }
    }
}
