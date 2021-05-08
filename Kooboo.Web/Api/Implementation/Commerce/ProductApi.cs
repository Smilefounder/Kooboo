using FluentValidation;
using Kooboo.Api;
using Kooboo.Sites.Commerce;
using Kooboo.Sites.Commerce.Models;
using Kooboo.Sites.Commerce.Models.Product;
using Kooboo.Sites.Commerce.Models.ProductVariant;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.Validators;
using System;
using System.Collections.Generic;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class ProductApi : CommerceApi
    {
        public override string ModelName => "Product";

        public void Post(ProductAggregateModel viewModel, ApiCall apiCall)
        {
            new ProductAggregateModelValidator().ValidateAndThrow(viewModel);
            var commerce = SiteCommerce.Get(apiCall.WebSite);

            commerce.CreateDbConnection().ExecuteTask(con =>
            {
                new ProductService(commerce).Save(viewModel.Product, con);
                new ProductVariantService(commerce).Save(viewModel.Product.Id, viewModel.ProductVariants, con);
                new ProductStockService(commerce).Adjust(viewModel.Product.Id, viewModel.Stocks, con);

                if (viewModel.Categories != null)
                {
                    new ProductCategoryService(commerce).SaveByProductId(viewModel.Categories, viewModel.Product.Id, con);
                }
            }, true);
        }

        public ProductEditModel Get(Guid id, ApiCall apiCall)
        {
            return GetService<ProductService>(apiCall).Get(id);
        }

        public PagedListModel<ProductListModel> List(ProductQueryModel viewModel, ApiCall apiCall)
        {
            return GetService<ProductService>(apiCall).Query(viewModel);
        }

        public KeyValuePair<Guid, string>[] KeyValue(ApiCall apiCall)
        {
            var commerce = SiteCommerce.Get(apiCall.WebSite);
            var service = new ProductService(commerce);

            var typeId = apiCall.GetGuidValue("typeId");

            if (typeId != default(Guid))
            {
                return service.GetByTypeId(typeId);
            }

            var categoryId = apiCall.GetGuidValue("categoryId");

            if (categoryId != default(Guid))
            {
                return service.GetByCatrgoryId(categoryId);
            }

            return new ProductService(commerce).KeyValue();
        }

        public void Delete(Guid[] ids, ApiCall apiCall)
        {
            GetService<ProductService>(apiCall).Deletes(ids);
        }

        public ProductVariantDetailModel[] SkuList(ApiCall apiCall, Guid id)
        {
            return GetService<ProductVariantService>(apiCall).List(id);
        }
    }
}
