using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SkyQuery.CodeGen;

namespace Jhu.SkyQuery.Web.UI.Apps.XMatch
{
    public partial class XMatchForm : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RefreshXMatchColumnList();
            }
        }

        protected void RegionValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!String.IsNullOrWhiteSpace(region.Text))
            {
                try
                {
                    Spherical.Region.Parse(region.Text);
                    args.IsValid = true;
                }
                catch (Exception ex)
                {
                    regionValidator.ErrorMessage = "Cannot parse region string: " + ex.Message;
                    args.IsValid = false;
                }
            }
        }

        private void RefreshXMatchColumnList()
        {
            var tr = new SkyQuery.Parser.BayesFactorXMatchTableReference();

            xmatchColumnList.Items.Clear();

            foreach (var cr in tr.ColumnReferences)
            {
                var li = new ListItem(cr.ColumnName, cr.ColumnName);
                xmatchColumnList.Items.Add(li);
            }
        }

        public void UpdateForm(CodeGen.XMatch xmatch)
        {
            targetTable.Text = xmatch.TargetTable;
            bayesFactor.Text = xmatch.BayesFactor.ToString();
            region.Text = xmatch.Region;

            foreach (ListItem li in xmatchColumnList.Items)
            {
                li.Selected = xmatch.Columns.Contains(li.Value);
            }
        }

        public void SaveForm(CodeGen.XMatch xmatch)
        {
            xmatch.TargetTable = targetTable.Text;
            xmatch.BayesFactor = double.Parse(bayesFactor.Text);
            xmatch.Region = region.Text;
            xmatch.Columns.Clear();

            foreach (ListItem li in xmatchColumnList.Items)
            {
                if (li.Selected)
                {
                    xmatch.Columns.Add(li.Value);
                }
            }
        }
    }
}