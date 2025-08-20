using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}