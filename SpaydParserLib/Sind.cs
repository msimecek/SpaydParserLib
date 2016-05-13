using SpaydParserLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaydParserLib
{
    // Short Invoice Descriptor
    public class Sind
    {
        // 1.0*
        // number-dot-number
        // required
        public string ProtocolVersion { get; set; }

        // ID:ABCD123456789EF*
        // max 40 chars
        // required
        public string Id { get; set; }

        // DD:20160615*
        // 8 chars, ISO 8601 (YYYYMMDD)
        // required
        public DateTime IssuedDate { get; set; }

        // AM:123456789.45*
        // max 18 numbers total, max 2 decimal, dot delimiter
        // required
        public double Amount { get; set; }

        // TP:0*
        // 1 number
        // default = 0
        public TaxPerformance? TaxPerformance { get; set; } // daňové plnění

        // TD:9*
        // 1 number
        // default = 9
        public InvoiceType? InvoiceType { get; set; } // typ dokladu

        // SA:1*
        // 1 number
        // default = false
        public bool? AdvancesSettlement { get; set; } // zůčtování záloh

        // VS:1234567890*
        // max 10 numbers
        public long? Vs { get; set; }

        // VII:CZ12345678*
        // according to national standards
        public string IssuerVatIdentification { get; set; } // DIČ výstavce

        // INI:12345678*
        // according to national standards
        public string IssuerIdentificationNumber { get; set; } // IČO výstavce

        // VIR:CZ09876543*
        // according to national standards
        public string RecipientVatIdentification { get; set; } // DIČ příjemce

        // INR:98765432*
        // according to national standards
        public string RecipientIdentificationNumber { get; set; } // IČO příjemce

        // DUZP:20160413*
        // 8 chars, ISO 8601 (YYYYMMDD)
        public DateTime? TaxPerformanceDate { get; set; } // datum uskutečnitelného zdanitelného plnění

        // DPPD:20161201*
        // 8 chars, ISO 8601 (YYYYMMDD)
        public DateTime? TaxStatementDueDate { get; set; } // datum povinnosti přiznat daň

        // DT:20160908*
        // 8 chars, ISO 8601 (YYYYMMDD)
        public DateTime? DueDate { get; set; } // datum splatnosti celkové částky

        // TB0:3000*
        // max 18 numbers total, max 2 decimal, dot delimiter
        public double? TB0 { get; set; }

        // T0:630*
        // max 18 numbers total, max 2 decimal, dot delimiter
        public double? T0 { get; set; }

        // TB1:2000*
        // max 18 numbers total, max 2 decimal, dot delimiter
        public double? TB1 { get; set; }

        // T1:300*
        // max 18 numbers total, max 2 decimal, dot delimiter
        public double? T1 { get; set; }

        // TB2:1000*
        // max 18 numbers total, max 2 decimal, dot delimiter
        public double? TB2 { get; set; }

        // T2:100*
        // max 18 numbers total, max 2 decimal, dot delimiter
        public double? T2 { get; set; }

        // NTB:500*
        // max 18 numbers total, max 2 decimal, dot delimiter
        public double? NTB { get; set; }

        // CC:CZK*
        // 3 chars, ISO 4217, all-caps
        public string Currency { get; set; }

        // FX:123456.789*
        // max 18 numbers total, max 3 decimal, dot delimiter
        public double? ExchangeRate { get; set; }

        // FXA:100*
        // max 5 numbers
        // default = 1
        public int ForeignCurrencyAmount { get; set; }

        // ACC:CZ5855000000001265098001+RZBCCZPP*
        // max 46 chars
        public BankAccount Account { get; set; }

        // X-SW:E-FAKTURANT V5.3*
        // max 30 chars
        public string Software { get; set; }

        // X-URL:HTTP://E-FAKTURANT.CZ/INV/ABCD123456789EFG.ISDOC*
        // max 70 chars
        public string Url { get; set; }
    }
}
