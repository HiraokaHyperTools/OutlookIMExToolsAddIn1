using kenjiuno.AutoHourglass;
using Microsoft.Office.Interop.Outlook;
using OutlookIMExToolsAddIn1.Helpers;
using OutlookIMExToolsAddIn1.Usecases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OutlookIMExToolsAddIn1.Forms
{
    public partial class ImTbContactsForm : Form
    {
        private readonly LaunchImportUsecase _launchImportUsecase;
        private readonly OutlookHelperUsecase _outlookHelperUsecase;
        private readonly ThunderbirdHelperUsecase _thunderbirdHelperUsecase;
        private readonly ThunderbirdProfilesUsecase _thunderbirdProfilesUsecase;
        private readonly Func<ImportVCardToDelegate> _getImportVCardTo;
        private MAPIFolder _folder;

        public ImTbContactsForm(
            ThunderbirdProfilesUsecase thunderbirdProfilesUsecase,
            ThunderbirdHelperUsecase thunderbirdHelperUsecase,
            OutlookHelperUsecase outlookHelperUsecase,
            LaunchImportUsecase launchImportUsecase)
        {
            _launchImportUsecase = launchImportUsecase;
            _outlookHelperUsecase = outlookHelperUsecase;
            _thunderbirdHelperUsecase = thunderbirdHelperUsecase;
            _thunderbirdProfilesUsecase = thunderbirdProfilesUsecase;
            InitializeComponent();

            _overwrite.Items.Add("Always duplicate");
            _overwrite.SelectedIndex = 0;

            var importers = new List<Tuple<string, ImportVCardToDelegate>>
            {
                Tuple.Create(
                    "Outlook OpenSharedItem (will loose non-ANSI characters)",
                    (ImportVCardToDelegate)_outlookHelperUsecase.ImportVCardTo
                ),
                Tuple.Create(
                    "IMEx Tools contact converter",
                    (ImportVCardToDelegate)_outlookHelperUsecase.ImportVCardWithAltTo
                ),
            };
            _importer.DisplayMember = "Item1";
            _importer.ValueMember = "Item2";
            _importer.DataSource = importers.ToArray();
            _importer.SelectedIndex = 0;

            _getImportVCardTo = () => _importer.SelectedValue as ImportVCardToDelegate;
        }

        private void _selectPopup_Click(object sender, EventArgs e)
        {
            _popup.Items.Clear();

            _popup.Items.Add(
                "Add abook.sqlite file...",
                null,
                (a, b) =>
                {
                    if (_ofdAbook.ShowDialog(this) == DialogResult.OK)
                    {
                        AddAB(
                            Path.GetFileNameWithoutExtension(_ofdAbook.FileName),
                            _ofdAbook.FileName
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
                                ApplyThisProfileFolder(one.Path);
                            }
                        }
                    );
                }
            }

            _popup.Show((Control)sender, Point.Empty);
        }

        private void ApplyThisProfileFolder(string baseDir)
        {
            _tree.Nodes.Clear();

            var prefsJsFile = Path.Combine(baseDir, "prefs.js");
            if (File.Exists(prefsJsFile))
            {
                var prefs = _thunderbirdHelperUsecase.ParsePrefsJs(
                    File.ReadAllText(prefsJsFile, Encoding.UTF8)
                );

                // "Personal Address Book" "abook.sqlite"
                // ...
                // "Collected Addresses" "history.sqlite"

                AddAB("Personal Address Book", Path.Combine(baseDir, "abook.sqlite"));

                var groups = _thunderbirdHelperUsecase.Grouping(prefs, "ldap_2.servers.");
                foreach (var dict in groups.Values)
                {
                    if (true
                        && dict.TryGetValue("description", out var descriptionRaw)
                        && descriptionRaw is string description
                        && description != null
                        && dict.TryGetValue("filename", out var filenameRaw)
                        && filenameRaw is string filename
                        && !string.IsNullOrEmpty(filename)
                        && Path.Combine(baseDir, filename) is string sqliteFile
                        && File.Exists(sqliteFile)
                    )
                    {
                        AddAB(description, sqliteFile);
                    }
                }

                AddAB("Collected Addresses", Path.Combine(baseDir, "history.sqlite"));
            }
            else
            {
                MessageBox.Show(
                    $"The profile directory does not contain prefs.js: {baseDir}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void AddAB(string desc, string sqliteFile)
        {
            var node = _tree.Nodes.Add(desc);
            node.Tag = new AB { SqliteFile = sqliteFile, DisplayName = desc, };
            node.Checked = true;
        }

        private class AB : IThunderbirdAddrBook
        {
            public string DisplayName { get; set; }
            public string SqliteFile { get; set; }
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

        private void FolderUpdated()
        {
            _toOutlookFolder.Text = (_folder != null)
                ? _outlookHelperUsecase.FormatFolderNameTree(_folder)
                : "No folder selected"
                ;
        }

        private void ImTbContactsForm_Load(object sender, EventArgs e)
        {
            _folder = _outlookHelperUsecase.GetDefaultContactsFolder();
            FolderUpdated();
        }

        private void _onAll_Click(object sender, EventArgs e)
        {
            CheckAll(true);

        }

        private void _offAll_Click(object sender, EventArgs e)
        {
            CheckAll(false);

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

        private void _import_Click(object sender, EventArgs e)
        {
            var importVCardTo = _getImportVCardTo();
            if (importVCardTo == null)
            {
                _importer.Focus();
                _importer.DroppedDown = true;
                return;
            }

            _launchImportUsecase.LaunchContactsImport(
                _thunderbirdHelperUsecase.CreateContactImporterFrom(_tree.Nodes),
                _folder,
                importVCardTo
            );
        }
    }
}
