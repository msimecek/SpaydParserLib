using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaydParserLib.Descriptors;

namespace SpaydParserLib.Test
{
    [TestClass]
    public class SindTests
    {
        [TestMethod]
        public void Parse_Valid()
        {
            // Arrange
            string source = "SID*1.0*ID:012150672*DD:20151201*TP:0*AM:495.00*VS:012150672*VII:CZ60194383*INI:60194383*VIR:CZ12345678*DUZP:20151201*DT:20151217*TB0:409.00*T0:85.91*CC:CZK*ACC:CZ3103000000270016060243*";
            Sind expected = new Sind()
            {
                Id = "012150672",
                IssuedDate = new DateTime(2015, 12, 1),
                TaxPerformance = Enums.TaxPerformance.Common,
                Amount = 495,
                Vs = 012150672,
                IssuerVatIdentification = "CZ60194383",
                IssuerIdentificationNumber = "60194383",
                RecipientVatIdentification = "CZ12345678",
                TaxPerformanceDate = new DateTime(2015, 12, 1),
                DueDate = new DateTime(2015, 12, 17),
                TB0 = 409.00,
                T0 = 85.91,
                Currency = "CZK",
                BankAccount = new BankAccount()
                {
                    Iban = "CZ3103000000270016060243"
                }
            };

            // Act
            Sind result = Sind.FromString(source);

            // Assert
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.IssuedDate, result.IssuedDate);
            Assert.AreEqual(expected.TaxPerformance, result.TaxPerformance);
            Assert.AreEqual(expected.Amount, result.Amount);
            Assert.AreEqual(expected.Vs, result.Vs);
            Assert.AreEqual(expected.IssuerVatIdentification, result.IssuerVatIdentification);
            Assert.AreEqual(expected.IssuerIdentificationNumber, result.IssuerIdentificationNumber);
            Assert.AreEqual(expected.RecipientVatIdentification, result.RecipientVatIdentification);
            Assert.AreEqual(expected.TaxPerformanceDate, result.TaxPerformanceDate);
            Assert.AreEqual(expected.DueDate, result.DueDate);
            Assert.AreEqual(expected.TB0, result.TB0);
            Assert.AreEqual(expected.T0, result.T0);
            Assert.AreEqual(expected.Currency, result.Currency);
            Assert.AreEqual(expected.BankAccount.Iban, result.BankAccount.Iban);
        }

