using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Helpers
{
    [ComImport]
    [Guid("00020307-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMessage
    {
        [PreserveSig]
        int GetLastError(int hResult, uint ulFlags, IntPtr lppMAPIError);
        void SaveChanges();
        void GetProps();
        void GetPropList();
        void OpenProperty();
        void SetProps();
        void DeleteProps();
        void CopyTo();
        void CopyProps();
        void GetNamesFromIDs();
        void GetIDsFromNames();


        void GetAttachmentTable();
        void OpenAttach();
        void CreateAttach();
        void DeleteAttach();
        void GetRecipientTable();
        void ModifyRecipients();
        void SubmitMessage();
        void SetReadFlag();
    }
}
