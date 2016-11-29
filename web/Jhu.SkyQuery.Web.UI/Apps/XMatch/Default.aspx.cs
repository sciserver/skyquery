﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Web.UI;
using Jhu.Graywulf.Web.Api.V1;
using Jhu.Graywulf.Jobs.Query;
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
        private SqlQuery query;

        protected void Page_Init(object sender, EventArgs e)
        {
            LoadSessionXMatch();
            catalogList.XMatch = xmatch;
            xmatchForm.XMatch = xmatch;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            errorMessagePanel.Visible = false;

            if (!IsPostBack)
            {
                RefreshDatasetList();
                RefreshTableList();
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
                    string q;
                    int[] s;
                    bool selectionOnly = true;

                    if (HasQueryInSession())
                    {
                        GetQueryFromSession(out q, out s, out selectionOnly);
                    }

                    SetQueryInSession(query.QueryString, null, selectionOnly);

                    Response.Redirect(Jhu.Graywulf.Web.UI.Apps.Query.Default.GetUrl());
                }
            }
        }

        private void LoadSessionXMatch()
        {
            xmatch = (CodeGen.XMatch)(Session["Jhu.SkyQuery.Web.UI.Apps.XMatch"] ?? new CodeGen.XMatch());
        }

        private void SaveSessionXMatch()
        {
            Session["Jhu.SkyQuery.Web.UI.Apps.XMatch"] = xmatch;
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
                var uf = UserDatabaseFactory.Create(RegistryContext.Federation);
                var mydbds = uf.GetUserDatabases(RegistryUser);

                foreach (var key in mydbds.Keys)
                {
                    var mydbli = new ListItem(key, key);
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

        private void GenerateQuery()
        {
            xmatch.Validate(FederationContext.SchemaManager);

            var cg = new SkyQueryCodeGenerator();
            var sql = cg.GenerateXMatchQuery(xmatch, FederationContext.SchemaManager);
            var queryJob = new QueryJob(sql, JobQueue.Long);
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
                errorMessage.Text = "Error: " + ex.Message;
                errorMessagePanel.Visible = true;
                return false;
            }
        }
    }
}