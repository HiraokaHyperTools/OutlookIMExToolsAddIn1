using OutlookIMExToolsAddIn1.Usecases;
using Microsoft.Office.Interop.Outlook;
using OutlookIMExToolsAddIn1.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Exception = System.Exception;
using kenjiuno.AutoHourglass;
using System.Threading;

namespace OutlookIMExToolsAddIn1.Forms
{
    public partial class ImTbForm : Form
    {
        private readonly LaunchImportUsecase _launchImportUsecase;
        private readonly ThunderbirdHelperUsecase _thunderbirdHelperUsecase;
        private readonly ThunderbirdProfilesUsecase _thunderbirdProfilesUsecase;
        private readonly ParseIniUsecase _parseIniUsecase;
        private readonly OutlookHelperUsecase _outlookHelperUsecase;
        private MAPIFolder _folder;

        public ImTbForm(
            OutlookHelperUsecase outlookHelperUsecase,
            ParseIniUsecase parseIniUsecase,
            ThunderbirdProfilesUsecase thunderbirdProfilesUsecase,
            ThunderbirdHelperUsecase thunderbirdHelperUsecase,
            LaunchImportUsecase launchImportUsecase
        )
        {
            _launchImportUsecase = launchImportUsecase;
            _thunderbirdHelperUsecase = thunderbirdHelperUsecase;
            _thunderbirdProfilesUsecase = thunderbirdProfilesUsecase;
            _parseIniUsecase = parseIniUsecase;
            _outlookHelperUsecase = outlookHelperUsecase;
            InitializeComponent();
        }

        private void _import_Click(object sender, EventArgs e)
        {
            _launchImportUsecase.LaunchImport(
                _thunderbirdHelperUsecase.CreateImporterFrom(_tree.Nodes),
                _folder
            );
        }

        private void ImTbForm_Load(object sender, EventArgs e)
        {
            _folder = _outlookHelperUsecase.GetCurrentFolder();
            FolderUpdated();
        }

        private void FolderUpdated()
        {
            _toOutlookFolder.Text = (_folder != null)
                ? _outlookHelperUsecase.FormatFolderNameTree(_folder)
                : "No folder selected"
                ;
        }

        private void _selectOutlookFolder_Click(object sender, EventArgs e)
        {
            var folder = _outlookHelperUsecase.SelectFolder();
            if (folder != null)
            {
                Marshal.ReleaseComObject(_folder);
                _folder = folder;
                FolderUpdated();
            }
        }

        private void ImTbForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_folder != null)
            {
                Marshal.ReleaseComObject(_folder);
            }
        }

        private void _selectPopup_Click(object sender, EventArgs e)
        {
            _popup.Items.Clear();

            _popup.Items.Add(
                "Select from folder...",
                null,
                (a, b) =>
                {
                    if (_ofd.ShowDialog(this) == DialogResult.OK)
                    {
                        SelectThunderbirdAccountFolder(
                            Path.GetDirectoryName(_ofd.FileName)
                        );
                    }
                }
            );

            _popup.Items.Add(new ToolStripSeparator());

            using (new AH())
            {
                foreach (var one in _thunderbirdProfilesUsecase.ListAll())
                {
                    _popup.Items.Add(
                        $"{one.Name} ({one.Path})",
                        null,
                        (a, b) =>
                        {
                            using (var ah = new AH())
                            {
                                _tree.Nodes.Clear();

                                var prefsJsFile = Path.Combine(one.Path, "prefs.js");
                                if (File.Exists(prefsJsFile))
                                {
                                    var prefs = _thunderbirdHelperUsecase.ParsePrefsJs(
                                        File.ReadAllText(prefsJsFile, Encoding.UTF8)
                                    );

                                    IDictionary<string, string> folderPathToName = new Dictionary<string, string>();
                                    var folderCacheJsonFile = Path.Combine(
                                        one.Path,
                                        "folderCache.json"
                                    );
                                    if (File.Exists(folderCacheJsonFile))
                                    {
                                        var folderCacheJson = File.ReadAllText(
                                            folderCacheJsonFile,
                                            Encoding.UTF8
                                        );
                                        folderPathToName = _thunderbirdHelperUsecase.ParseFolderCacheJson(
                                            folderCacheJson
                                        );
                                    }

                                    var accountList = _thunderbirdHelperUsecase.SummaryAccountsFromPrefsJs(
                                        prefs,
                                        one.Path
                                    );

                                    foreach (var account in accountList)
                                    {
                                        var accountNode = _tree.Nodes.Add(account.Name);
                                        accountNode.Checked = true;

                                        Walk(
                                            accountNode.Nodes,
                                            _thunderbirdHelperUsecase.SummaryThunderbirdAccountFolder(
                                                account.AccountDir,
                                                folderPathToName
                                            )
                                        );
                                    }
                                }
                                else
                                {
                                    ah.Dispose();
                                    MessageBox.Show(
                                        $"The profile directory does not contain prefs.js: {one.Path}",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error
                                    );
                                }
                            }
                        }
                    );
                }
            }

            _popup.Show((Control)sender, Point.Empty);
        }

        private void SelectThunderbirdAccountFolder(string dir)
        {
            using (new AH())
            {
                _tree.Nodes.Clear();

                Walk(
                    _tree.Nodes,
                    _thunderbirdHelperUsecase.SummaryThunderbirdAccountFolder(
                        dir, 
                        new Dictionary<string, string>()
                    )
                );
            }
        }

        private void Walk(TreeNodeCollection nodes, IReadOnlyList<ImportFolderNode> tbNodes)
        {
            foreach (var tbNode in tbNodes)
            {
                var node = nodes.Add(tbNode.DisplayName);
                node.Checked = tbNode.WillImport;
                node.Tag = tbNode;

                if (tbNode.SubFolders?.Any() ?? false)
                {
                    Walk(node.Nodes, tbNode.SubFolders);
                }

                node.Expand();
            }
        }

        private void _checkOnRecur_Click(object sender, EventArgs e)
        {
            CheckRecur(_tree.SelectedNode, true);
        }

        private void _checkOffRecur_Click(object sender, EventArgs e)
        {
            CheckRecur(_tree.SelectedNode, false);
        }

        private void CheckRecur(TreeNode node, bool isChecked)
        {
            if (node == null)
            {
                return;
            }

            if (node.Checked != isChecked)
            {
                node.Checked = isChecked;
            }

            foreach (TreeNode child in node.Nodes)
            {
                CheckRecur(child, isChecked);
            }
        }

        private void CheckAll(bool isChecked)
        {
            foreach (TreeNode node in _tree.Nodes)
            {
                CheckRecur(node, isChecked);
            }
        }

        private void _onAll_Click(object sender, EventArgs e)
        {
            CheckAll(true);
        }

        private void _offAll_Click(object sender, EventArgs e)
        {
            CheckAll(false);
        }
    }
}
