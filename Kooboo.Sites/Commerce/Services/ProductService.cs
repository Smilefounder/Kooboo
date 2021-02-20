using Dapper;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Kooboo.Sites.Commerce.ViewModels.ProductViewModel;

namespace Kooboo.Sites.Commerce.Services
{
    public class ProductService : ServiceBase
    {
        public ProductService(RenderContext context) : base(context)
        {
        }

        public void Save(ProductViewModel viewModel)
        {
            var stockChangedSkus = viewModel.Skus.Where(w => w.Stock != 0).ToArray();

            using (var con = DbConnection)
            {
                var product = con.QueryFirstOrDefault<ProductViewModel>("select * from Product where Id=@Id LIMIT 1", viewModel);
                var oldSkus = con.Query<Sku>("select * from Sku where ProductId=@Id", viewModel);
                var oldSkuIds = oldSkus.Select(s => s.Id).ToArray();
                var newSkus = viewModel.Skus;
                var newSkuIds = newSkus.Select(s => s.Id).ToArray();
                con.Open();
                var tran = con.BeginTransaction();

                if (product == null)
                {
                    con.Insert<Product>(viewModel);
                    con.Insert<Sku>(newSkus);
                }
                else
                {
                    con.Update<Product>(viewModel);
                    con.Insert<Sku>(newSkus.Where(w => !oldSkuIds.Contains(w.Id)));
                    con.Delete(oldSkus.Where(w => !newSkuIds.Contains(w.Id)));
                    con.Update<Sku>(newSkus.Where(w => oldSkuIds.Contains(w.Id)));
                }

                var stocks = con.Query<Stock>(
                    "select SkuId,sum(Quantity) as 'Quantity' from Stock where SkuId in @Ids group by SkuId",
                    new { Ids = stockChangedSkus.Select(s => s.Id) });

                var InsertStocks = new List<Stock>();

                foreach (var item in stockChangedSkus)
                {
                    var lastStock = stocks.FirstOrDefault(f => f.SkuId == item.Id)?.Quantity ?? 0;
                    if (lastStock + item.Stock < 0) throw new Exception("Negative inventory, please edit again");
                    InsertStocks.Add(new Stock
                    {
                        Id = Guid.NewGuid(),
                        DateTime = DateTime.UtcNow,
                        Quantity = item.Stock,
                        SkuId = item.Id,
                        ProductId = viewModel.Id,
                        StockType = StockType.Adjust
                    });
                }

                con.Insert(InsertStocks);
                tran.Commit();
            }
        }

        public ProductViewModel Query(Guid id)
        {
            using (var con = DbConnection)
            {
                var skus = con.Query<SkuViewModel>("select * from Sku where ProductId=@Id", new { Id = id });
                var product = con.QueryFirstOrDefault<ProductViewModel>("select * from Product where Id=@Id LIMIT 1", new { Id = id });
                if (!skus.Any() || product == null) throw new Exception("Not find product");

                var stocks = con.Query<Stock>(
                     "select SkuId,sum(Quantity) as 'Quantity' from Stock where SkuId in @Ids group by SkuId",
                     new { Ids = skus.Select(s => s.Id) });

                foreach (var item in skus)
                {
                    item.Stock = stocks.FirstOrDefault(f => f.SkuId == item.Id)?.Quantity ?? 0;
                }

                product.Skus = skus.ToArray();
                return product;
            }
        }


        public PagedListViewModel<ProductListViewModel> Query(PagingQueryViewModel viewModel)
        {
            var result = new PagedListViewModel<ProductListViewModel>();
            result.List = new List<ProductListViewModel>();

            using (var con = DbConnection)
            {
                var count = con.QuerySingle<long>("select count(1) from Product");
                result.SetPageInfo(viewModel, count);

                var products = con.Query<Product>("select Id,Title,Images,Enable,CategoryId from Product limit @Size offset @Offset", new
                {
                    Size = viewModel.Size,
                    Offset = result.GetOffset(viewModel.Size)
                });

                var stocks = con.Query<dynamic>(@"
                select ProductId,sum(Quantity) as Stock,sum(case when StockType = 1 or StockType=2 then Quantity else 0 end) as Sales
                from Stock
                where ProductId in @Ids
                group by ProductId
                ", new
                {
                    Ids = products.Select(s => s.Id)
                });

                foreach (var item in products)
                {
                    var product = new ProductListViewModel
                    {
                        Id = item.Id,
                        Enable = item.Enable,
                        Title = item.Title,
                        Images = item.Images,
                        CategoryId = item.CategoryId
                    };

                    var stock = stocks.FirstOrDefault(f => f.ProductId == item.Id);

                    if (stock != null)
                    {
                        product.Sales = stock.Sales;
                        product.Stock = stock.Stock;
                    }

                    result.List.Add(product);
                }

                return result;
            }
        }
    }
}