        [TestMethod]
        public void Parse_Valid_Max()
        {
            /*
             SID*1.0*ID:2001401154*DD:20140404*TP:9*AM:61189.00*VS:3310001054*VII:CZ25568736*INI:25568736*INR:25568736*VIR:CZ25568736*DUZP:20140404*DT:20140412*TB0:26492.70*T0:5563.47*TB1:25333.10*T1:3799.97*NTB:-0.24*CC:CZK*TD:0*SA:0*ACC:CZ9701000000007098760287+KOMBCZPP*X-SW:MoneyS5-1.7.1* 
             */

            // Arrange
            string source = "SID*1.0*ID:2001401154*DD:20140404*TP:0*AM:61189.00*VS:3310001054*VII:CZ25568736*INI:25568736*INR:25568736*VIR:CZ25568736*DUZP:20140404*DT:20140412*TB0:26492.70*T0:5563.47*TB1:25333.10*T1:3799.97*NTB:-0.24*CC:CZK*TD:0*SA:0*ACC:CZ9701000000007098760287+KOMBCZPP*X-SW:MoneyS5-1.7.1*";
            Sind expected = new Sind()
            {
                Id = "2001401154",
                IssuedDate = new DateTime(2014, 4, 4),
                TaxPerformance = Enums.TaxPerformance.Common,
                Amount = 61189,
                Vs = 3310001054,
                IssuerVatIdentification = "CZ25568736",
                IssuerIdentificationNumber = "25568736",
                RecipientIdentificationNumber = "25568736",
                RecipientVatIdentification = "CZ25568736",
                TaxPerformanceDate = new DateTime(2014, 4, 4),
                DueDate = new DateTime(2014, 4, 12),
                TB0 = 26492.70,
                T0 = 5563.47,
                TB1 = 25333.10,
                T1 = 3799.97,
                NTB = -0.24,
                Currency = "CZK",
                InvoiceType = Enums.InvoiceType.NonTax,
                AdvancesSettlement = false,
                BankAccount = new BankAccount()
                {
                    Iban = "CZ9701000000007098760287",
                    Bic = "KOMBCZPP"
                },
                Software = "MoneyS5-1.7.1"
            };

            // Act
            Sind result = Sind.FromString(source);

            // Assert
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.IssuedDate, result.IssuedDate);
            Assert.AreEqual(expected.TaxPerformance, result.TaxPerformance);
            Assert.AreEqual(expected.Amount, result.Amount);
            Assert.AreEqual(expected.Vs, result.Vs);
            Assert.AreEqual(expected.IssuerVatIdentification, result.IssuerVatIdentification);
            Assert.AreEqual(expected.IssuerIdentificationNumber, result.IssuerIdentificationNumber);
            Assert.AreEqual(expected.RecipientVatIdentification, result.RecipientVatIdentification);
            Assert.AreEqual(expected.RecipientIdentificationNumber, result.RecipientIdentificationNumber);
            Assert.AreEqual(expected.TaxPerformanceDate, result.TaxPerformanceDate);
            Assert.AreEqual(expected.DueDate, result.DueDate);
            Assert.AreEqual(expected.TB0, result.TB0);
            Assert.AreEqual(expected.T0, result.T0);
            Assert.AreEqual(expected.TB1, result.TB1);
            Assert.AreEqual(expected.T1, result.T1);
            Assert.AreEqual(expected.NTB, result.NTB);
            Assert.AreEqual(expected.Currency, result.Currency);
            Assert.AreEqual(expected.InvoiceType, result.InvoiceType);
            Assert.AreEqual(expected.AdvancesSettlement, result.AdvancesSettlement);
            Assert.AreEqual(expected.BankAccount.Iban, result.BankAccount.Iban);
            Assert.AreEqual(expected.BankAccount.Bic, result.BankAccount.Bic);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFormatException))]
        public void Parse_IdMissing()
        {
            // Arrange
            string source = "SID*1.0*DD:20140404*TP:0*AM:61189.00*VS:3310001054*VII:CZ25568736*INI:25568736*INR:25568736*VIR:CZ25568736*DUZP:20140404*DT:20140412*TB0:26492.70*T0:5563.47*TB1:25333.10*T1:3799.97*NTB:-0.24*CC:CZK*TD:0*SA:0*ACC:CZ9701000000007098760287+KOMBCZPP*X-SW:MoneyS5-1.7.1*";

            // Act
            var result = Sind.FromString(source);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFormatException))]
        public void Parse_IssedDateMissing()
        {
            // Arrange
            string source = "SID*1.0*ID:2001401154*TP:0*AM:61189.00*VS:3310001054*VII:CZ25568736*INI:25568736*INR:25568736*VIR:CZ25568736*DUZP:20140404*DT:20140412*TB0:26492.70*T0:5563.47*TB1:25333.10*T1:3799.97*NTB:-0.24*CC:CZK*TD:0*SA:0*ACC:CZ9701000000007098760287+KOMBCZPP*X-SW:MoneyS5-1.7.1*";

            // Act
            var result = Sind.FromString(source);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFormatException))]
        public void Parse_AmountMissing()
        {
            // Arrange
            string source = "SID*1.0*ID:2001401154*DD:20140404*TP:0*VS:3310001054*VII:CZ25568736*INI:25568736*INR:25568736*VIR:CZ25568736*DUZP:20140404*DT:20140412*TB0:26492.70*T0:5563.47*TB1:25333.10*T1:3799.97*NTB:-0.24*CC:CZK*TD:0*SA:0*ACC:CZ9701000000007098760287+KOMBCZPP*X-SW:MoneyS5-1.7.1*";

            // Act
            var result = Sind.FromString(source);
        }

        [TestMethod]
        public void GetJson()
        {
            // Arrange
            Sind sp = Sind.FromString("SID*1.0*ID:2001401154*DD:20140404*TP:0*AM:61189.00*VS:3310001054*VII:CZ25568736*INI:25568736*INR:25568736*VIR:CZ25568736*DUZP:20140404*DT:20140412*TB0:26492.70*T0:5563.47*TB1:25333.10*T1:3799.97*NTB:-0.24*CC:CZK*TD:0*SA:0*ACC:CZ9701000000007098760287+KOMBCZPP*X-SW:MoneyS5-1.7.1");
            string expected = "{\"ProtocolVersion\":\"1.0\",\"Id\":\"2001401154\",\"IssuedDate\":\"2014-04-04T00:00:00\",\"Amount\":61189.0,\"TaxPerformance\":0,\"InvoiceType\":0,\"AdvancesSettlement\":false,\"Vs\":3310001054,\"IssuerVatIdentification\":\"CZ25568736\",\"IssuerIdentificationNumber\":\"25568736\",\"RecipientVatIdentification\":\"CZ25568736\",\"RecipientIdentificationNumber\":\"25568736\",\"TaxPerformanceDate\":\"2014-04-04T00:00:00\",\"TaxStatementDueDate\":null,\"DueDate\":\"2014-04-12T00:00:00\",\"TB0\":26492.7,\"T0\":5563.47,\"TB1\":25333.1,\"T1\":3799.97,\"TB2\":null,\"T2\":null,\"NTB\":-0.24,\"Currency\":\"CZK\",\"ExchangeRate\":null,\"ForeignCurrencyAmount\":1,\"BankAccount\":{\"AccountPrefix\":null,\"AccountNumber\":null,\"BankCode\":null,\"Iban\":\"CZ9701000000007098760287\",\"Bic\":\"KOMBCZPP\"},\"Software\":\"MoneyS5-1.7.1\",\"Url\":null}";

            // Act
            var result = sp.GetJson();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
