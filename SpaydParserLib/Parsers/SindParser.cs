﻿using SpaydParserLib.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpaydParserLib
{
    public class SindParser
    {
        private readonly List<string> _errors;
        private readonly Dictionary<string, string> _data;
        private readonly string[] _requiredKeys = new[] { "ID", "DD", "AM" };

        public SindParser(Dictionary<string, string> data)
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
                _errors.Add("Required parameters in SIND are missing: " + string.Join(", ", missingKeys));

                return false;
            }

            return true;
        }

        public string TryGetId()
        {
            string origin = _data.ContainsKey("ID") ? _data["ID"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                _errors.Add("ID cannot be empty.");

                return "";
            }

            if (origin.Length > 40)
            {
                _errors.Add("ID too long. Max length is 40 characters.");

                return "";
            }

            return origin;
        }

        public DateTime TryGetIssuedDate()
        {
            string origin = _data.ContainsKey("DD") ? _data["DD"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                _errors.Add("Issued date (DD) value cannot be empty.");

                return DateTime.MinValue;
            }

            DateTime date;
            bool isParsed = DateTime.TryParseExact(origin, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            if (!isParsed)
            {
                _errors.Add("Issued date (DD) value is invalid.");

                return DateTime.MinValue;
            }

            return date;
        }

        public double TryGetAmount()
        {
            string origin = _data.ContainsKey("AM") ? _data["AM"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                _errors.Add("Amount (AM) is required and cannot be null.");
                return 0; //TODO: neplatná hodnota
            }

            if (origin.Length > 18)
            {
                _errors.Add("Amount (AM) cannot be longer than 18 characters.");
                return 0; //TODO: neplatná hodnota
            }

            double num;
            var parseResult = double.TryParse(origin, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, new CultureInfo("en-US"), out num);

            if (!parseResult)
            {
                _errors.Add("Amount (AM) value is invalid.");
                return 0; //TODO: neplatná hodnota
            }

            return num;
        }

        public TaxPerformance? TryGetTaxPerformance()
        {
            string origin = _data.ContainsKey("TP") ? _data["TP"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return TaxPerformance.Common;
            }

            int originInt;
            var isValid = Int32.TryParse(origin, out originInt);

            if (!isValid || originInt > 2 || originInt < 0)
            {
                _errors.Add("Tax Performance (TP) value is invalid. Expected: 0, 1, 2 or nothing.");
                return null;
            }

            return (TaxPerformance)originInt;
        }

        public InvoiceType? TryGetInvoiceType()
        {
            var origin = _data.ContainsKey("TD") ? _data["TD"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return InvoiceType.Other;
            }

            int originInt;
            if (Int32.TryParse(origin, out originInt) && originInt >= 0 && (originInt == 9 || originInt <= 5))
            {
                return (InvoiceType)originInt;
            }
            else
            {
                _errors.Add("Invoice type (TD) is invalid. Must be 0, 1, 2, 3, 4, 5, 9 or nothing.");
                return null;
            }
        }

        public bool? TryGetAdvancesSettlement()
        {
            string origin = _data.ContainsKey("SA") ? _data["SA"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return false;
            }

            if (origin == "0")
            {
                return false;
            }
            else if (origin == "1")
            {
                return true;
            }
            else
            {
                _errors.Add("Advances settlement (SA) value is invalid. Must be 0, 1 or nothing.");
                return null;
            }
        }

        public long? TryGetVariableSymbol()
        {
            string origin = _data.ContainsKey("VS") ? _data["VS"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            if (origin.Length > 10)
            {
                _errors.Add("Variable symbol (VS) value is invalid. Max length is exceeded, should not be longer than 10 characters.");
                return null;
            }

            long symbol;
            bool isParsed = long.TryParse(origin, out symbol);

            if (!isParsed)
            {
                _errors.Add("Variable symbol (VS) value is invalid. Unable to parse the value.");
                return null;
            }

            return symbol;
        }

        public string TryGetIssuerVatIdentification()
        {
            string origin = _data.ContainsKey("VII") ? _data["VII"] : null;

            return origin;
        }

        public string TryGetIssuerIdentificationNumber()
        {
            string origin = _data.ContainsKey("INI") ? _data["INI"] : null;

            return origin;
        }

        public string TryGetRecipientVatIdentification()
        {
            string origin = _data.ContainsKey("VIR") ? _data["VIR"] : null;

            return origin;
        }

        public string TryGetRecipientIdentificationNumber()
        {
            string origin = _data.ContainsKey("INR") ? _data["INR"] : null;

            return origin;
        }

        public DateTime? TryGetDate(string key, string name)
        {
            string origin = _data.ContainsKey(key) ? _data[key] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            DateTime date;
            bool isParsed = DateTime.TryParseExact(origin, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            if (!isParsed)
            {
                _errors.Add($"{name} ({key}) value is invalid.");

                return null;
            }

            return date;
        }

        public double? TryGetT(string whichT)
        {
            string origin = _data.ContainsKey(whichT) ? _data[whichT] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            if (origin.Length > 18)
            {
                _errors.Add($"{whichT} value is invalid. Should not be longer than 18 characters.");

                return null;
            }

            double originDouble;
            bool parsed = double.TryParse(origin, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, new CultureInfo("en-US"), out originDouble);

            if (!parsed) //TODO: check for max 2 decimal digits
            {
                _errors.Add($"{whichT} value is invalid. Should be a decimal number max 18 characters long, with max 2 decimals.");

                return null;
            }

            return originDouble;
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

        public double? TryGetExchangeRate()
        {
            string origin = _data.ContainsKey("FX") ? _data["FX"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            if (origin.Length > 18)
            {
                _errors.Add("Exchange rate (FX) value is invalid. Should not be longer than 18 characters.");

                return null;
            }

            double originDouble;
            bool parsed = double.TryParse(origin, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, new CultureInfo("en-US"), out originDouble);

            if (!parsed) //TODO: check for max 3 decimal digits
            {
                _errors.Add("Exchange rate (FX) value is invalid. Should be a decimal number max 18 characters long, with max 3 decimals.");

                return null;
            }

            return originDouble;
        }

        public int TryGetForeignCurrencyAmount()
        {
            string origin = _data.ContainsKey("FXA") ? _data["FXA"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return 1;
            }

            int originInt;
            bool parsed = int.TryParse(origin, out originInt);

            if (!parsed)
            {
                _errors.Add("Foreign currency amount (FXA) value is invalid. Should be an integer.");

                return 1;
            }

            return originInt;
        }

        public BankAccount TryGetBankAccount()
        {
            string origin = _data.ContainsKey("ACC") ? _data["ACC"] : null;

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
                _errors.Add("Bank account (ACC) value is invalid. IBAN is not valid.");
                return null;
            }

            BankAccount bankAccount = new BankAccount { Iban = iban };

            if (ibanBic.Length == 2)
            {
                bankAccount.Bic = ibanBic[1];
            }

            return bankAccount;
        }

        public string TryGetSoftware()
        {
            string origin = _data.ContainsKey("X-SW") ? _data["X-SW"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            if (origin.Length > 30)
            {
                _errors.Add("Software (X-SW) value is invalid. Cannot be longer than 30 characters.");

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

            if (origin.Length > 70)
            {
                _errors.Add("URL (X-URL) value is invalid. Cannot be longer than 70 characters.");

                return null;
            }

            return origin;
        }
    }
}
