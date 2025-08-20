using kenjiuno.AutoHourglass;
using Microsoft.Office.Interop.Outlook;
using OutlookIMExToolsAddIn1.Forms;
using OutlookIMExToolsAddIn1.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OutlookIMExToolsAddIn1.Usecases
{
    public class LaunchImportUsecase
    {
        private readonly Func<ImForm> _newImForm;

        public LaunchImportUsecase(
            Func<ImForm> newImForm
            )
        {
            _newImForm = newImForm;
        }

        public void LaunchImport(
            IReadOnlyList<ImportFolderNode> importFolderNodes,
            MAPIFolder folder
        )
        {
            var form = _newImForm();
            form.Show();

            var cts = new CancellationTokenSource();

            form._cancel.Click += (a, b) => form.Close();
            form.FormClosed += (a, b) => cts.Cancel();

            void UpdateProgress(string hint, int rate)
            {
                if (form.IsDisposed)
                {
                    return;
                }
                else
                {
                    form._hint.Text = hint;
                    form._progress.Value = rate;
                }
            }

            var log = new StringWriter();

            form._log.Click += (a, b) =>
            {
                try
                {
                    using (new AH())
                    {
                        var logFile = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                            "OutlookIMExTools.log"
                        );
                        File.WriteAllText(logFile, log.ToString());
                        Process.Start(new ProcessStartInfo(logFile) { UseShellExecute = true, });
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Failed to open log file: {ex.Message}");
                }
            };

            var importerTask = form.LaunchImportAsync(
                importFolderNodes,
                folder,
                cts.Token,
                UpdateProgress,
                log
            );

            async Task TrackAsync()
            {
                try
                {
                    await importerTask;
                    UpdateProgress("Import succeeded!", 10000);
                    form._cancel.Text = "Close";
                    form.AcceptButton = form._cancel;
                }
                catch (System.Exception ex)
                {
                    UpdateProgress($"Import Failed!\n\n{ex}", 10000);
                }
            }

            var trackerTask = TrackAsync();
        }
    }
}