using FluentValidation;
using Kooboo.Sites.Commerce.Models.ProductVariant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Validators
{
    public class ProductVariantModelsValidator : AbstractValidator<ProductVariantModel[]>
    {
        public ProductVariantModelsValidator()
        {
            RuleForEach(r => r).ChildRules(item =>
            {
                item.RuleFor(r => r.Id).NotEmpty();

                item.When(w => w.Image != null, () =>
                {
                    item.RuleFor(r => r.Image).ChildRules(c =>
                    {
                        c.RuleFor(r => r.Thumbnail).MaximumLength(512);
                        c.RuleFor(r => r.Url).MaximumLength(512);
                    });
                });

                item.RuleFor(r => r.Name).MaximumLength(64);
                item.RuleFor(r => r.Price).GreaterThanOrEqualTo(0);
                item.RuleFor(r => r.ProductId).NotEmpty();
                item.RuleFor(r => r.Specifications).NotNull();

                item.RuleForEach(r => r.Specifications).ChildRules(c =>
                {
                    c.RuleFor(r => r.Key).NotEmpty();
                    c.RuleFor(r => r.Value).NotEmpty();
                });

                item.RuleFor(r => r.Tax).GreaterThanOrEqualTo(0);
            });
        }
    }
}
