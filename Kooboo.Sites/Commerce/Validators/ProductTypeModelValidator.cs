using FluentValidation;
using Kooboo.Sites.Commerce.Models;
using Kooboo.Sites.Commerce.Models.Type;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Validators
{
    public class ProductTypeModelValidator : AbstractValidator<ProductTypeModel>
    {
        public ProductTypeModelValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Name).NotEmpty().Length(2, 30);
            RuleFor(r => r.Attributes).NotNull();
            RuleFor(r => r.Specifications).NotNull();
            RuleForEach(r => r.Attributes).ChildRules(ValidItemDefine());
            RuleForEach(r => r.Specifications).ChildRules(ValidItemDefine());
        }

        private static Action<InlineValidator<ItemDefineModel>> ValidItemDefine()
        {
            return itemDefine =>
            {
                itemDefine.RuleFor(r => r.Id).NotEmpty();
                itemDefine.RuleFor(r => r.Name).NotEmpty().Length(2, 30);
                itemDefine.When(w => w.Type == Models.ItemDefineModel.DefineType.Option, () =>
                {
                    itemDefine.RuleForEach(define => define.Options)
                       .ChildRules(vm =>
                       {
                           vm.RuleFor(r => r.Key).NotEmpty();
                           vm.RuleFor(r => r.Value).NotEmpty().Length(1, 30);
                       });
                });
            };
        }
    }
}
