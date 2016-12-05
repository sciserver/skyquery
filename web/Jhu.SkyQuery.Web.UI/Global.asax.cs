using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Jhu.Graywulf.Web.UI;

namespace Jhu.SkyQuery.Web.UI
{
    public class Global : FederationApplicationBase
    {
        protected override void RegisterApps()
        {
            base.RegisterApps();

            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Common.App));
            RegisterApp(typeof(Jhu.SkyQuery.Web.UI.Apps.XMatch.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Schema.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Query.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.MyDB.App));
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

            RegisterFooterButton(new Graywulf.Web.UI.Controls.MenuButton()
            {
                Text="copyright",
                NavigateUrl = "~/Docs/99_info/03_copyright.aspx"
            });

            RegisterFooterButton(new Graywulf.Web.UI.Controls.MenuButton()
            {
                Text = "personnel",
                NavigateUrl = "~/Docs/99_info/01_personnel.aspx"
            });
        }
    }
}