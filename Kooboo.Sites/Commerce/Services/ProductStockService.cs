using Dapper;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Services
{
    public class ProductStockService : ServiceBase
    {
        public ProductStockService(RenderContext context) : base(context)
        {
        }

        public void Adjust(Guid productId, KeyValuePair<Guid, int>[] stocks, IDbConnection connection)
        {
            var con = connection ?? DbConnection;
            IDbTransaction tran = null;

            if (connection == null)
            {
                con.Open();
                tran = con.BeginTransaction();
            }

            var addStocks = new List<ProductStock>();

            var oldStocks = con.Query<KeyValuePair<Guid, int>>(@"
SELECT PS.Id                                                   AS Key,
       CASE WHEN s.Quantity IS NULL THEN 0 ELSE s.Quantity END AS Value
FROM Product P
         LEFT JOIN ProductSku PS ON P.Id = PS.ProductId
         LEFT JOIN ProductStock S ON PS.Id = S.SkuId
WHERE P.Id = @Id
GROUP BY PS.Id
", new { Id = productId });

            foreach (var item in stocks)
            {
                if (item.Value == 0) continue;
                var oldStock = oldStocks.FirstOrDefault(f => f.Key == item.Key);
                if (oldStock.Value + item.Value < 0) throw new Exception("Inventory cannot be negative");
                addStocks.Add(new ProductStock
                {
                    DateTime = DateTime.UtcNow,
                    ProductId = productId,
                    Quantity = item.Value,
                    SkuId = item.Key,
                    Type = StockType.Adjust
                });
            }

            con.InsertList(addStocks);


            if (connection == null)
            {
                tran.Commit();
                con.Dispose();
            }
        }
    }
}
