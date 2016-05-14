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
            DateTime? result = parser.TryGetTaxPerformanceDate();

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
            DateTime? result = parser.TryGetTaxPerformanceDate();

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
            DateTime? result = parser.TryGetTaxStatementDueDate();

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
            DateTime? result = parser.TryGetTaxStatementDueDate();

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
            DateTime? result = parser.TryGetDueDate();

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
            DateTime? result = parser.TryGetDueDate();

            // Assert
            Assert.IsNull(result);
            Assert.IsTrue(parser.GetErrors().Count > 0);
        }

        [Test]
        public void GetJson()
        {
            // Arrange
            Sind sp = Sind.FromString("SID*1.0*ID:2001401154*DD:20140404*TP:9*AM:61189.00*VS:3310001054*VII:CZ25568736*INI:25568736*INR:25568736*VIR:CZ25568736*DUZP:20140404*DT:20140412*TB0:26492.70*T0:5563.47*TB1:25333.10*T1:3799.97*NTB:-0.24*CC:CZK*TD:0*SA:0*ACC:CZ9701000000007098760287+KOMBCZPP*X-SW:MoneyS5-1.7.1");
            string expected = "{\"ProtocolVersion\":\"1.0\",\"Id\":\"2001401154\",\"IssuedDate\":\"2014-04-04T00:00:00\",\"Amount\":61189.0,\"TaxPerformance\":0,\"InvoiceType\":0,\"AdvancesSettlement\":false,\"Vs\":3310001054,\"IssuerVatIdentification\":\"CZ25568736\",\"IssuerIdentificationNumber\":\"25568736\",\"RecipientVatIdentification\":\"CZ25568736\",\"RecipientIdentificationNumber\":\"25568736\",\"TaxPerformanceDate\":\"2014-04-04T00:00:00\",\"TaxStatementDueDate\":null,\"DueDate\":\"2014-04-12T00:00:00\",\"TB0\":26492.7,\"T0\":5563.47,\"TB1\":25333.1,\"T1\":3799.97,\"TB2\":null,\"T2\":null,\"NTB\":-0.24,\"Currency\":\"CZK\",\"ExchangeRate\":null,\"ForeignCurrencyAmount\":1,\"BankAccount\":{\"AccountPrefix\":null,\"AccountNumber\":null,\"BankCode\":null,\"Iban\":\"CZ9701000000007098760287\",\"Bic\":\"KOMBCZPP\"},\"Software\":\"MoneyS5-1.7.1\",\"Url\":null}";

            // Act
            var result = sp.GetJson();

            // Assert
            Assert.AreEqual(expected, result);
        }

        private Dictionary<string, string> CreateTestCase(string key, string value)
        {
            return new Dictionary<string, string> { { key, value } };
        }
    }
}
