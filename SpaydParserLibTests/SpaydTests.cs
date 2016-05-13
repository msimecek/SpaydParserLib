using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaydParserLib.Enums;

namespace SpaydParserLib.Test
{
    [TestClass]
    public class SpaydTests
    {
        [TestMethod]
        public void Parse_Valid_MinimalisticSpayd()
        {
            // prepare
            string spaydString = "SPD*1.0*ACC:CZ2806000000000168540115";
            Spayd expected = new Spayd()
            {
                BankAccount = new BankAccount
                {
                    Iban = "CZ2806000000000168540115"
                }
            };

            // do
            Spayd result = Spayd.FromString(spaydString);

            // assert
            Assert.AreEqual(expected.BankAccount.AccountNumber, result.BankAccount.AccountNumber);
        }


        [TestMethod]
        public void Parse_Valid_MinimalSpayd()
        {

        }

        [TestMethod]
        public void Parse_Valid_FullSpayd()
        {

        }

        [TestMethod]
        public void Parse_Mixed()
        {
            // Arrange
            string spaydString = "SPD*1.0*AM:9535.00*X-VS:1234567890*DT:20161216*CC:CZK*ACC:CZ3103000000270016060243*X-INV:SID%2A1.0%2AID:1963/160/2015%2ADD:20161201%2ATP:0%2AVII:CZ60194383%2AVIR:CZ12345678%2AINI:60194383%2ADUZP:20161201%2ATB0:1000.00%2AT0:210.00%2ATB1:6500.00%2AT1:975.00%2ANTB:850.00*";
            Spayd expected = new Spayd()
            {
                Amount = 9535,
                Vs = 1234567890,
                Date = new System.DateTime(2016, 12, 16),
                Currency = "CZK",
                BankAccount = new BankAccount()
                {
                    Iban = "CZ3103000000270016060243"
                }
            };

            // Act
            var result = Spayd.FromString(spaydString);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFormatException))]
        public void Parse_Invalid_MissingSPD()
        {
            // prepare
            string spaydString = "1.0*ACC:CZ2806000000000168540115*AM:450.00*CC:CZK*MSG:PLATBA ZA ZBOZI*X-VS:1234567890";

            // do
            Spayd result = Spayd.FromString(spaydString);
        }

        [TestMethod]
        public void Parse_Invalid_MinimalSpayd()
        {

        }

        [TestMethod]
        public void Parse_Invalid_FullSpayd()
        {

        }
    }
}
