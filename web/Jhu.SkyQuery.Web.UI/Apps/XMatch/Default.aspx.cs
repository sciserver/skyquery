using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Web.UI;

namespace Jhu.SkyQuery.Web.UI.Apps.XMatch
{
    public partial class Default : FederationPageBase
    {
        public static string GetUrl()
        {
            return "~/Apps/XMatch/Default.aspx";
        }

        protected List<Catalog> Catalogs
        {
            get
            {
                return (List<Catalog>)(Session["Jhu.SkyQuery.Web.UI.Apps.XMatch.Catalogs"] ?? new List<Catalog>());
            }
            set
            {
                Session["Jhu.SkyQuery.Web.UI.Apps.XMatch.Catalogs"] = value;
            }   
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            catalogList.Catalogs = Catalogs;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RefreshDatasetList();
                RefreshTableList();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Catalogs = catalogList.Catalogs;

            if (catalogList.Catalogs.Count == 0)
            {
                introForm.Visible = true;
                catalogListPanel.Visible = false;
            }
            else
            {
                introForm.Visible = false;
                catalogListPanel.Visible = true;
            }
        }

        protected void DatasetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshTableList();
        }

        protected void AddCatalog_Click(object sender, EventArgs e)
        {
            // TODO: validate

            var catalog = new Catalog()
            {
                DatasetName = datasetList.SelectedValue,
                TableUniqueKey = tableList.SelectedValue
            };

            catalogList.Catalogs.Add(catalog);
            catalogList.UpdateControl();
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