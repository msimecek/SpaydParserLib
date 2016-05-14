using SpaydParserLib.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                _errors.Add("Incorrect ID.");

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
                return 0; //TODO: neplatná hodnota
            }

            if (origin.Length > 18)
            {
                _errors.Add("AM is ignored: max length is exceeded");
                return 0; //TODO: neplatná hodnota
            }

            double num;
            var parseResult = double.TryParse(origin, NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out num);

            if (!parseResult)
            {
                _errors.Add("Amount (AM) value is invalid.");
                return num; //TODO: neplatná hodnota
            }

            return num;
        }

        public TaxPerformance? TryGetTaxPerformance()
        {
            string origin = _data.ContainsKey("TP") ? _data["TP"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return 0;
            }

            int originInt;
            var isValid = Int32.TryParse(origin, out originInt);

            if (!isValid || originInt > 2 || originInt < 0)
            {
                _errors.Add("Tax performance (TP) value is invalid. Must be 0, 1 or 2.");

                return 0;
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
                _errors.Add("Invoice type (TD) is invalid. Must be 0, 1, 2, 3, 4, 5 or 9.");
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
                _errors.Add("Advances settlement (SA) value is invalid. Must be 0 or 1.");
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
                _errors.Add("Variable symbol (VS) value is invalid. Value canot be parsed.");
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

        public DateTime? TryGetTaxPerformanceDate()
        {
            string origin = _data.ContainsKey("DUZP") ? _data["DUZP"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            DateTime date;
            bool isParsed = DateTime.TryParseExact(origin, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            if (!isParsed)
            {
                _errors.Add("Tax performance date (DUZP) value is invalid.");

                return null;
            }

            return date;
        }

        public DateTime? TryGetTaxStatementDueDate()
        {
            string origin = _data.ContainsKey("DPPD") ? _data["DPPD"] : null;

            if (string.IsNullOrEmpty(origin))
            {
                return null;
            }

            DateTime date;
            bool isParsed = DateTime.TryParseExact(origin, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            if (!isParsed)
            {
                _errors.Add("Tax statment due date (DPPD) value is invalid.");

                return null;
            }

            return date;
        }

        public DateTime? TryGetDueDate()
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
                _errors.Add("Due date (DT) value is invalid.");

                return null;
            }

            return date;
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


    }
}
