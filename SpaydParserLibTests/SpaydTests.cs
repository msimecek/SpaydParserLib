using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaydParserLib.Enums;

namespace SpaydParserLib.Test
{
    [TestClass]
    public class SpaydTests
    {
        [TestMethod]
        public void Parse_Valid_SampleSpayd()
        {
            // prepare
            string spaydString = "SPD*1.0*ACC:CZ2806000000000168540115*AM:450.00*CC:CZK*MSG:PLATBA ZA ZBOZI*X-VS:1234567890";
            Spayd expected = new Spayd()
            {
                Amount = 450,
                Currency = "CZK",
                Message = "PLATBA ZA ZBOZI",
                Vs = 1234567890,
                NotificationChannel = NotificationChannel.Email,
                BankAccount = new BankAccount
                {
                    Iban = "CZ2806000000000168540115"
                }
            };

            // do
            Spayd result = Spayd.FromString(spaydString);

            // assert
            Assert.AreEqual(expected.BankAccount.GetHashCode(), result.BankAccount.GetHashCode());
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
