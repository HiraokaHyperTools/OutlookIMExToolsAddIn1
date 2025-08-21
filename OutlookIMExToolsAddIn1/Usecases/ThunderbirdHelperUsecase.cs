using Jurassic;
using kenjiuno.AutoHourglass;
using Microsoft.Office.Interop.Outlook;
using Newtonsoft.Json;
using OutlookIMExToolsAddIn1.Forms;
using OutlookIMExToolsAddIn1.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OutlookIMExToolsAddIn1.Usecases
{
    public class ThunderbirdHelperUsecase
    {
        private readonly Microsoft.Office.Interop.Outlook.Application _app;
        private static readonly Regex _from = new Regex("^>+From\\s");

        public ThunderbirdHelperUsecase(
            Microsoft.Office.Interop.Outlook.Application app
        )
        {
            _app = app;
        }

        public IReadOnlyList<ImportFolderNode> SummaryThunderbirdAccountFolder(
            string dir,
            IDictionary<string, string> folderPathToName
        )
        {
            var list = new List<ImportFolderNode>();

            bool Filter(string path)
            {
                var name = Path.GetFileName(path);
                return true
                    && !path.EndsWith(".msf")
                    && name != "msgFilterRules.dat"
                    && name != "popstate.dat"
                    && name != "filterlog.html"
                    ;
            }

            foreach (var file in Directory.GetFiles(dir)
                .Where(Filter)
            )
            {
                var subFolder = file + ".sbd";
                IReadOnlyList<ImportFolderNode> subFolders = new ImportFolderNode[0];
                if (Directory.Exists(subFolder))
                {
                    subFolders = SummaryThunderbirdAccountFolder(subFolder, folderPathToName);
                }

                string displayName = null;
                if (!folderPathToName.TryGetValue(file, out displayName))
                {
                    if (!folderPathToName.TryGetValue(file + ".msf", out displayName))
                    {

                    }
                }

                list.Add(new ImportFolderNode
                {
                    DisplayName = displayName ?? Path.GetFileName(file),
                    WillImport = true,
                    Mails = new ExposeThunderbirdMbox(file),
                    SubFolders = subFolders,
                });
            }

            return list.AsReadOnly();
        }

        private class ExposeThunderbirdMbox : IExposeMails
        {
            private string _file;

            public ExposeThunderbirdMbox(string file)
            {
                _file = file;
            }

            public IEnumerable<Stream> GetMails()
            {
                var stream = new MemoryStream();
                var latin1 = Encoding.GetEncoding("latin1");
                var y = 0;

                using (var reader = new StreamReader(_file, latin1))
                {
                    while (true)
                    {
                        var line = reader.ReadLine();
                        if (line == null)
                        {
                            yield break; // End of file
                        }

                        if (line.StartsWith("From "))
                        {
                            if (y != 0)
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                                yield return stream;
                                stream = new MemoryStream();
                            }
                            y++;
                        }
                        else if (_from.IsMatch(line))
                        {
                            var b = latin1.GetBytes(line.Substring(1) + "\r\n");
                            stream.Write(b, 0, b.Length);
                        }
                        else
                        {
                            var b = latin1.GetBytes(line + "\r\n");
                            stream.Write(b, 0, b.Length);
                        }
                    }
                }
            }
        }

        public IDictionary<string, object> ParsePrefsJs(string jscript)
        {
            var dict = new Dictionary<string, object>();
            var se = new ScriptEngine();
            void userPref(string key, object value)
            {
                dict[key] = value;
            }
            se.SetGlobalFunction("user_pref", (Action<string, object>)userPref);
            var ev = CompiledEval.Compile(new StringScriptSource(jscript));
            ev.Evaluate(se);
            return dict;
        }

        public IReadOnlyList<ThunderbirdAccountSummary> SummaryAccountsFromPrefsJs(
            IDictionary<string, object> prefs,
            string baseDir
        )
        {
            var list = new List<ThunderbirdAccountSummary>();

            void TryToAddServer(string serverKey)
            {
                if (true
                    && prefs.TryGetValue($"mail.server.{serverKey}.name", out var nameRaw)
                    && prefs.TryGetValue($"mail.server.{serverKey}.directory", out var directoryRaw)
                    && directoryRaw is string directory
                    && !string.IsNullOrWhiteSpace(directory)
                    && Path.Combine(baseDir, directory) is string fullDirectory
                    && prefs.TryGetValue($"mail.server.{serverKey}.directory-rel", out var directoryRelRaw)
                    && (directoryRelRaw + "").Replace("[ProfD]", baseDir + "\\") is string directoryRel
                    && !string.IsNullOrWhiteSpace(directoryRel)
                    && new string[] { fullDirectory, directoryRel }.Where(Directory.Exists).ToArray() is string[] directories
                    && directories.Any()
                )
                {
                    list.Add(new ThunderbirdAccountSummary
                    {
                        Name = nameRaw + "",
                        AccountDir = directories.First(),
                    });
                }
            }

            if (true
                && prefs.TryGetValue("mail.accountmanager.accounts", out var accounts)
                )
            {
                using (new AH())
                {
                    foreach (var accountKey in (accounts + "").Split(','))
                    {
                        if (true
                            && prefs.TryGetValue($"mail.account.{accountKey}.server", out var serverKey)
                        )
                        {
                            TryToAddServer(serverKey + "");
                        }
                    }
                }
            }

            return list.AsReadOnly();
        }

        public IReadOnlyList<ImportFolderNode> CreateImporterFrom(TreeNodeCollection nodes)
        {
            return nodes
                .Cast<TreeNode>()
                .Select(
                    node =>
                    {
                        var tbNode = node.Tag as ImportFolderNode;
                        return new ImportFolderNode()
                        {
                            WillImport = node.Checked,
                            DisplayName = node.Text,
                            Mails = tbNode?.Mails ?? ExposeNoMails.Instance,
                            SubFolders = CreateImporterFrom(node.Nodes),
                        };
                    }
                )
                .ToList()
                .AsReadOnly();
        }

        private class FolderCacheEntry
        {
            public string folderName { get; set; }
        }

        public IDictionary<string, string> ParseFolderCacheJson(string body)
        {
            var dict = new Dictionary<string, string>();
            var json = JsonConvert.DeserializeObject<Dictionary<string, FolderCacheEntry>>(body);
            if (json != null)
            {
                foreach (var pair in json)
                {
                    dict[pair.Key] = pair.Value.folderName;
                }
            }
            return dict;
        }

        public IDictionary<string, IDictionary<string, string>> Grouping(IDictionary<string, object> prefs, string prefix)
        {
            var groups = new Dictionary<string, IDictionary<string, string>>();
            var domains = prefs
                .Where(pair => pair.Key.StartsWith(prefix))
                .Select(pair => (Key: pair.Key.Substring(prefix.Length), Value: pair.Value))
                .Select(
                    pair =>
                    {
                        int sep = pair.Key.IndexOf('.');
                        if (sep < 0)
                        {
                            return ("", "", "");
                        }
                        else
                        {
                            return (Domain: pair.Key.Substring(0, sep), DomainKey: pair.Key.Substring(sep + 1), Value: pair.Value);
                        }
                    }
                )
                .Where(pair => pair.Domain.Length != 0)
                .GroupBy(pair => pair.Domain);
            foreach (var domain in domains)
            {
                var domainKey = domain.Key;
                var group = new Dictionary<string, string>();
                foreach (var pair in domain)
                {
                    group[pair.DomainKey] = pair.Value + "";
                }
                groups[domainKey] = group;
            }
            return groups;
        }

        public IReadOnlyList<IThunderbirdAddrBook> CreateContactImporterFrom(TreeNodeCollection nodes)
        {
            return nodes
                .Cast<TreeNode>()
                .Where(node => node.Checked)
                .Select(node => node.Tag)
                .OfType<IThunderbirdAddrBook>()
                .ToList()
                .AsReadOnly();
        }
    }
}