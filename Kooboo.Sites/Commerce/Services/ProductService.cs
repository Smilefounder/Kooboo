using Dapper;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.ViewModels;
using Kooboo.Sites.Commerce.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Services
{
    public class ProductService : ServiceBase
    {
        static MatchRule.TargetModels.Product[] _matchList;
        readonly static object _locker = new object();

        public IEnumerable<MatchRule.TargetModels.Product> MatchList
        {
            get
            {
                lock (_locker)
                {
                    if (_matchList == null)
                    {
                        lock (_locker)
                        {
                            GetMatchList();
                        }
                    }
                }

                return _matchList;
            }
        }

        private void GetMatchList()
        {
            using (var con = DbConnection)
            {
                _matchList = con.Query<MatchRule.TargetModels.Product>("select pro.Id,sku.Price,pro.Title,pro.TypeId from ProductSku sku left join Product pro on sku.ProductId=pro.Id").ToArray();
            }
        }

        public ProductService(RenderContext context) : base(context)
        {
        }

        public void Save(ProductViewModel viewModel)
        {
            var stockChangedSkus = viewModel.Skus.Where(w => w.Stock != 0).ToArray();

            using (var con = DbConnection)
            {
                var oldSkuIds = con.Query<Guid>("select Id from ProductSku where ProductId=@Id", viewModel);
                var newSkus = viewModel.Skus.Select(s => s.ToSku());
                var newSkuIds = newSkus.Select(s => s.Id).ToArray();
                con.Open();
                var tran = con.BeginTransaction();

                if (!con.Exist<Product>(viewModel.Id))
                {
                    con.Insert(viewModel.ToProduct());
                    con.InsertList(newSkus);
                }
                else
                {
                    con.Update(viewModel.ToProduct());
                    con.InsertList(newSkus.Where(w => !oldSkuIds.Contains(w.Id)));
                    con.DeleteList<ProductSku>(oldSkuIds.Where(w => !newSkuIds.Contains(w)));
                    con.UpdateList(newSkus.Where(w => oldSkuIds.Contains(w.Id)));
                }

                var stocks = con.Query<ProductStock>(
                    "select SkuId,sum(Quantity) as 'Quantity' from ProductStock where SkuId in @Ids group by SkuId",
                    new { Ids = stockChangedSkus.Select(s => s.Id) });

                var InsertStocks = new List<ProductStock>();

                foreach (var item in stockChangedSkus)
                {
                    var lastStock = stocks.FirstOrDefault(f => f.SkuId == item.Id)?.Quantity ?? 0;
                    if (lastStock + item.Stock < 0) throw new Exception("Negative inventory, please edit again");
                    InsertStocks.Add(new ProductStock
                    {
                        DateTime = DateTime.UtcNow,
                        Quantity = item.Stock,
                        SkuId = item.Id,
                        ProductId = viewModel.Id,
                        StockType = StockType.Adjust
                    });
                }

                con.InsertList(InsertStocks);
                tran.Commit();
            }
            _matchList = null;
        }

        public ProductViewModel Query(Guid id)
        {
            using (var con = DbConnection)
            {
                var skus = con.Query<ProductSku>("select * from ProductSku where ProductId=@Id", new { Id = id });
                var product = con.QueryFirstOrDefault<Product>("select * from Product where Id=@Id LIMIT 1", new { Id = id });
                if (!skus.Any() || product == null) throw new Exception("Not find product");

                var stocks = con.Query<dynamic>(
                     "select SkuId,sum(Quantity) as 'Quantity' from ProductStock where SkuId in @Ids group by SkuId",
                     new { Ids = skus.Select(s => s.Id) });

                var skuViewModels = new List<ProductViewModel.Sku>();

                foreach (var item in skus)
                {
                    var stock = stocks.FirstOrDefault(f => f.SkuId == item.Id)?.Quantity ?? 0;
                    skuViewModels.Add(new ProductViewModel.Sku(item, (int)stock));
                }

                return new ProductViewModel(product, skuViewModels.ToArray(), new Guid[0]);
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

                var products = con.Query<Product>("select Id,Title,Images,Enable,TypeId from Product limit @Size offset @Offset", new
                {
                    Size = viewModel.Size,
                    Offset = result.GetOffset(viewModel.Size)
                });

                var stocks = con.Query<dynamic>(@"
                select ProductId,sum(Quantity) as Stock,sum(case when StockType = 1 or StockType=2 then Quantity else 0 end) as Sales
                from ProductStock
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
                        CategoryId = item.TypeId
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

        public KeyValuePair<Guid, string>[] KeyValue()
        {
            using (var con = DbConnection)
            {
                return con.Query<KeyValuePair<Guid, string>>("select Id as Key,title as value from Product").ToArray();
            }
        }
    }
}
