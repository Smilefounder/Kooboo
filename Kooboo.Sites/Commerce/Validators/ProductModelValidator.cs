using FluentValidation;
using Kooboo.Sites.Commerce.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Validators
{
    public class ProductModelValidator : AbstractValidator<ProductModel>
    {
        public ProductModelValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Title).NotEmpty().Length(2, 256);
            RuleFor(r => r.Description).NotNull();
            RuleFor(r => r.TypeId).NotEmpty();
            RuleFor(r => r.Images).NotNull();
            RuleFor(r => r.Attributes).NotNull();
            RuleFor(r => r.Specifications).NotNull();

            RuleForEach(r => r.Images).ChildRules(image =>
            {
                image.RuleFor(r => r.Id).NotEmpty();
                image.RuleFor(r => r.Thumbnail).NotEmpty();
                image.RuleFor(r => r.Url).NotEmpty();
            });

            RuleForEach(r => r.Attributes).ChildRules(c =>
            {
                c.RuleFor(r => r.Key).NotEmpty();
                c.RuleFor(r => r.Value).NotNull();
            });

            RuleForEach(r => r.Specifications).ChildRules(c =>
            {
                c.RuleFor(r => r.Id).NotEmpty();
                c.RuleFor(r => r.Value).NotEmpty();
                c.When(w => w.Options != null, () =>
                {
                    c.RuleForEach(r => r.Options).ChildRules(kv =>
                    {
                        kv.RuleFor(r => r.Key).NotEmpty();
                        kv.RuleFor(r => r.Value).NotEmpty().Length(1, 30);
                    });
                });
            });

        }
    }
}
