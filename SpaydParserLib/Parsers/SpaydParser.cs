using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SpaydParserLib.Enums;

namespace SpaydParserLib
{
    public class SpaydParser
    {
        private readonly List<string> _errors;
        private readonly Dictionary<string, string> _data;
        private readonly string[] _requiredKeys = new[] { "ACC" };

        public SpaydParser(Dictionary<string, string> data)
        {
            _data = data;
            _errors = new List<string>();
        }

        public List<string> GetErrors()
        {
            return _errors;
        }

        public bool ContainsAllRequiredKeys()
        {
            List<string> missingKeys = _requiredKeys.Where(x => !_data.ContainsKey(x)).ToList();

            if (missingKeys.Any())
            {
                _errors.Add("Some required parameter in SPAYD is missing: " + string.Join(", ", missingKeys));

                return false;
            }

            return true;
        }

        public bool IsCrcValid()
        {
            throw new NotImplementedException();
        }

        public BankAccount TryGetBankAccount()
        {
            string origin = _data.ContainsKey("ACC") ? _data["ACC"]
                : (_data.ContainsKey("ALT-ACC") ? _data["ALT-ACC"] : string.Empty);

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            string[] fragments = origin.Split(',');
            var ibanWithBic = fragments[0];

            string[] ibanBic = ibanWithBic.Split('+');

            string iban = ibanBic[0];
            if (!IbanValidator.Validate(iban))
            {
                _errors.Add("IBAN is not valid");
                return null;
            }

            BankAccount bankAccount = new BankAccount {Iban = iban};

            if (ibanBic.Length == 2)
            {
                bankAccount.Bic = ibanBic[1];
            }

            return bankAccount;
        }

        public long? TryGetVariableSymbol()
        {
            string origin = _data.ContainsKey("X-VS") ? _data["X-VS"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            if (origin.Length > 10)
            {
                _errors.Add("VS is ignored: max length is exceeded");
                return null;
            }

            long symbol;
            bool isParsed = long.TryParse(origin, out symbol);

            if (!isParsed)
            {
                _errors.Add("VS is skipped: value canot be parsed");
                return null;
            }

            return symbol;
        }

        public long? TryGetSpecificSymbol()
        {
            string origin = _data.ContainsKey("X-SS") ? _data["X-SS"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            if (origin.Length > 10)
            {
                _errors.Add("SS is ignored: max length is exceeded");
                return null;
            }

            long symbol;
            bool isParsed = long.TryParse(origin, out symbol);

            if (!isParsed)
            {
                _errors.Add("SS is skipped: value canot be parsed");
                return null;
            }

            return symbol;
        }

        public long? TryGetConstantSymbol()
        {
            string origin = _data.ContainsKey("X-KS") ? _data["X-KS"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            if (origin.Length > 10)
            {
                _errors.Add("KS is ignored: max length is exceeded");
                return null;
            }

            long symbol;
            bool isParsed = long.TryParse(origin, out symbol);

            if (!isParsed)
            {
                _errors.Add("KS is skipped: value canot be parsed");
                return null;
            }

            return symbol;
        }

        public double? TryGetAmount()
        {
            string origin = _data.ContainsKey("AM") ? _data["AM"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            if (origin.Length > 10)
            {
                _errors.Add("AM is ignored: max length is exceeded");
                return null;
            }

            double num;
            double.TryParse(origin, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, new CultureInfo("en-US"), out num);

            return num;
        }

        public string TryGetCurrency()
        {
            string origin = _data.ContainsKey("CC") ? _data["CC"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            if (origin.Length != 3)
            {
                _errors.Add("CC is ignored: string length should be exactly 3 chars");

                return null;
            }

            if (!StaticLists.Iso4217Currencies.Select(x => x.ToLowerInvariant()).Contains(origin.ToLowerInvariant()))
            {
                _errors.Add("CC is ignored: currency code is not valid ISO 4217 currency code");

                return null;
            }

            return origin;
        }

        public long? TryGetRecipientReference()
        {
            string origin = _data.ContainsKey("RF") ? _data["RF"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            if (origin.Length > 16)
            {
                _errors.Add("RF is ignored: max length is exceeded");
                return null;
            }

            long symbol;
            bool isParsed = long.TryParse(origin, out symbol);

            if (!isParsed)
            {
                _errors.Add("RF is skipped: value canot be parsed");
                return null;
            }

            return symbol;
        }

        public string TryGetRecipientName()
        {
            string origin = _data.ContainsKey("RN") ? _data["RN"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            if (origin.Length > 35)
            {
                _errors.Add("RN is skipped: value canot be longer than 35 characters");

                return null;
            }

            return origin;
        }

        public string TryGetId()
        {
            string origin = _data.ContainsKey("X-ID") ? _data["X-ID"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            if (origin.Length > 20)
            {
                _errors.Add("X-ID is skipped: value canot be longer than 20 characters");

                return null;
            }

            return origin;
        }

        public string TryGetUrl()
        {
            string origin = _data.ContainsKey("X-URL") ? _data["X-URL"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            return origin;
        }

        public DateTime? TryGetDate()
        {
            string origin = _data.ContainsKey("DT") ? _data["DT"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            DateTime date;
            bool isParsed = DateTime.TryParseExact(origin, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            if (!isParsed)
            {
                return null;
            }

            return date;
        }

        public string TryGetPaymentType()
        {
            string origin = _data.ContainsKey("PT") ? _data["PT"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            if (origin.Length > 3)
            {
                _errors.Add("PT is skipped: value canot be longer than 3 characters");

                return null;
            }

            return origin;
        }

        public string TryGetMessage()
        {
            string origin = _data.ContainsKey("MSG") ? _data["MSG"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            if (origin.Length > 60)
            {
                _errors.Add("MSG is skipped: value canot be longer than 60 characters");
                return null;
            }

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            return origin;
        }

        public string TryGetCrc32()
        {
            string origin = _data.ContainsKey("CRC32") ? _data["CRC32"] : null;

            return origin;
        }

        public NotificationChannel? TryGetNotificationChannel()
        {
            string origin = _data.ContainsKey("NT") ? _data["NT"] : null;

            if (string.IsNullOrEmpty(origin) || origin.Length != 1)
            {
                return null;
            }

            char originChar = origin.ToUpperInvariant()[0];

            return (NotificationChannel) originChar;
        }

        public string TryGetNotificationContact()
        {
            string origin = _data.ContainsKey("NTA") ? _data["NTA"] : null;

            var channel = TryGetNotificationChannel();

            if (channel == NotificationChannel.Phone || channel == NotificationChannel.Email)
            {
                return origin;
            }

            //_errors.Add("NTA is skipped: related NotificationChannel is unknown");

            return null;
        }

        public int TryGetPaymentRepeat()
        {
            string origin = _data.ContainsKey("X-PER") ? _data["X-PER"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return 0;
            }

            int result;
            if (int.TryParse(origin, out result))
            {
                if (result > 30)
                {
                    return 30;
                }

                return result;
            }

            return 0;
        }
    }
}
