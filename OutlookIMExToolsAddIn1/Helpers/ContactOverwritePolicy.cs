using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Helpers
{
    public class ContactOverwritePolicy
    {
        public bool WillReplaceExisting { get; set; }
        public bool WillDuplicate { get; set; }
    }
}
