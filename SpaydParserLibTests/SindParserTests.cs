using NUnit.Framework;
using SpaydParserLib.Descriptors;
using SpaydParserLib.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SpaydParserLib.Test
{
    [TestFixture]
    public class SindParserTests
    {
        [Test]
        public void TryGetId_WhenValid([Values("ABCD123456789EF", "ABCD122aC3456789EF")] string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("ID", value));

            // Act
            var id = parser.TryGetId();

            // Assert
            Assert.AreEqual(value, id);
        }

        [Test]
        public void TryGetId_WhenNullOrEmpty([Values(null, "")] string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("ID", value));

            // Act
            var id = parser.TryGetId();

            // Assert
            Assert.IsTrue(parser.GetErrors().Count > 0);
        }

        [Test]
        public void TryGetId_WhenTooLong()
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("ID", new string('A', 41)));

            // Act
            var id = parser.TryGetId();

            // Assert
            Assert.IsTrue(parser.GetErrors().Count > 0);
        }

        [Test]
        public void TryGetIssuedDate_WhenValid()
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("DD", "20160615"));
            DateTime expected = new DateTime(2016, 6, 15);

            // Act
            var result = parser.TryGetIssuedDate();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TryGetIssuedDate_WhenNullOrEmpty([Values(null, "")] string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("DD", value));

            // Act
            var result = parser.TryGetIssuedDate();

            // Assert
            Assert.IsTrue(parser.GetErrors().Count > 0);
        }

        [Test]
        public void TryGetIssuedDate_WhenInvalidDate()
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("DD", "2016adc12"));

            // Act
            var result = parser.TryGetIssuedDate();

            // Assert
            Assert.IsTrue(parser.GetErrors().Count > 0);
        }

        [Test]
        public void TryGetAmount_WhenValid()
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("AM", "123456789.45"));
            double expected = 123456789.45;

            // Act
            var result = parser.TryGetAmount();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TryGetAmount_WhenValidNegative()
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("AM", "-123456789.45"));
            double expected = -123456789.45;

            // Act
            var result = parser.TryGetAmount();

            // Assert
            Assert.AreEqual(expected, result);
        }

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
        public void TryGetAmount_WhenNullOrEmpty([Values(null, "")] string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("AM", value));

            // Act
            var result = parser.TryGetAmount();

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
        public void TryGetTaxPerformance_WhenNullOrEmpty([Values(null, "")]string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("TP", value));

            // Act
            var result = parser.TryGetTaxPerformance();

            // Assert
            Assert.AreEqual(TaxPerformance.Common, result);
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
            Assert.IsTrue(parser.GetErrors().Count > 0);
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

        [Test]
        public void TryGetVariableSymbol_WhenValid([Values("1234567890")]string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("VS", value));

            // Act
            var result = parser.TryGetVariableSymbol();
            long expected = 1234567890;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TryGetVariableSymbol_WhenInvalid([Values("ABCDE", "123456789012", "123.22")]string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("VS", value));

            // Act
            var result = parser.TryGetVariableSymbol();

            // Assert
            Assert.IsTrue(parser.GetErrors().Count > 0);
            Assert.IsNull(result);
        }

        [Test]
        public void TryGetIssuerVatIdentification_WhenValid([Values("CZ12345678", "CY99999999L", "DK99999999")]string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("VII", value));

            // Act
            var result = parser.TryGetIssuerVatIdentification();
            string expected = value;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TryGetIssuerIdentificationNumber_WhenValid([Values("12345678")]string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("INI", value));

            // Act
            var result = parser.TryGetIssuerIdentificationNumber();
            string expected = value;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TryGetRecipientVatIdentification_WhenValid([Values("CZ12345678", "CY99999999L", "DK99999999")]string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("VIR", value));

            // Act
            var result = parser.TryGetRecipientVatIdentification();
            string expected = value;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TryGetRecipientIdentificationNumber_WhenValid([Values("12345678")]string value)
        {
            // Arrange
            SindParser parser = new SindParser(CreateTestCase("INR", value));

            // Act
            var result = parser.TryGetRecipientIdentificationNumber();
            string expected = value;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TryGetTaxPerformanceDate_WhenValid()
        {
            // Arrange
            var parser = new SindParser(CreateTestCase("DUZP", "20161216"));
            var expected = new DateTime(2016, 12, 16);

            // Act
            DateTime? result = parser.TryGetDate("DUZP", "Tax performance date");

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsTrue(parser.GetErrors().Count == 0);
        }

        [Test]
        public void TryGetTaxPerformanceDate_WhenInvalid([Values("ABCD", "198720101")] string value)
        {
            // Arrange
            var parser = new SindParser(CreateTestCase("DUZP", value));

            // Act
            DateTime? result = parser.TryGetDate("DUZP", "Tax performance date");

            // Assert
            Assert.IsNull(result);
            Assert.IsTrue(parser.GetErrors().Count > 0);
        }

        [Test]
        public void TryGetTaxStatementDueDate_WhenValid()
        {
            // Arrange
            var parser = new SindParser(CreateTestCase("DPPD", "20161201"));
            var expected = new DateTime(2016, 12, 01);

            // Act
            DateTime? result = parser.TryGetDate("DPPD", "Tax statement due date");

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsTrue(parser.GetErrors().Count == 0);
        }

        [Test]
        public void TryGetTaxStatementDueDate_WhenInvalid([Values("ABCD", "198720101")] string value)
        {
            // Arrange
            var parser = new SindParser(CreateTestCase("DPPD", value));

            // Act
            DateTime? result = parser.TryGetDate("DPPD", "Tax statement due date");

            // Assert
            Assert.IsNull(result);
            Assert.IsTrue(parser.GetErrors().Count > 0);
        }

        [Test]
        public void TryGetDueDate_WhenValid()
        {
            // Arrange
            var parser = new SindParser(CreateTestCase("DT", "20161201"));
            var expected = new DateTime(2016, 12, 01);

            // Act
            DateTime? result = parser.TryGetDate("DT", "Due date");

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsTrue(parser.GetErrors().Count == 0);
        }

        [Test]
        public void TryGetDueDate_WhenInvalid([Values("ABCD", "198720101")] string value)
        {
            // Arrange
            var parser = new SindParser(CreateTestCase("DT", value));

            // Act
            DateTime? result = parser.TryGetDate("DT", "Due date");

            // Assert
            Assert.IsNull(result);
            Assert.IsTrue(parser.GetErrors().Count > 0);
        }

        private Dictionary<string, string> CreateTestCase(string key, string value)
        {
            return new Dictionary<string, string> { { key, value } };
        }
    }
}
