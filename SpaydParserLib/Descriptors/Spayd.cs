using System;
using System.Collections.Generic;
using System.Linq;
using SpaydParserLib.Enums;
using Newtonsoft.Json;

namespace SpaydParserLib
{
    // Short Payment Descriptor
    public class Spayd
    {
        // 1.0*
        // number-dot-number
        // required
        public string ProtocolVersion { get; set; }

        // ACC:CZ5855000000001265098001+RZBCCZPP*
        // max 46 chars
        // required
        public BankAccount BankAccount { get; set; }

        // AM:480.55*
        // max 10 chars
        public double? Amount { get; set; }

        // CC:CZK*
        // 3 chars
        public string Currency { get; set; }

        // RF:1234567890123456*
        // max 16 chars
        public long? RecipientReference { get; set; }

        // RN:PETR DVORAK*
        // max 35 chars, any from permitted chars but '*'
        public string RecipientName { get; set; }

        // DT:20121231*
        // 8 numbers, YYYYMMDD (ISO 8601)
        public DateTime? Date { get; set; }

        // PT:P2P*
        // max 3 chars, any from permitted chars but '*'
        public string PaymentType { get; set; }

        // MSG:PLATBA ZA ELEKTRINU*
        // max 60 chars, any from permitted chars but '*'
        public string Message { get; set; }

        // CRC32:1234ABCD*
        // 8 chars, [A-F0-9]
        public string Crc32 { get; set; }

        // NT:P
        // 1 char, P or E
        public NotificationChannel? NotificationChannel { get; set; }

        // NTA:00420123456789
        // NTA:+420123456789
        // NTA:123456789
        // NTA:ales.dynda@abc.cz
        // max +N[12] for NT:P, max e-mailAddress[64]@domainName[255] for NT:E
        public string NotificationContact { get; set; }

        // X-PER:7*
        // max value 30, min value 0
        public int PaymentRepeat { get; set; }

        // X-VS:1234567890*
        // max 10 numbers
        public long? Vs { get; set; }

        // X-SS:1234567890*
        // max 10 numbers
        public long? Ss { get; set; }

        // X-KS:1234567890*
        // max 10 numbers
        public long? Ks { get; set; }

        // X-ID:ABCDEFGHIJ1234567890*
        // max 20 chars, any from permitted chars but '*'
        public string Identifier { get; set; }

        // X-URL:HTTP://WWW.SOMEURL.COM/*
        // max 140 chars, any from permitted chars but '*'
        public string Url { get; set; }


        public static Spayd FromString(string spaydData)
        {
            if (!spaydData.StartsWith("SPD"))
            {
                throw new InvalidFormatException("Required parameter SPD not found");
            }

            spaydData = spaydData.Replace("SPD*", string.Empty);

            Dictionary<string, string> data = spaydData.ExtractValuePairs('*', ':', true);

            var parser = new SpaydParser(data);

            if (!parser.ContainsAllRequiredKeys())
            {
                throw new InvalidFormatException("Required parameters in SPAYD not found.");
            }

            var spayd = new Spayd
            {
                Vs = parser.TryGetVariableSymbol(),
                Amount = parser.TryGetAmount(),
                NotificationChannel = parser.TryGetNotificationChannel(),
                BankAccount = parser.TryGetBankAccount(),
                Crc32 = parser.TryGetCrc32(),
                Currency = parser.TryGetCurrency(),
                Date = parser.TryGetDate(),
                Identifier = parser.TryGetId(),
                Ks = parser.TryGetConstantSymbol(),
                Message = parser.TryGetMessage(),
                NotificationContact = parser.TryGetNotificationContact(),
                PaymentRepeat = parser.TryGetPaymentRepeat(),
                PaymentType = parser.TryGetPaymentType(),
                RecipientName = parser.TryGetRecipientName(),
                RecipientReference = parser.TryGetRecipientReference(),
                Ss = parser.TryGetSpecificSymbol(),
                Url = parser.TryGetUrl(),
                ProtocolVersion = null
            };

            var errors = parser.GetErrors();
            if (errors.Any())
            {
                throw new InvalidFormatException(string.Join(", ", errors));
            }

            return spayd;
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string GetXml()
        {
            throw new NotImplementedException();
        }

        public string GetValidationError()
        {
            throw new NotImplementedException();
        }
    }
}
