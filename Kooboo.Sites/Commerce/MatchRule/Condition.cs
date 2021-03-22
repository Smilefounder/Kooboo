using Kooboo.Data.Definition;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule
{
    public class Condition
    {
        public Guid Id { get; set; }
        public string Left { get; set; }
        public Comparer Comparer { get; set; }
        public string Right { get; set; }
    }
}
