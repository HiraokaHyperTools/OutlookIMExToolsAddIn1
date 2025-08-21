using DryIoc;
using OutlookIMExToolsAddIn1.Forms;
using OutlookIMExToolsAddIn1.Usecases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookIMExToolsAddIn1.Helpers
{
    internal static class AddExtension
    {
        public static IContainer AddOutlookIMExToolsAddIn1(
            this IContainer container,
            Microsoft.Office.Interop.Outlook.Application application
        )
        {
            container.RegisterInstance(application);
            {
                var setup = Setup.With(allowDisposableTransient: true);
                container.Register<ImTbForm>(setup: setup);
                container.Register<ImTbContactsForm>(setup: setup);
                container.Register<ImForm>(setup: setup);
            }
            container.Register<OutlookHelperUsecase>(Reuse.Singleton);
            container.Register<ParseIniUsecase>(Reuse.Singleton);
            container.Register<ThunderbirdProfilesUsecase>(Reuse.Singleton);
            container.Register<ThunderbirdHelperUsecase>(Reuse.Singleton);
            container.Register<LaunchImportUsecase>(Reuse.Singleton);
            container.Register<VCardHelperUsecase>(Reuse.Singleton);
            container.Register<LaunchMailImportUsecase>(Reuse.Singleton);
            container.Register<LaunchContactImportUsecase>(Reuse.Singleton);
            container.Register<ThunderbirdAddrBookUsecase>(Reuse.Singleton);
            //NextService: container.Register<$ClassName$>(Reuse.Singleton);

            return container;
        }
    }
}