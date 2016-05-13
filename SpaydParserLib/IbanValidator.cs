using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SpaydParserLib
{
    public class IbanValidator
    {
        private static Dictionary<string, int> _countries = new Dictionary<string, int>()
        {
            {"AL", 28}, // Albania
            {"AD", 24},	// Andorra
		    {"AT", 20},	// Austria
		    {"AZ", 28}, // Azerbaijan

            {"BH", 22},	// Bahrain
            {"BE", 16},	// Belgium
		    {"BA", 20},	// Bosnia and Herzegovina
            {"BR", 29}, // Brazil
		    {"BG", 22}, // Bulgaria

            {"CR", 21},	// Costa Rica
            {"HR", 21}, // Croatia
            {"CY", 28}, // Cyprus
		    {"CZ", 24}, // Czech Republic

            {"DK", 18}, // Denmark
		    {"DO", 28}, // Dominican Republic

            {"TL", 23}, // East Timor
            {"EE", 20}, // Estonia
		    {"FO", 18}, // Faroe Islands
		    {"FI", 18}, // Finland
		    {"FR", 27}, // France

            {"GE", 22}, // Georgia
            {"DE", 22},	// Germany
		    {"GI", 23},	// Gibraltar
		    {"GR", 27},	// Greece
		    {"GL", 18},	// Greenland
		    {"GT", 28},	// Guatemala
		    {"HU", 28},	// Hungary
		    {"IS", 26},	// Iceland
		    {"IE", 22},	// Ireland
		    {"IL", 23},	// Israel
		    {"IT", 27},	// Italy

            {"JO", 30}, // Jordan

		    {"KZ", 20},	// Kazakhstan
            {"XK", 20}, // Kosovo
		    {"KW", 30},	// Kuwait

		    {"LV", 21},	// Latvia
		    {"LB", 28},	// Lebanon
		    {"LI", 21},	// Liechtenstein
		    {"LT", 20},	// Lituania
		    {"LU", 20},	// Luxembourg
        
		    {"MK", 19},	// Macedonia
		    {"MT", 31},	// Malta
		    {"MR", 27},	// Mauritania
		    {"MU", 30},	// Mauritius
		    {"MC", 27},	// Monaco
		    {"MD", 24},	// Moldova
		    {"ME", 22},	// Montenegro
		    {"NL", 18},	// Netherlands
		    {"NO", 15},	// Norway
		
            {"PK", 24},	// Pakistan
		    {"PS", 29},	// Palestinian territories
		    {"PL", 28},	// Poland
		    {"PT", 25},	// Portugal

            {"QA", 29}, // Qatar

		    {"RO", 24},	// Romania

		    {"SM", 27},	// San Marino
		    {"SA", 24},	// Saudi Arabia
		    {"RS", 22},	// Serbia
		    {"SK", 24},	// Slovakia
            {"SI", 19},	// Slovenia
		    {"ES", 24},	// Spain
		    {"SE", 24},	// Sweden
		    {"CH", 21},	// Switzerland

		    {"TN", 24},	// Tunisia
		    {"TR", 26},	// Turkey

		    {"AE", 23},	// United Arab Emirates
		    {"GB", 22},	// United Kingdom

		    {"VG", 24},	// Virgin Islands, British

            // Nordea IBANs
            {"DZ", 24}, // Algeria
            {"AO", 25}, // Angola
            {"BJ", 28}, // Benin
            {"BF", 27}, // Burkina Faso
            {"BI", 16}, // Burundi
            {"CM", 27}, // Cameroon
            {"CV", 25}, // Cape Verde
            {"IR", 26}, // Iran
            {"CI", 28}, // Ivory Coast
            {"MG", 27}, // Madagascar
            {"ML", 28}, // Mali
            {"MZ", 25}, // Mozambique
            {"SN", 28}, // Senegal
            {"UA", 29} // Ukraine
        };

        public static bool Validate(string iban)
        {
            if (iban == null)
            {
                return false;
            }
            iban = iban.Replace(" ", "").ToUpper();

            Regex ibanFormat = new Regex("([A-Z]{2})([0-9]{2})[A-Z0-9]{0,30}");
            var matches = ibanFormat.Matches(iban);
            if (matches.Count != 1) // exatcly 1 IBAN only
            {
                return false;
            }

            string countryCode = matches[0].Groups[1].Value;
            string checksum = matches[0].Groups[2].Value;

            if (!_countries.ContainsKey(countryCode))
            {
                // unknown country
                return false;
            }

            if (_countries[countryCode] != iban.Length)
            {
                // wrong length for this country
                return false;
            }

            string switchedString = $"{iban.Substring(4)}{iban.Substring(0, 4)}";

            return validateMod97(switchedString);
        }

        private static bool validateMod97(string str)
        {
            int length = str.Length;
            int checkSum = 0;
            for (int index = 0; index < length; index++)
            {
                int value = integerValue(str[index]);
                if (value < 10)
                {
                    checkSum = (10 * checkSum) + value;
                }
                else
                {
                    checkSum = (100 * checkSum) + value;
                }
                if (checkSum >= int.MaxValue / 100)
                {
                    checkSum %= 97;
                }
            }
            int result = checkSum % 97;

            return result == 1;
        }

        private static int integerValue(char ch)
        {
            if (ch >= '0' && ch <= '9')
            {
                return ch - '0';
            }
            return ch - 'A' + 10;
        }

    }
}
