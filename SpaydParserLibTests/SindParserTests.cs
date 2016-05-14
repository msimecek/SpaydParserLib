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
            TaxPerformance expected = (TaxPerformance)int.Parse(value);
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

        [Test]
        public void TryGetInvoiceType_WhenValid([Values("0", "1", "5", "9")]string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("TD", value));

            // Act
            var result = parser.TryGetInvoiceType();
            InvoiceType expected = (InvoiceType)int.Parse(value);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TryGetInvoiceType_WhenInvalid([Values("ABC", "123.4", "-2", "6")]string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("TD", value));

            // Act
            var result = parser.TryGetInvoiceType();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void TryGetInvoiceType_WhenNullOrEmpty([Values(null, "")]string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("TD", value));

            // Act
            var result = parser.TryGetInvoiceType();
            var expected = InvoiceType.Other;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TryAdvancesSettlement_WhenValid([Values("0", "1")]string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("SA", value));

            // Act
            var result = parser.TryGetAdvancesSettlement();
            bool expected = value == "0" ? false : true;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TryAdvancesSettlement_WhenInvalid([Values("2", "asdm")]string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("SA", value));

            // Act
            var result = parser.TryGetAdvancesSettlement();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void TryAdvancesSettlement_WhenNullOrEmpty([Values(null, "")]string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("SA", value));

            // Act
            var result = parser.TryGetAdvancesSettlement();

            // Assert
            Assert.IsFalse(result);
        }

        private Dictionary<string, string> CreateTestCase(string key, string value)
        {
            return new Dictionary<string, string> { { key, value } };
        }
    }
}
