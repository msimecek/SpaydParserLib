using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
