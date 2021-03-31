using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Product
{
    public class EditProductTypeModel : ProductTypeModel
    {
        public EditProductTypeModel(ProductTypeModel viewModel, bool hasDependent)
        {
            Id = viewModel.Id;
            Name = viewModel.Name;
            Attributes = viewModel.Attributes;
            Specifications = viewModel.Specifications;
            HasDependent = hasDependent;
        }
        public bool HasDependent { get; set; }
    }
}
