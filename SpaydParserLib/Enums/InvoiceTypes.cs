using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaydParserLib.Enums
{
    public enum InvoiceType
    {
        /// <summary>
        /// Nedaňový doklad
        /// </summary>
        NonTax = 0,

        /// <summary>
        /// Opravný daňový doklad
        /// </summary>
        Correcting = 1,

        /// <summary>
        /// Doklad k přijaté platbě
        /// </summary>
        Receipt = 2,

        /// <summary>
        /// Splátkový kalendář
        /// </summary>
        RepaymentSchedule = 3,

        /// <summary>
        /// Platební kalendář
        /// </summary>
        PaymentSchedule = 4,

        /// <summary>
        /// Souhrnný daňový doklad
        /// </summary>
        Aggregate = 5,

        /// <summary>
        /// Other (default)
        /// </summary>
        Other = 9
    }
}
