using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SkyQuery.CodeGen;

namespace Jhu.SkyQuery.Web.UI.Apps.XMatch
{
    public partial class CatalogList : System.Web.UI.UserControl
    {
        private CodeGen.XMatch xmatch;

        public CodeGen.XMatch XMatch
        {
            get { return xmatch; }
            set
            {
                xmatch = value;
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
                    control.SaveForm(xmatch.Catalogs[li.DataItemIndex]);
                }
            }
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
                        xmatch.Catalogs.RemoveAt(item.DataItemIndex);
                        UpdateControl();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public void UpdateControl()
        {
            if (xmatch.Catalogs != null && xmatch.Catalogs.Count > 0)
            {
                catalogList.DataSource = xmatch.Catalogs;
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