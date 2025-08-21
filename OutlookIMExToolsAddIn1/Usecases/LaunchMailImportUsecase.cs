using OutlookIMExToolsAddIn1.Usecases;
using Microsoft.Office.Interop.Outlook;
using OutlookIMExToolsAddIn1.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Usecases
{
    public class LaunchMailImportUsecase
    {
        private readonly OutlookHelperUsecase _outlookHelperUsecase;
        private readonly ThunderbirdHelperUsecase _thunderbirdHelperUsecase;

        public LaunchMailImportUsecase(ThunderbirdHelperUsecase thunderbirdHelperUsecase, OutlookHelperUsecase outlookHelperUsecase)
        {
            _outlookHelperUsecase = outlookHelperUsecase;
            _thunderbirdHelperUsecase = thunderbirdHelperUsecase;
        }

        public async Task LaunchImportAsync(
            IReadOnlyList<ImportFolderNode> tbNodes,
            MAPIFolder folder,
            CancellationToken cancellationToken,
            Action<string, int> updateProgress,
            TextWriter logger
        )
        {
            int folderIndex = 0;
            int totalFolderCount = 0;
            int numConverted = 0;

            var converterSession = _outlookHelperUsecase.CreateConverterSession();
            try
            {
                var emptyMail = _outlookHelperUsecase.LoadPseudoReceived();
                try
                {
                    async Task ConvertFolderAsync(
                        IReadOnlyList<ImportFolderNode> tbNodes2,
                        MAPIFolder folder2,
                        string prefix
                    )
                    {
                        foreach (var tbNode in tbNodes2)
                        {
                            await Task.Delay(10);

                            if (cancellationToken.IsCancellationRequested)
                            {
                                return;
                            }

                            var at = $"{prefix} > {tbNode.DisplayName}";

                            updateProgress(
                                at,
                                (int)((folderIndex++) * 10000f / totalFolderCount)
                            );

                            logger.WriteLine($"Converting: {at}");

                            var subFolder = _outlookHelperUsecase.AddOrGetFolder(folder2.Folders, tbNode.DisplayName);

                            if (tbNode.WillImport)
                            {
                                var mails = tbNode.Mails;
                                var n = 0;
                                foreach (var stream in mails.GetMails())
                                {
                                    if (stream == null)
                                    {
                                        continue;
                                    }

                                    using (stream)
                                    {
                                        ++n;
                                        if ((n & 15) == 0)
                                        {
                                            await Task.Delay(1);
                                        }

                                        if (cancellationToken.IsCancellationRequested)
                                        {
                                            return;
                                        }

                                        try
                                        {
                                            _outlookHelperUsecase
                                                .Convert(
                                                    subFolder,
                                                    stream,
                                                    converterSession,
                                                    emptyMail
                                                );

                                            numConverted++;
                                        }
                                        catch (System.Exception ex)
                                        {
                                            logger.WriteLine($"Error converting mail number {n}: {ex}");
                                        }
                                    }
                                }
                            }

                            if (tbNode.SubFolders != null)
                            {
                                await ConvertFolderAsync(
                                    tbNode.SubFolders,
                                    subFolder,
                                    at
                                );
                            }
                        }
                    }

                    void UpdateTotalFolderCount(IEnumerable<ImportFolderNode> tbNodes2)
                    {
                        foreach (var tbNode in tbNodes2)
                        {
                            totalFolderCount++;

                            if (tbNode.SubFolders != null)
                            {
                                UpdateTotalFolderCount(tbNode.SubFolders);
                            }
                        }
                    }

                    logger.WriteLine("Couting total folder count.");

                    UpdateTotalFolderCount(tbNodes);
                    totalFolderCount = Math.Max(totalFolderCount, 1);

                    logger.WriteLine($"totalFolderCount = {totalFolderCount}");

                    logger.WriteLine("Going to import mails from Thunderbird.");

                    try
                    {
                        await ConvertFolderAsync(tbNodes, folder, "Root");
                    }
                    catch (System.Exception ex)
                    {
                        logger.WriteLine($"Error during conversion: {ex}");
                        throw;
                    }

                    logger.WriteLine("Done.");
                }
                finally
                {
                    logger.WriteLine($"numConverted = {numConverted}");

                    try
                    {
                        emptyMail?.Delete();
                    }
                    catch
                    {
                        // Ignore any exceptions during deletion
                    }

                    Marshal.ReleaseComObject(emptyMail);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(converterSession);
            }
        }
    }
}