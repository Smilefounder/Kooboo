using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Data.Context;
using Kooboo.Data.Language;

namespace Kooboo.Web.Menus.SideBarMenu.Commerce
{
    public class Customers : ISideBarMenu
    {
        public SideBarSection Parent => SideBarSection.Commerce;

        public string Name => "Customers";

        public string Icon => "";

        public string Url => "ECommerce/Customers";

        public int Order => 4;

        public List<ICmsMenu> SubItems { get; set; }

        public string GetDisplayName(RenderContext Context)
        {
            return Hardcoded.GetValue("Customers", Context);
        }
    }
}