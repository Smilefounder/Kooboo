using Dapper;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Kooboo.Sites.Commerce.ViewModels.SaveProductViewModel;

namespace Kooboo.Sites.Commerce.Services
{
    public class ProductService : ServiceBase
    {
        public ProductService(RenderContext context) : base(context)
        {
        }

        public void Save(SaveProductViewModel viewModel)
        {
            var stockChangedSkus = viewModel.Skus.Where(w => w.Stock != 0).ToArray();

            using (var con = DbConnection)
            {
                var oldSkus = con.Query<Sku>("select * from Sku where ProductId=@Id", viewModel);
                var oldSkuIds = oldSkus.Select(s => s.Id).ToArray();
                var newSkus = viewModel.Skus;
                var newSkuIds = newSkus.Select(s => s.Id).ToArray();
                con.Open();
                var tran = con.BeginTransaction();

                if (oldSkus.Any())
                {
                    con.Update<Entities.Product>(viewModel);
                    con.Insert(newSkus.Where(w => !oldSkuIds.Contains(w.Id)));
                    con.Delete(oldSkus.Where(w => !newSkuIds.Contains(w.Id)));
                    con.Update(newSkus.Where(w => oldSkuIds.Contains(w.Id)));
                }
                else
                {
                    con.Insert<Entities.Product>(viewModel);
                    con.Insert<Sku>(newSkus);
                }

                var stocks = con.Query<Stock>("select max(SkuId) as 'SkuId',sum(Quantity) as 'Quantity' from Stock where SkuId in (@Id) group by SkuId", stockChangedSkus);
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
                        StockType = StockType.Adjust
                    });
                }

                con.Insert(InsertStocks);
                tran.Commit();
            }
        }

        public SaveProductViewModel Query(Guid id)
        {
            using (var con = DbConnection)
            {
                var skus = con.Query<SkuViewModel>("select * from Sku where ProductId=@Id", new { Id = id });
                var product = con.QueryFirstOrDefault<SaveProductViewModel>("select * from Product where Id=@Id LIMIT 1", new { Id = id });
                if (!skus.Any() || product == null) throw new Exception("Not find product");
                var stocks = con.Query<Stock>("select max(SkuId) as 'SkuId',sum(Quantity) as 'Quantity' from Stock where SkuId in (@Id) group by SkuId", skus);

                foreach (var item in skus)
                {
                    item.Stock = stocks.FirstOrDefault(f => f.SkuId == item.Id)?.Quantity ?? 0;
                }

                product.Skus = skus.ToArray();
                return product;
            }
        }
    }
}
