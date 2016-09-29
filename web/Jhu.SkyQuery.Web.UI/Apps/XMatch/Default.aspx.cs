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
    public partial class Default : FederationPageBase
    {
        public static string GetUrl()
        {
            return "~/Apps/XMatch/Default.aspx";
        }

        private CodeGen.XMatch xmatch;

        protected void Page_Init(object sender, EventArgs e)
        {
            xmatch = (CodeGen.XMatch)(Session["Jhu.SkyQuery.Web.UI.Apps.XMatch"] ?? new CodeGen.XMatch());
            catalogList.XMatch = xmatch;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                xmatchForm.UpdateForm(xmatch);
                RefreshDatasetList();
                RefreshTableList();
            }
            else
            {
                xmatchForm.SaveForm(xmatch);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (xmatch.Catalogs.Count == 0)
            {
                introForm.Visible = true;
                catalogListPanel.Visible = false;
            }
            else
            {
                introForm.Visible = false;
                catalogListPanel.Visible = true;
            }

            Session["Jhu.SkyQuery.Web.UI.Apps.XMatch"] = xmatch;
        }

        protected void DatasetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshTableList();
        }

        protected void AddCatalog_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tableList.SelectedValue))
            {
                var catalog = new Catalog()
                {
                    DatasetName = datasetList.SelectedValue,
                    TableUniqueKey = tableList.SelectedValue
                };

                xmatch.Catalogs.Add(catalog);
                catalogList.UpdateControl();
            }
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            xmatch.Catalogs.Clear();
            catalogList.UpdateControl();
        }

        protected void GenerateQuery_Click(object sender, EventArgs e)
        {
            Validate();

            if (IsValid)
            {
                // TODO
            }
        }

        private void RefreshDatasetList()
        {
            datasetList.Items.Clear();

            // Add MyDB as the first item
            // TODO: extend this in case of multiple user databases
            if (FederationContext.RegistryUser != null)
            {
                var mydbli = new ListItem(FederationContext.MyDBDataset.Name, FederationContext.MyDBDataset.Name);
                mydbli.Attributes.Add("class", "ToolbarControlHighlight");
                datasetList.Items.Add(mydbli);
            }

            // Add other registered catalogs     
            FederationContext.SchemaManager.Datasets.LoadAll(false);

            // TODO: this needs to be modified here, use flags instead of filtering on name!
            foreach (var dsd in FederationContext.SchemaManager.Datasets.Values.Where(k =>
                k.Name != Graywulf.Registry.Constants.UserDbName &&
                k.Name != Graywulf.Registry.Constants.CodeDbName &&
                k.Name != Graywulf.Registry.Constants.TempDbName).OrderBy(k => k.Name))
            {
                datasetList.Items.Add(dsd.Name);
            }
        }

        private void RefreshTableList()
        {
            tableList.Items.Clear();

            if (FederationContext.SchemaManager.Datasets.ContainsKey(datasetList.SelectedValue))
            {
                var li = new ListItem("(select item)", "");
                tableList.Items.Add(li);

                var dataset = FederationContext.SchemaManager.Datasets[datasetList.SelectedValue];

                dataset.Tables.LoadAll(dataset.IsMutable);
                dataset.Views.LoadAll(dataset.IsMutable);

                var tables =
                    dataset.Tables.Values.Cast<TableOrView>().Concat(
                    dataset.Views.Values).OrderBy(i => i.DisplayName);

                foreach (var t in tables)
                {
                    li = new ListItem(t.DisplayName, t.UniqueKey);
                    tableList.Items.Add(li);
                }
            }

            if (tableList.Items.Count <= 1)
            {
                tableList.Items.Clear();

                var li = new ListItem("(no items)", "");
                tableList.Items.Add(li);
            }
        }
    }
}