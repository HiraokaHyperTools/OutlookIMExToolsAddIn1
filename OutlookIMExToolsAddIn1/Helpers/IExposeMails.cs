using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Helpers
{
    public interface IExposeMails
    {
        IEnumerable<Stream> GetMails();
    }
}
