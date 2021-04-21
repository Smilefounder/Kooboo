using FluentValidation;
using Kooboo.Sites.Commerce.Models.Customer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Validators
{
    public class CreateCustomerModelValidator : AbstractValidator<CreateCustomerModel>
    {
        public CreateCustomerModelValidator()
        {
            RuleFor(r => r.UserName).NotEmpty().Length(2, 30);
            RuleFor(r => r.Password).NotEmpty().Length(2, 30);

            When(w => !string.IsNullOrWhiteSpace(w.Email), () =>
             {
                 RuleFor(r => r.Email).EmailAddress();
             });

            When(w => !string.IsNullOrWhiteSpace(w.Phone), () =>
            {
                RuleFor(r => r.Phone).Length(11, 20);
            });
        }
    }
}
