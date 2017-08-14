using System;
using System.Collections.Generic;
using System.Reflection;
using Jhu.Graywulf.Web.UI;

namespace Jhu.SkyQuery.Web.UI
{
    public class Global : FederationApplicationBase
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);

            var a = Assembly.GetAssembly(typeof(Jhu.SkyQuery.Jobs.Query.XMatchQuery));
            var v = a.GetName().Version.ToString();
            Application[Jhu.Graywulf.Web.UI.Constants.ApplicationVersion] = v;
        }

        protected override void RegisterApps()
        {
            base.RegisterApps();

            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Common.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Schema.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.MyDB.App));
            RegisterApp(typeof(Jhu.SkyQuery.Web.UI.Apps.XMatch.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Query.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Jobs.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Api.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Docs.App));
        }

        protected override void RegisterServices()
        {
            base.RegisterServices();

            RegisterService(typeof(Jhu.Graywulf.Web.Api.V1.ISchemaService));
            RegisterService(typeof(Jhu.Graywulf.Web.Api.V1.IJobsService));
            RegisterService(typeof(Jhu.Graywulf.Web.Api.V1.IDataService));
        }

        protected override void RegisterButtons()
        {
            base.RegisterButtons();

            RegisterFooterButton(new Graywulf.Web.UI.MenuButton()
            {
                Text = "status",
                NavigateUrl = Jhu.Graywulf.Web.UI.Apps.Common.Status.GetUrl()
            });

            RegisterFooterButton(new Graywulf.Web.UI.MenuButton()
            {
                Text="copyright",
                NavigateUrl = "~/Docs/99_info/01_index.aspx"
            });

            RegisterFooterButton(new Graywulf.Web.UI.MenuButton()
            {
                Text = "personnel",
                NavigateUrl = "~/Docs/99_info/01_index.aspx"
            });
        }
    }
}