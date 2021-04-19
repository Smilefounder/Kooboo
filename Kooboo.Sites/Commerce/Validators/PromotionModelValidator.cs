using FluentValidation;
using Kooboo.Sites.Commerce.Models.Promotion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Validators
{
    public class PromotionModelValidator : AbstractValidator<PromotionModel>
    {
        public PromotionModelValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Name).NotEmpty().Length(2, 30);
            RuleFor(r => r.Description).MaximumLength(1024);
            RuleFor(r => r.StartTime).NotEmpty();
            RuleFor(r => r.EndTime).NotEmpty();

            When(w => w.Type == Entities.Promotion.PromotionType.MoneyOff, () =>
            {
                RuleFor(r => r.Discount).GreaterThan(0);
            });

            When(w => w.Type == Entities.Promotion.PromotionType.PercentOff, () =>
            {
                RuleFor(r => r.Discount).GreaterThan(0).LessThan(100);
            });

        }
    }
}
