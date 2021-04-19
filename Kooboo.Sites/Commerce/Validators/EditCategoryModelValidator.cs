using FluentValidation;
using Kooboo.Sites.Commerce.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Validators
{
    public class EditCategoryModelValidator : AbstractValidator<EditCategoryModel>
    {
        public EditCategoryModelValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Name).NotEmpty().Length(2, 30);


            When(w => w.Type == Entities.Category.AddingType.Auto, () =>
            {
                RuleFor(r => r.Rule).NotNull();
            });

            When(w => w.Type == Entities.Category.AddingType.Manual, () =>
            {
                RuleFor(r => r.Products).NotNull().ForEach(f => f.NotEmpty());
            });
        }
    }
}
