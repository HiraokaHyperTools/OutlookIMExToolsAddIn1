using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Helpers
{
    public interface IThunderbirdAddrBook
    {
        string DisplayName { get; }
        string SqliteFile { get; }
    }
}
