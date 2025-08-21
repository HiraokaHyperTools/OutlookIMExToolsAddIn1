using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Helpers
{
    public class VCardLine
    {
        public string Key { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
        public string Value { get; set; }

        public string OriginalLine { get; set; }
    }
}
