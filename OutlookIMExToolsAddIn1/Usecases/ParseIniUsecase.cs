using System;
using System.Collections.Generic;

namespace OutlookIMExToolsAddIn1.Usecases
{
    public class ParseIniUsecase
    {
        public IDictionary<string, IDictionary<string, string>> ParseIni(string body)
        {
            var root = new Dictionary<string, IDictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
            IDictionary<string, string> section = null;

            foreach (var line in body.Replace("\r\n", "\n").Split('\n'))
            {
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    // Section header
                    var sectionName = line.Substring(1, line.Length - 2);
                    if (!root.TryGetValue(sectionName, out section))
                    {
                        section = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                    }
                    root[sectionName] = section;
                }
                else if (true
                    && line.Split(new char[] { '=' }, 2) is string[] pair
                    && pair.Length == 2
                    && pair[0].Trim() is string key
                    && key.Length != 0
                    && section != null
                )
                {
                    section[key] = pair[1].Trim();
                }
            }

            return root;
        }
    }
}