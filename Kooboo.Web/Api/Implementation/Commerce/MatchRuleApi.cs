using Kooboo.Api;
using Kooboo.Sites.Commerce;
using Kooboo.Sites.Commerce.MatchRule.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Kooboo.Sites.Commerce.MatchRule.Rule;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class MatchRuleApi : IApi
    {
        public string ModelName => "MatchRule";

        public bool RequireSite => true;

        public bool RequireUser => false;

        public object CategoryDefines()
        {
            var defiles = GetConditionDefines<ProductRuleModel>();

            return new
            {
                MatchingType = Helpers.GetEnumDescription(typeof(MatchingType)),
                ConditionDefines = defiles.Select(define => new
                {
                    define.Name,
                    Display = define.Name,
                    ValueType = define.ValueType.ToString(),
                    Comparers = define.Comparers.Select(s => new
                    {
                        Name = s.ToString(),
                        Display = s.ToString()
                    })
                }),
            };
        }
    }
}
