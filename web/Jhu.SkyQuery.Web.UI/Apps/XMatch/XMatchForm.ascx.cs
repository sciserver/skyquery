using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Web.UI;
using Jhu.SkyQuery.CodeGen;

namespace Jhu.SkyQuery.Web.UI.Apps.XMatch
{
    public partial class XMatchForm : FederationUserControlBase
    {
        private CodeGen.XMatch xmatch;

        public CodeGen.XMatch XMatch
        {
            get { return xmatch; }
            set
            {
                xmatch = value;
                UpdateForm();
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
                var li = new ListItem();
                li.Value = cr.ColumnName;
                li.Text = tr.TableOrView.Columns[cr.ColumnName].IsKey ?
                    String.Format("<img src=\"{0}\" />&nbsp;{1}", Page.ResolveUrl("images/icon_key.png"), cr.ColumnName) :
                    cr.ColumnName;

                xmatchColumnList.Items.Add(li);
            }
        }

        private void RefreshTargetDatasetList()
        {
            targetDataset.Items.Clear();

            // Add MyDB etc. to the beginning of the list
            var uf = UserDatabaseFactory.Create(FederationContext.Federation);
            var mydbds = uf.GetUserDatabases(FederationContext.RegistryUser);

            foreach (var key in mydbds.Keys)
            {
                var mydbli = new ListItem(key, key);
                mydbli.Attributes.Add("class", "ToolbarControlHighlight");
                targetDataset.Items.Add(mydbli);
            }
        }

        public void UpdateForm()
        {
            RefreshTargetDatasetList();
            RefreshXMatchColumnList();

            targetDataset.SelectedValue = xmatch.TargetDataset;
            targetTable.Text = xmatch.TargetTable;
            bayesFactor.Text = xmatch.BayesFactor.ToString();
            region.Text = xmatch.Region;

            foreach (ListItem li in xmatchColumnList.Items)
            {
                li.Selected = xmatch.Columns.Contains(li.Value);
            }
        }

        public void SaveForm()
        {
            xmatch.TargetDataset = targetDataset.SelectedValue;
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