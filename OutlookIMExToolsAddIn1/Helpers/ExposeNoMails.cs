using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Helpers
{
    public class ExposeNoMails : IExposeMails
    {
        public static IExposeMails Instance = new ExposeNoMails();

        public IEnumerable<Stream> GetMails()
        {
            // This is an empty implementation, returning no mails.
            // In a real implementation, this would return a collection of Stream objects representing emails.
            return Enumerable.Empty<Stream>();
        }
    }
}
