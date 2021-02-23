using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.ViewModels.Product
{
    public class EditProductTypeViewModel : ProductTypeViewModel
    {
        public EditProductTypeViewModel(ProductTypeViewModel viewModel, bool hasDependent)
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
