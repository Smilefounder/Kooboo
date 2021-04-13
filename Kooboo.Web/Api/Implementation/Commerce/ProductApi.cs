using Kooboo.Api;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.Models;
using Kooboo.Sites.Commerce.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Sites.Commerce;
using Kooboo.Sites.Commerce.Validators;
using FluentValidation;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class ProductApi : IApi
    {
        public string ModelName => "Product";

        public bool RequireSite => true;

        public bool RequireUser => true;

        public void Post(ProductAggregateModel viewModel, ApiCall apiCall)
        {
            new ProductAggregateModelValidator().ValidateAndThrow(viewModel);

            using (var con = apiCall.Context.CreateCommerceDbConnection())
            {
                con.Open();
                var tran = con.BeginTransaction();
                new ProductService(apiCall.Context).Save(viewModel.Product, con);
                new ProductSkuService(apiCall.Context).Save(viewModel.Product.Id, viewModel.Skus, con);
                new ProductStockService(apiCall.Context).Adjust(viewModel.Product.Id, viewModel.Stocks, con);
                tran.Commit();
            }
        }

        public ProductEditModel Get(Guid id, ApiCall apiCall)
        {
            return new ProductService(apiCall.Context).Query(id);
        }

        public PagedListModel<ProductListModel> List(PagingQueryModel viewModel, ApiCall apiCall)
        {
            return new ProductService(apiCall.Context).Query(viewModel);
        }

        public KeyValuePair<Guid, string>[] KeyValue(ApiCall apiCall)
        {
            return new ProductService(apiCall.Context).KeyValue();
        }

        public void Delete(Guid[] ids, ApiCall apiCall)
        {
            new ProductService(apiCall.Context).Deletes(ids);
        }

        //public SkuModel[] SkuList(ApiCall apiCall, Guid id)
        //{
        //    return new ProductService(apiCall.Context).SkuList(id);
        //}
    }
}
