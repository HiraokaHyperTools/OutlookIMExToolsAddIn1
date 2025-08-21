using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Helpers
{
    public delegate Task LaunchGenericImporterDelegate(
        CancellationToken cancellationToken,
        Action<string, int> updateProgress,
        TextWriter log
    );
}
