using System;
using System.Collections.Generic;
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
                _errors.Add("Some required parameter in SIND is missing: " + string.Join(", ", missingKeys));

                return false;
            }

            return true;
        }
    }
}
