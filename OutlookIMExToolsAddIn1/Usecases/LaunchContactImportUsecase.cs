using Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;
using OutlookIMExToolsAddIn1.Helpers;
using OutlookIMExToolsAddIn1.Usecases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Usecases
{
    public class LaunchContactImportUsecase
    {
        private readonly OutlookHelperUsecase _outlookHelperUsecase;
        private readonly ThunderbirdAddrBookUsecase _thunderbirdAddrBookUsecase;

        public LaunchContactImportUsecase(ThunderbirdAddrBookUsecase thunderbirdAddrBookUsecase, OutlookHelperUsecase outlookHelperUsecase)
        {
            _outlookHelperUsecase = outlookHelperUsecase;
            _thunderbirdAddrBookUsecase = thunderbirdAddrBookUsecase;
        }

        public async Task LaunchImportAsync(
            IReadOnlyList<IThunderbirdAddrBook> nodes,
            MAPIFolder folder,
            CancellationToken cancellationToken,
            Action<string, int> updateProgress,
            TextWriter logger
        )
        {
            int folderIndex = 0;
            int totalFolderCount = nodes.Count;
            int numConverted = 0;

            logger.WriteLine("Going to import contacts from Thunderbird.");
            try
            {
                try
                {
                    foreach (var node in nodes)
                    {
                        await Task.Delay(10);

                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        var at = node.DisplayName;
                        updateProgress(
                            at,
                            (int)((folderIndex++) * 10000f / totalFolderCount)
                        );

                        logger.WriteLine($"Converting: {at}");

                        var loaded = await _thunderbirdAddrBookUsecase.LoadAsync(node.SqliteFile);

                        var contactUidTo = new Dictionary<string, ContactItem>();

                        foreach (var prop in loaded.Properties.Where(prop => prop.Name == "_vCard"))
                        {
                            logger.WriteLine($"Converting card: {prop.Card}");
                            var vcf = prop.Value;
                            var vcfFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.vcf");
                            File.WriteAllText(vcfFile, vcf, Encoding.Default);
                            try
                            {
                                contactUidTo[prop.Card] = _outlookHelperUsecase.ImportVCardTo(
                                    vcfFile,
                                    folder,
                                    new ContactOverwritePolicy()
                                );
                                numConverted++;
                                logger.WriteLine($"Converted.");

                                File.Delete(vcfFile);
                            }
                            catch (System.Exception ex)
                            {
                                logger.WriteLine($"Error importing contact from {vcfFile}: {ex}");
                            }
                        }

                        foreach (var list in loaded.Lists)
                        {
                            logger.WriteLine($"Converting list: {list.Name}");

                            var outlookContacts = loaded.ListCards
                                .Where(lc => lc.List == list.Uid)
                                .Select(lc => contactUidTo.TryGetValue(lc.Card, out var contact) ? contact : null)
                                .OfType<ContactItem>()
                                .ToArray();

                            try
                            {
                                _outlookHelperUsecase.ImportContactGroup(
                                    folder,
                                    list.Name,
                                    list.NickName,
                                    list.Description,
                                    outlookContacts
                                );
                                numConverted++;
                                logger.WriteLine($"Converted.");
                            }
                            catch (System.Exception ex)
                            {
                                logger.WriteLine($"Error creating contact group '{list.Name}': {ex}");
                            }
                        }
                    }
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
            }
        }
    }
}