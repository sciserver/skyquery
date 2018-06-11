using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Web.UI;
using Jhu.SkyQuery.Sql.CodeGeneration;

namespace Jhu.SkyQuery.Web.UI.Apps.XMatch
{
    public partial class CatalogForm : FederationUserControlBase
    {
        protected void CoordinateMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshCoordinateVisibility();
        }

        protected void ErrorMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshErrorVisibility();
        }

        public void UpdateForm(Catalog catalog)
        {
            var sm = FederationContext.SchemaManager;
            var ds = sm.Datasets[catalog.DatasetName];
            var t = (TableOrView)ds.GetObject(catalog.TableUniqueKey);

            RefreshColumnLists(catalog, t);
            
            table.Text = String.Format("{0}:{1}", ds.Name, t.DisplayName);
            alias.Text = catalog.Alias;
            coordinateMode.SelectedValue = catalog.CoordinateMode.ToString();
            ra.SelectedValue = catalog.RaColumn;
            dec.SelectedValue = catalog.DecColumn;
            errorMode.SelectedValue = catalog.ErrorMode.ToString();
            errorValue.Text = catalog.ErrorValue.ToString("0.0000");
            errorColumn.SelectedValue = catalog.ErrorColumn;
            errorMin.Text = catalog.ErrorMin.ToString("0.0000");
            errorMax.Text = catalog.ErrorMin.ToString("0.0000");
            where.Text = catalog.Where;

            RefreshCoordinateVisibility();
            RefreshErrorVisibility();
        }

        public void SaveForm(Catalog catalog)
        {
            catalog.Alias = alias.Text;
            catalog.CoordinateMode = (CoordinateMode)Enum.Parse(typeof(CoordinateMode), coordinateMode.SelectedValue);
            switch (catalog.CoordinateMode)
            {
                case CoordinateMode.Automatic:
                    break;
                case CoordinateMode.Manual:
                    catalog.RaColumn = ra.SelectedValue;
                    catalog.DecColumn = dec.SelectedValue;
                    break;
                default:
                    throw new NotImplementedException();
            }
            catalog.ErrorMode = (ErrorMode)Enum.Parse(typeof(ErrorMode), errorMode.SelectedValue);
            switch (catalog.ErrorMode)
            {
                case ErrorMode.Constant:
                    catalog.ErrorValue = double.Parse(errorValue.Text);
                    break;
                case ErrorMode.Column:
                    catalog.ErrorColumn = errorColumn.SelectedValue;
                    catalog.ErrorMin = double.Parse(errorMin.Text);
                    catalog.ErrorMax = double.Parse(errorMax.Text);
                    break;
                default:
                    throw new NotImplementedException();
            }
            catalog.Where = where.Text;

            catalog.Columns.Clear();

            foreach (ListItem li in columnList.Items)
            {
                if (li.Selected)
                {
                    catalog.Columns.Add(li.Value);
                }
            }
        }

        private void RefreshColumnLists(Catalog catalog, TableOrView table)
        {
            ra.Items.Clear();
            dec.Items.Clear();
            errorColumn.Items.Clear();
            columnList.Items.Clear();

            foreach (var column in table.Columns.Values.OrderBy(c => c.ID))
            {
                ra.Items.Add(column.Name);
                dec.Items.Add(column.Name);
                errorColumn.Items.Add(column.Name);

                var li = new ListItem()
                {
                    Text = column.IsKey ?
                        String.Format("<img src=\"{0}\" />&nbsp;{1}", Page.ResolveUrl("images/icon_key.png"), column.Name) :
                        column.Name,
                    Value = column.Name
                };

                if (catalog.Columns.Contains(column.Name))
                {
                    li.Selected = true;
                }

                columnList.Items.Add(li);
            }
        }

        private void RefreshCoordinateVisibility()
        {
            switch (coordinateMode.SelectedValue)
            {
                case "Automatic":
                    coordinatesRow.Visible = false;
                    break;
                case "Manual":
                    coordinatesRow.Visible = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void RefreshErrorVisibility()
        {
            switch (errorMode.SelectedValue)
            {
                case "Constant":
                    errorRow.Visible = true;
                    errorColumnRow.Visible = false;
                    errorLimitsRow.Visible = false;
                    break;
                case "Column":
                    errorRow.Visible = false;
                    errorColumnRow.Visible = true;
                    errorLimitsRow.Visible = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}