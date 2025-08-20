using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Helpers
{
    /// <see cref="https://learn.microsoft.com/ja-jp/office/client-developer/outlook/mapi/mapi-constants"/>
    /// <see cref="https://github.com/microsoft/mfcmapi/blob/434f6914fd2a3bed4e71aa3619e19ebc7cf72037/core/mapi/mapiMime.h#L12"/>
    [ComImport]
    [Guid("4b401570-b77b-11d0-9da5-00c04fd65685")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IConverterSession
    {
        void SetAdrBook(IntPtr pab);

        [PreserveSig]
        int SetEncoding(uint et);

        void PlaceHolder1();

        [PreserveSig]
        int MIMEToMAPI(
            [MarshalAs(UnmanagedType.Interface)] IStream pstm,
            [MarshalAs(UnmanagedType.Interface)] IMessage pmsg,
            IntPtr pszSrcSrv,
            uint ulFlags
            );

        void MAPIToMIMEStm(IntPtr pmsg, IStream pstm, uint ulFlags);

        void PlaceHolder2();
        void PlaceHolder3();
        void PlaceHolder4();

        void SetTextWrapping(bool fWrapText, uint ulWrapWidth);

        [PreserveSig]
        int SetSaveFormat(uint mstSaveFormat);

        void PlaceHolder5();

        [PreserveSig]
        int SetCharset(bool fApply, IntPtr hcharset, uint csetapplytype);
    }
}
