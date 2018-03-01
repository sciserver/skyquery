using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Web.UI;
using Jhu.Graywulf.Web.Api.V1;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.SkyQuery.CodeGen;

namespace Jhu.SkyQuery.Web.UI.Apps.XMatch
{
    public partial class Default : Jhu.Graywulf.Web.UI.Apps.Query.QueryPageBase
    {
        public static string GetUrl()
        {
            return "~/Apps/XMatch/Default.aspx";
        }

        private CodeGen.XMatch xmatch;
        private SqlQuery query;

        protected void Page_Init(object sender, EventArgs e)
        {
            LoadSessionXMatch();
            catalogList.XMatch = xmatch;
            xmatchForm.XMatch = xmatch;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            messageBar.Visible = false;

            if (!IsPostBack)
            {
                RefreshDatasetList();
                RefreshTableList();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!jobResultsForm.Visible)
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
            }
            else
            {
                introForm.Visible = false;
                catalogListPanel.Visible = false;
            }
        }

        protected void DatasetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshTableList();
        }

        protected void AddCatalog_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveForm();

                if (!String.IsNullOrWhiteSpace(tableList.SelectedValue))
                {
                    var ds = FederationContext.SchemaManager.Datasets[datasetList.SelectedValue];
                    var table = (TableOrView)ds.GetObject(tableList.SelectedValue);

                    xmatch.AddCatalog(table);

                    catalogList.UpdateForm();
                }
            }
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            xmatch.Catalogs.Clear();
            catalogList.UpdateForm();
        }

        protected void GenerateQuery_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                SaveForm();

                if (ValidateQuery())
                {
                    SaveSessiongQuery();
                    Response.Redirect(Jhu.Graywulf.Web.UI.Apps.Query.Default.GetUrl());
                }
            }
        }

        protected void SubmitJob_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                SaveForm();

                if (ValidateQuery())
                {
                    SaveSessiongQuery();

                    var q = CreateQueryJob(query.Parameters.QueryString, JobQueue.Long);

                    if (q != null)
                    {
                        ScheduleQuery(q);
                        introForm.Visible = false;
                        catalogListPanel.Visible = false;
                        toolbar.Visible = false;
                        jobResultsForm.Visible = true;
                    }
                }
            }
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            Response.Redirect(Jhu.Graywulf.Web.UI.Apps.Jobs.Default.GetUrl("query"));
        }

        private void LoadSessionXMatch()
        {
            xmatch = (CodeGen.XMatch)(Session["Jhu.SkyQuery.Web.UI.Apps.XMatch"] ?? new CodeGen.XMatch());
        }

        private void SaveSessionXMatch()
        {
            Session["Jhu.SkyQuery.Web.UI.Apps.XMatch"] = xmatch;
        }

        private void SaveSessiongQuery()
        {
            string q;
            int[] s;
            bool selectionOnly = true;

            if (HasQueryInSession())
            {
                GetQueryFromSession(out q, out s, out selectionOnly);
            }

            SetQueryInSession(query.Parameters.QueryString, null, selectionOnly);
        }

        private void UpdateForm()
        {
            xmatchForm.UpdateForm();
            catalogList.UpdateForm();
        }

        private void SaveForm()
        {
            xmatchForm.SaveForm();
            catalogList.SaveForm();
            SaveSessionXMatch();
        }

        private void RefreshDatasetList()
        {
            datasetList.Items.Clear();

            // Add MyDB etc. to the beginning of the list
            if (FederationContext.RegistryUser != null)
            {
                var uf = UserDatabaseFactory.Create(FederationContext);
                var mydbds = uf.GetUserDatabases(RegistryUser);

                foreach (var key in mydbds.Keys)
                {
                    var mydbli = CreateDatasetListItem(mydbds[key]);
                    mydbli.Attributes.Add("class", "ToolbarControlHighlight");
                    datasetList.Items.Add(mydbli);
                }
            }

            // Add other registered catalogs     
            FederationContext.SchemaManager.Datasets.LoadAll(false);

            // TODO: this needs to be modified here, use flags instead of filtering on name!
            foreach (var dsd in FederationContext.SchemaManager.Datasets.Values.Where(k =>
                k.Name != Graywulf.Registry.Constants.UserDbName &&
                k.Name != Graywulf.Registry.Constants.CodeDbName &&
                k.Name != Graywulf.Registry.Constants.TempDbName).OrderBy(k => k.Name))
            {
                var li = CreateDatasetListItem(dsd);
                datasetList.Items.Add(li);
            }
        }

        private ListItem CreateDatasetListItem(DatasetBase ds)
        {
            var li = new ListItem(ds.Name, ds.Name);

            if (ds.IsInError)
            {
                li.Text += " (not available)";
            }

            return li;
        }

        private void RefreshTableList()
        {
            tableList.Items.Clear();

            try
            {
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
            catch (Exception ex)
            {
                tableList.Items.Clear();
                var li = new ListItem("(not available)", "");
                tableList.Items.Add(li);
            }
        }

        private void GenerateQuery()
        {
            xmatch.Validate(FederationContext.SchemaManager);

            var cg = new SkyQueryCodeGenerator();
            var sql = cg.GenerateXMatchQuery(xmatch, FederationContext.SchemaManager);
            var queryJob = new QueryJob(sql);
            query = queryJob.CreateQuery(FederationContext);
            query.Verify();
        }

        private bool ValidateQuery()
        {
            try
            {
                GenerateQuery();
                return true;
            }
            catch (Exception ex)
            {
                messageBar.Text = "Error: " + ex.Message;
                messageBar.Visible = true;
                return false;
            }
        }
    }
}