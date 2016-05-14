using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpaydParserLib.Enums;
using System;
using System.Globalization;
using SpaydParserLib.Descriptors;

namespace SpaydParserLib.Test
{
    [TestFixture]
    public class SpaydParserTests
    {
        [Test]
        public void TryGetCurrency_WhenUnsupportedCurrency([Values("XYZ", "YA9", "9PQ")] string value)
        {
            // Arrange
            SpaydParser parser = new SpaydParser(CreateTestCase("CC", value));

            // Act
            var result = parser.TryGetCurrency();

            // Assert
            Assert.IsTrue(parser.GetErrors().Any());
            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [Test]
        public void TryGetCurrency_WhenValidCurrency([Values("CZK", "USD", "EUR")] string value)
        {
            // Arrange
            SpaydParser parser = new SpaydParser(CreateTestCase("CC", value));

            // Act
            var result = parser.TryGetCurrency();

            // Assert
            Assert.AreEqual(value, result);
        }

        [Test]
        public void TryGetAmount_WhenValidInteger()
        {
            // Arrange
            SpaydParser parser = new SpaydParser(CreateTestCase("AM", "154"));

            // Act
            var result = parser.TryGetAmount();

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(154, result);
        }

        [Test]
        public void TryGetAmount_WhenValidDouble()
        {
            // Arrange
            SpaydParser parser = new SpaydParser(CreateTestCase("AM", "00123.7890"));

            // Act
            var result = parser.TryGetAmount();

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(123.789, result);
        }

        [Test]
        public void TryGetNotificationChannel_WhenValidTypeOfP([Values("P", "p")] string value)
        {
            // Arrange
            SpaydParser parser = new SpaydParser(CreateTestCase("NT", value));

            // Act
            NotificationChannel? result = parser.TryGetNotificationChannel();

            // Assert
            Assert.AreEqual(NotificationChannel.Phone, result);
        }

        [Test]
        public void TryGetNotificationChannel_WhenValidTypeOfN([Values("E", "e")] string value)
        {
            // Arrange
            SpaydParser parser = new SpaydParser(CreateTestCase("NT", value));

            // Act
            NotificationChannel? result = parser.TryGetNotificationChannel();

            // Assert
            Assert.AreEqual(NotificationChannel.Email, result);
        }

        [Test]
        public void TryGetVariableSymbol_ErrorsIsEmpty_WhenValid([Values("000014", "255554", "911111111")] string value)
        {
            // Arrange
            SpaydParser parser = new SpaydParser(CreateTestCase("VS", value));

            // Act
            parser.TryGetVariableSymbol();

            // Assert
            Assert.IsTrue(parser.GetErrors().Count == 0);
        }

        [Test]
        public void TryGetVariableSymbol_IsValid_LongNumber()
        {
            // Arrange
            SpaydParser parser = new SpaydParser(CreateTestCase("X-VS", "9111111111"));

            // Act
            long? result = parser.TryGetVariableSymbol();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(9111111111, result.Value);
        }

        [Test]
        public void TryGetVariableSymbol_IsValid_NumberWithLeadingZeros()
        {
            // Arrange
            SpaydParser parser = new SpaydParser(CreateTestCase("X-VS", "000000098"));

            // Act
            long? result = parser.TryGetVariableSymbol();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(98, result.Value);
        }

        [Test]
        public void TryGetCurrency_WhenBadLength([Values("US", "U", "USDS")] string value)
        {
            // Arrange
            SpaydParser parser = new SpaydParser(CreateTestCase("CC", value));

            // Act
            string result = parser.TryGetCurrency();

            // Assert
            Assert.AreEqual(null, result);
            Assert.IsTrue(parser.GetErrors().Count > 0);
        }

        [Test]
        public void TryGetCurrency_WhenUnknown([Values("XXX", "YYY", "QQQ")] string value)
        {
            // Arrange
            SpaydParser parser = new SpaydParser(CreateTestCase("CC", value));

            // Act
            string result = parser.TryGetCurrency();

            // Assert
            Assert.AreEqual(null, result);
            Assert.IsTrue(parser.GetErrors().Count > 0);
        }

        [Test]
        public void TryGetCurrency_WhenValid([Values("CZK", "EUR", "USD")] string value)
        {
            // Arrange
            SpaydParser parser = new SpaydParser(CreateTestCase("CC", value));

            // Act
            string result = parser.TryGetCurrency();

            // Assert
            Assert.AreEqual(value, result);
            Assert.IsTrue(parser.GetErrors().Count == 0);
        }

        [Test]
        public void TryGetCurrency_WhenInvalid()
        {
            // Arrange
            SpaydParser parser = new SpaydParser(CreateTestCase("AV", "XYZ"));

            // Act
            string result = parser.TryGetCurrency();

            // Assert
            Assert.AreEqual(null, result);
            Assert.IsTrue(parser.GetErrors().Count == 0);
        }

        [Test]
        public void TryGetDate_WhenValid([Values("20161216", "19870101")] string value)
        {
            // Arrange
            var parser = new SpaydParser(CreateTestCase("DT", value));
            var expected = DateTime.ParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);

            // Act
            DateTime? result = parser.TryGetDate();

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsTrue(parser.GetErrors().Count == 0);
        }

        [Test]
        public void TryGetDate_WhenInvalid([Values("ABCD", "198720101")] string value)
        {
            // Arrange
            var parser = new SpaydParser(CreateTestCase("DT", value));

            // Act
            DateTime? result = parser.TryGetDate();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetJson()
        {
            // Arrange
            Spayd sp = Spayd.FromString("SPD*1.0*ACC:CZ2806000000000168540115*AM:450.00*CC:CZK*MSG:PLATBA ZA ZBOZI*X-VS:1234567890");
            string expected = "{\"ProtocolVersion\":null,\"BankAccount\":{\"AccountPrefix\":null,\"AccountNumber\":null,\"BankCode\":null,\"Iban\":\"CZ2806000000000168540115\",\"Bic\":null},\"Amount\":450.0,\"Currency\":\"CZK\",\"RecipientReference\":null,\"RecipientName\":null,\"Date\":null,\"PaymentType\":null,\"Message\":\"PLATBA ZA ZBOZI\",\"Crc32\":null,\"NotificationChannel\":null,\"NotificationContact\":null,\"PaymentRepeat\":0,\"Vs\":1234567890,\"Ss\":null,\"Ks\":null,\"Identifier\":null,\"Url\":null}";

            // Act
            var result = sp.GetJson();

            // Assert
            Assert.AreEqual(expected, result);
        }

        private Dictionary<string, string> CreateTestCase(string key, string value)
        {
            return new Dictionary<string, string> { {key, value} };
        }
    }
}
