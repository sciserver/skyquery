using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.SkyQuery.Web.UI.Apps.XMatch
{
    public partial class CatalogList : System.Web.UI.UserControl
    {
        private List<Catalog> catalogs;

        public List<Catalog> Catalogs
        {
            get { return catalogs; }
            set
            {
                catalogs = value;
                UpdateControl();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                foreach (ListViewDataItem li in catalogList.Items)
                {
                    var control = (CatalogForm)li.FindControl("catalog");
                    control.SaveForm(catalogs[li.DataItemIndex]);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void CatalogList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var item = (ListViewDataItem)e.Item;
                var form = (CatalogForm)item.FindControl("catalog");
                form.UpdateForm((Catalog)item.DataItem);
            }
        }

        protected void CatalogList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var item = (ListViewDataItem)e.Item;

                switch (e.CommandName)
                {
                    case "remove":
                        catalogs.RemoveAt(item.DataItemIndex);
                        UpdateControl();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public void UpdateControl()
        {
            if (catalogs != null && catalogs.Count > 0)
            {
                catalogList.DataSource = catalogs;
                catalogList.DataBind();
            }
            else
            {
                catalogList.DataSource = null;
                catalogList.DataBind();
            }
        }
    }
}