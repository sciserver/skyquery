using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.Graywulf.Web.UI;

namespace Jhu.SkyQuery.Web.UI
{
    public partial class Default : PageBase
    {
        public static string GetUrl()
        {
            return "~/Default.aspx";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WelcomeForm.Text = String.Format("Welcome to {0}", Application[Jhu.Graywulf.Web.UI.Constants.ApplicationShortTitle]);
        }
    }
}