using System;
using System.Collections.Generic;
using System.Reflection;
using Jhu.Graywulf.Web.UI;

namespace Jhu.SkyQuery.Web.UI
{
    public class Global : FederationApplicationBase
    {
        protected override void OnApplicationStart()
        {
            base.OnApplicationStart();

            var a = Assembly.GetAssembly(typeof(Jhu.SkyQuery.Sql.Jobs.Query.XMatchQuery));
            var v = a.GetName().Version.ToString();
            Application[Jhu.Graywulf.Web.UI.Constants.ApplicationVersion] = v;
        }

        protected override void OnRegisterApps()
        {
            base.OnRegisterApps();

            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Common.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Schema.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.MyDB.App));
            RegisterApp(typeof(Jhu.SkyQuery.Web.UI.Apps.XMatch.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Query.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Jobs.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Api.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Docs.App));
        }

        protected override void OnRegisterServices()
        {
            base.OnRegisterServices();

            RegisterService(typeof(Jhu.Graywulf.Web.Api.V1.ISchemaService));
            RegisterService(typeof(Jhu.Graywulf.Web.Api.V1.IJobsService));
            RegisterService(typeof(Jhu.Graywulf.Web.Api.V1.IDataService));
        }

        protected override void OnRegisterButtons()
        {
            base.OnRegisterButtons();
            
            RegisterFooterButton(new Graywulf.Web.UI.MenuButton()
            {
                Text = "status",
                NavigateUrl = Jhu.Graywulf.Web.UI.Apps.Common.Status.GetUrl()
            });

            RegisterFooterButton(new Graywulf.Web.UI.MenuButton()
            {
                Text = "about",
                NavigateUrl = "~/Assets/Docs/99_info/00_index.aspx"
            });
        }
    }
}