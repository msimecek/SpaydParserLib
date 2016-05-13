using System;
using NUnit.Framework;

namespace SpaydParserLib.Test
{
    [TestFixture]
    public class IbanValidatorTests
    {
        [Test]
        public void Validate_WhenValidElectronicIban([Values("CZ6508000000192000145399", "de44500105175407324931", "TR330006100519786457841326")] string iban)
        {
            // Prepare

            // Do
            var result = IbanValidator.Validate(iban);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Validate_WhenValidPaperIban([Values("CZ65 0800 0000 1920 0014 5399", "gb29 NWBK 6016 1331 9268 19", "TR33 0006 1005 1978 6457 8413 26")] string iban)
        {
            // Prepare

            // Do
            var result = IbanValidator.Validate(iban);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Validate_WhenInvalidCountry([Values("GG65 0800 0000 1920 0014 5399", "CM29 NWBK 6016 1331 9268 19", "TQ33 0006 1005 1978 6457 8413 26")] string iban)
        {
            // Prepare

            // Do
            var result = IbanValidator.Validate(iban);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Validate_WhenInvalidLength([Values("GG65 0800 0000 1920 0014 5399 14", "CM29 NWBK 6016 1331 9268 19 AAC", "TQ33 0006 1005 1978 6457 8413")] string iban)
        {
            // Prepare

            // Do
            var result = IbanValidator.Validate(iban);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Validate_WhenInvalidChecksum([Values("GG64 0800 0000 1920 0014 5399 14", "CMC2 NWBK 6016 1331 9268 19 AAC", "TQ73 0006 1005 1978 6457 8413")] string iban)
        {
            // Prepare

            // Do
            var result = IbanValidator.Validate(iban);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
