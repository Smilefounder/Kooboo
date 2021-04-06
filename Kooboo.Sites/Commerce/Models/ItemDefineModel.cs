using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models
{
    public class ItemDefineModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DefineType Type { get; set; }
        public KeyValuePair<Guid, string>[] Options { get; set; }
        public enum DefineType
        {
            Text = 0,
            Option = 1
        }
    }

}
