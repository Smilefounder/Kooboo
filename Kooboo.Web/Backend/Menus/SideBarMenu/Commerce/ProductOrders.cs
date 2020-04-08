using Kooboo.Data.Context;
using Kooboo.Data.Language;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Web.Menus.SideBarMenu.Commerce
{
    public class ProductOrders : ISideBarMenu
    {
        public SideBarSection Parent => SideBarSection.Commerce;

        public string Name => "ProductOrders";

        public string Icon => "";

        public string Url => "ECommerce/Orders";

        public int Order => 2;

        public List<ICmsMenu> SubItems { get; set; }

        public string GetDisplayName(RenderContext Context)
        {
            return Hardcoded.GetValue("Product Orders", Context);
        }
    }
}
