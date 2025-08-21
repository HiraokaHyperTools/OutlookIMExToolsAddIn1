using Microsoft.Office.Interop.Outlook;
using OutlookIMExToolsAddIn1.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace OutlookIMExToolsAddIn1.Usecases
{
    public class OutlookHelperUsecase
    {
        private readonly Application _app;

        public OutlookHelperUsecase(
            Microsoft.Office.Interop.Outlook.Application app
        )
        {
            _app = app;
        }

        public MAPIFolder GetCurrentFolder()
        {
            return _app.ActiveExplorer().CurrentFolder;
        }

        public MAPIFolder GetDefaultContactsFolder()
        {
            return _app.GetNamespace("MAPI").GetDefaultFolder(OlDefaultFolders.olFolderContacts);
        }

        public string FormatFolderNameTree(MAPIFolder folder)
        {
            var names = new List<string>();

            while (folder != null)
            {
                names.Add(folder.Name ?? "Unnamed");
                folder = folder.Parent as MAPIFolder;
            }

            return string.Join(" > ", names.AsEnumerable().Reverse());
        }

        public MAPIFolder SelectFolder()
        {
            return _app.GetNamespace("MAPI").PickFolder();
        }

        public IConverterSession CreateConverterSession()
        {
            var type = Type.GetTypeFromCLSID(new Guid("4e3a7680-b77a-11d0-9da5-00c04fd65685"));
            return (IConverterSession)Activator.CreateInstance(type);
        }

        public MailItem LoadPseudoReceived()
        {
            var mail = (MailItem)_app
                .GetNamespace("MAPI")
                .OpenSharedItem(
                    Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "pseudoReceived.msg"
                    )
                );
            return mail;
        }

        public MAPIFolder AddOrGetFolder(Folders folders, string displayName)
        {
            foreach (MAPIFolder folder in folders)
            {
                if (folder.Name == displayName)
                {
                    return folder;
                }
            }
            var newFolder = folders.Add(displayName);
            if (newFolder == null)
            {
                throw new COMException("Failed to create folder: " + displayName);
            }
            return newFolder;
        }

        /// <see cref="https://learn.microsoft.com/ja-jp/office/client-developer/outlook/mapi/mapi-constants"/>
        private const uint CCSF_SMTP = 2;

        /// <see cref="https://learn.microsoft.com/ja-jp/office/client-developer/outlook/mapi/mapi-constants"/>
        private const uint CCSF_USE_RTF = 0x80;

        /// <see cref="https://learn.microsoft.com/ja-jp/office/client-developer/outlook/mapi/mapi-constants"/>
        private const uint CCSF_PLAIN_TEXT_ONLY = 0x1000;

        /// <see cref="https://learn.microsoft.com/ja-jp/office/client-developer/outlook/mapi/mapi-constants"/>
        private const uint CCSF_GLOBAL_MESSAGE = 0x00200000;

        /// <see cref="https://learn.microsoft.com/ja-jp/previous-versions/windows/desktop/oe/oe-mimesavetype?redirectedfrom=MSDN"/>
        private const uint SAVE_RFC1521 = 1;

        /// <see cref="https://learn.microsoft.com/sk-sk/previous-versions/windows/desktop/oe/oe-encodingtype"/>
        private const uint IET_BASE64 = 1;

        /// <see cref="https://learn.microsoft.com/ja-jp/previous-versions/windows/desktop/oe/oe-csetapplytype"/>
        private const uint CSET_APPLY_UNTAGGED = 0;

        public void Convert(
            MAPIFolder folder,
            Stream stream,
            IConverterSession converterSession,
            MailItem emptyMail
        )
        {
            var mail = (MailItem)emptyMail.Copy();
            try
            {
                var message = (IMessage)mail.MAPIOBJECT;

                int hr = converterSession.MIMEToMAPI(
                    new StreamWrapper(stream),
                    message,
                    IntPtr.Zero,
                    CCSF_SMTP
                );
                if (hr < 0)
                {
                    Marshal.ThrowExceptionForHR(hr);
                }

                if (((Folder)mail.Parent).EntryID != folder.EntryID)
                {
                    mail.Move(folder);
                }
                mail.Save();
            }
            finally
            {
                Marshal.ReleaseComObject(mail);
            }
        }

        private class StreamWrapper : IStream
        {
            private Stream _stream;

            public StreamWrapper(Stream stream)
            {
                _stream = stream;
            }

            void IStream.Read(byte[] pv, int cb, IntPtr pcbRead)
            {
                if (pcbRead != IntPtr.Zero)
                {
                    Marshal.WriteInt32(pcbRead, 0);
                }
                int bytesRead = _stream.Read(pv, 0, cb);
                if (pcbRead != IntPtr.Zero)
                {
                    Marshal.WriteInt32(pcbRead, bytesRead);
                }
            }

            void IStream.Write(byte[] pv, int cb, IntPtr pcbWritten)
            {
                throw new NotImplementedException();
            }

            void IStream.Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
            {
                long newPosition = _stream.Seek(dlibMove, (SeekOrigin)dwOrigin);

                if (plibNewPosition != IntPtr.Zero)
                {
                    Marshal.WriteInt64(plibNewPosition, newPosition);
                }
            }

            void IStream.SetSize(long libNewSize)
            {
                throw new NotImplementedException();
            }

            void IStream.CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
            {
                throw new NotImplementedException();
            }

            void IStream.Commit(int grfCommitFlags)
            {
                throw new NotImplementedException();
            }

            void IStream.Revert()
            {
                throw new NotImplementedException();
            }

            void IStream.LockRegion(long libOffset, long cb, int dwLockType)
            {
                throw new NotImplementedException();
            }

            void IStream.UnlockRegion(long libOffset, long cb, int dwLockType)
            {
                throw new NotImplementedException();
            }

            void IStream.Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag)
            {
                pstatstg = new System.Runtime.InteropServices.ComTypes.STATSTG
                {
                    cbSize = _stream.Length,
                    type = 2, // STREAM
                    grfMode = 0, // Read/Write
                    grfLocksSupported = 0, // No locks supported
                    clsid = Guid.Empty,
                    mtime = new System.Runtime.InteropServices.ComTypes.FILETIME(),
                    ctime = new System.Runtime.InteropServices.ComTypes.FILETIME(),
                    atime = new System.Runtime.InteropServices.ComTypes.FILETIME(),
                };
            }

            void IStream.Clone(out IStream ppstm)
            {
                throw new NotImplementedException();
            }
        }

        public ContactItem ImportVCardTo(
            string vcfFile,
            MAPIFolder folder,
            ContactOverwritePolicy policy
        )
        {
            var contact = (ContactItem)_app.GetNamespace("MAPI").OpenSharedItem(vcfFile);

            if (((Folder)contact.Parent).EntryID != folder.EntryID)
            {
                contact.Move(folder);
            }
            contact.Save();

            return contact;
        }

        public DistListItem ImportContactGroup(
            MAPIFolder folder,
            string name,
            string nickName,
            string description,
            ContactItem[] outlookContacts
        )
        {
            var session = _app.GetNamespace("MAPI").AddressLists.Session;
            var dli = (DistListItem)_app.CreateItem(OlItemType.olDistributionListItem);
            dli.Subject = name;
            dli.DLName = name;
            foreach (var one in outlookContacts)
            {
                Recipient recipient = new string[] { one.Email1Address, one.Email2Address, one.Email3Address }
                    .Where(it => !string.IsNullOrEmpty(it))
                    .Select(session.CreateRecipient)
                    .FirstOrDefault();
                if (recipient != null && recipient.Resolve())
                {
                    dli.AddMember(recipient);
                }
            }

            if (((Folder)dli.Parent).EntryID != folder.EntryID)
            {
                dli.Move(folder);
            }
            dli.Save();

            return dli;
        }
    }
}