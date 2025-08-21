using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Helpers
{
    public delegate ContactItem ImportVCardToDelegate(
        string vcfBody,
        MAPIFolder folder,
        ContactOverwritePolicy policy
    );
}
