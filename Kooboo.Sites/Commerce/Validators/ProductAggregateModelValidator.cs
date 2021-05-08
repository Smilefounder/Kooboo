using FluentValidation;
using Kooboo.Sites.Commerce.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Validators
{
    public class ProductAggregateModelValidator : AbstractValidator<ProductAggregateModel>
    {
        public ProductAggregateModelValidator()
        {
            RuleFor(r => r.Product).NotNull();
            RuleFor(r => r.ProductVariants).NotEmpty();
            RuleFor(r => r.Stocks).NotEmpty();
        }
    }
}
