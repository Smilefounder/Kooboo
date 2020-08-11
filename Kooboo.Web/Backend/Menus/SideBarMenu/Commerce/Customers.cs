using Kooboo.Data.Context;
using Kooboo.Data.Language;
using Kooboo.Web.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Web.Backend.Menus.SideBarMenu.Commerce
{
    public class Customers : ISideBarMenu
    {
        public SideBarSection Parent => SideBarSection.Commerce;

        public string Name => "Customers";

        public string Icon => "";

        public string Url => "ECommerce/Customers";

        public int Order => 6;

        public List<ICmsMenu> SubItems { get; set; }

        public string GetDisplayName(RenderContext Context)
        {
            return Hardcoded.GetValue("Customers management", Context);
        }
    }
}
