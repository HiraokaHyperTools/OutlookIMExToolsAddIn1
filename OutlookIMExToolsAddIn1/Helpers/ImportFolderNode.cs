using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Helpers
{
    public class ImportFolderNode
    {
        public bool WillImport { get; set; }

        public string DisplayName { get; set; }

        public IExposeMails Mails { get; set; }

        public IReadOnlyList<ImportFolderNode> SubFolders { get; set; }
    }
}
