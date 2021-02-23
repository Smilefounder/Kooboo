using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.ViewModels
{
    public class ItemDefineViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DefineType Type { get; set; }
        public IEnumerable<KeyValuePair<Guid, string>> Options { get; set; }
        public enum DefineType
        {
            Text = 0,
            Option = 1
        }
    }

}
