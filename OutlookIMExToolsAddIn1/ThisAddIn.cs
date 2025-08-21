using DryIoc;
using OutlookIMExToolsAddIn1.Forms;
using OutlookIMExToolsAddIn1.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace OutlookIMExToolsAddIn1
{
    public partial class ThisAddIn
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        private DryIoc.IContainer _resolver;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            _resolver = new DryIoc.Container()
                .AddOutlookIMExToolsAddIn1(this.Application);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see https://go.microsoft.com/fwlink/?LinkId=506785

            _resolver?.Dispose();
            _resolver = null;

            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            _disposables.Clear();
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new Ribbon1(
                importFromThunderbird: ImportFromThunderbird,
                importContactsFromThunderbird: ImportContactsFromThunderbird
            );
        }

        private void ImportContactsFromThunderbird()
        {
            _resolver.Resolve<ImTbContactsForm>()
                .AddTo(_disposables)
                .Show();
        }

        private void ImportFromThunderbird()
        {
            _resolver.Resolve<ImTbForm>()
                .AddTo(_disposables)
                .Show();
        }
    }
}
