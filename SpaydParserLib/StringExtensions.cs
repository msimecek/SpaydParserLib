using System.Collections.Generic;
using System.Net;

namespace SpaydParserLib
{
    public static class StringExtensions
    {
        /// <summary>
        /// Extract string to name-values pairs delimited by specific delimiters
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="pairDelimiter">Delimiter for whole pairs</param>
        /// <param name="valueDelimiter">Delimiter for concrete pair (key/value)</param>
        /// <param name="trimValues">Trim key and value when true</param>
        /// <returns>Parsed pairs</returns>
        public static Dictionary<string, string> ExtractValuePairs(this string source, char pairDelimiter, char valueDelimiter, bool trimValues = true)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            string[] pairs = source.Split(pairDelimiter);

            foreach (var pair in pairs)
            {
                string[] keyValuePair = pair.Split(valueDelimiter);

                if (keyValuePair.Length != 2)
                {
                    continue;
                }

                string key = trimValues ? keyValuePair[0].Trim().ToUpperInvariant() : keyValuePair[0].ToUpperInvariant();
                string value = trimValues ? keyValuePair[1].Trim() : keyValuePair[1];

                value = WebUtility.HtmlDecode(value);

                result.Add(key, value);
            }

            return result;
        }
    }
}
