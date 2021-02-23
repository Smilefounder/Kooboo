using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Data.Context;
using Kooboo.Data.Language;

namespace Kooboo.Web.Menus.SideBarMenu.Commerce
{
    public class ProductTypes : ISideBarMenu
    {
        public SideBarSection Parent => SideBarSection.Commerce;

        public string Name => "Product Types";

        public string Icon => "";

        public string Url => "ECommerce/ProductTypes";

        public int Order => 0;

        public List<ICmsMenu> SubItems { get; set; }

        public string GetDisplayName(RenderContext Context)
        {
            return Hardcoded.GetValue("Product Types", Context);
        }
    }
}