using Dapper;
using FluentValidation;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models;
using Kooboo.Sites.Commerce.Models.Product;
using Kooboo.Sites.Commerce.Models.Sku;
using Kooboo.Sites.Commerce.Validators;
using System;
using System.Collections.Generic;
using System.Data;
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

        public void Save(ProductModel viewModel, IDbConnection connection = null)
        {
            new ProductModelValidator().ValidateAndThrow(viewModel);
            var con = connection ?? DbConnection;
            var type = new ProductTypeService(Context).Get(viewModel.TypeId, con);
            if (type == null) throw new Exception("Can not find product type");
            //CheckRestrain(viewModel, type);

            if (con.Exist<Product>(viewModel.Id))
            {
                con.Update(viewModel.ToProduct());
            }
            else
            {
                con.Insert(viewModel.ToProduct());
            }

            if (connection == null) con.Dispose();
            _matchList = null;
        }

        private void CheckRestrain(ProductModel viewModel, Models.Type.ProductTypeDetailModel type)
        {
            throw new NotImplementedException();
        }

        public void Deletes(Guid[] ids)
        {
            using (var con = DbConnection)
            {
                con.DeleteList<Product>(ids);
            }
        }

        public ProductEditModel Query(Guid id)
        {
            using (var con = DbConnection)
            {
                var entity = con.Get<Product>(id);

                var model = new ProductEditModel(entity)
                {
                    Skus = new ProductSkuService(Context).List(id, con)
                };

                return model;
            }
        }

        public PagedListModel<ProductListModel> Query(PagingQueryModel viewModel)
        {
            var result = new PagedListModel<ProductListModel>
            {
                List = new List<ProductListModel>()
            };

            using (var con = DbConnection)
            {
                var count = con.Count<Product>();
                result.SetPageInfo(viewModel, count);

                var products = con.Query(@"
SELECT P.Id,
       P.TypeId,
       P.Images,
       P.Title,
       P.Enable,
       p.TypeId,
       PT.Name,
       P.Specifications  AS ProductSpecifications,
       PT.Specifications AS ProductTypeSpecifications
FROM Product P
         LEFT JOIN ProductType PT ON PT.Id = P.TypeId
ORDER BY CreateTime DESC
LIMIT @Size OFFSET @Offset
", new
                {
                    Size = viewModel.Size,
                    Offset = result.GetOffset(viewModel.Size)
                });

                var skus = con.Query(@"
SELECT PS.Id,
       PS.Name,
       Ps.Specifications,
       SUM(CASE WHEN P.Quantity IS NULL THEN 0 ELSE P.Quantity END)       AS Stock,
       SUM(CASE WHEN P.Type = 1 OR p.Type = 2 THEN p.Quantity ELSE 0 END) AS Sale,
       PS.Enable,
       PS.Image,
       PS.ProductId,
       PS.Price
FROM ProductSku PS
         LEFT JOIN ProductStock P ON PS.Id = P.SkuId
WHERE PS.ProductId IN @Ids
GROUP BY PS.Id
                ", new
                {
                    Ids = products.Select(s => s.Id)
                });

                foreach (var item in products)
                {
                    var product = new ProductListModel
                    {
                        Id = item.Id,
                        Enable = Convert.ToBoolean(item.Enable),
                        Title = item.Title,
                        Images = JsonHelper.Deserialize<ProductModel.Image[]>(item.Images),
                        TypeId = item.TypeId,
                        TypeName = item.Name
                    };

                    var productSpecifications = JsonHelper.Deserialize<ProductModel.Specification[]>(item.ProductSpecifications);
                    var productTypeSpecifications = JsonHelper.Deserialize<ItemDefineModel[]>(item.ProductTypeSpecifications);

                    product.Items = skus.Where(w => w.ProductId == item.Id).Select(s =>
                    {
                        var specifications = JsonHelper.Deserialize<KeyValuePair<Guid, Guid>[]>(s.Specifications);

                        return new ProductListModel.Item
                        {
                            Enable = Convert.ToBoolean(s.Enable),
                            Id = s.Id,
                            Image = s.Image == null ? null : JsonHelper.Deserialize<ProductModel.Image>(s.Image),
                            Name = s.Name,
                            Sale = (int)s.Sale,
                            Specifications = Helpers.GetSpecifications(productTypeSpecifications, productSpecifications, specifications),
                            Stock = (int)s.Stock,
                            Price = (decimal)s.Price
                        };
                    }).ToArray();

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


        public SkuModel[] SkuList(Guid productId)
        {
            var result = new List<SkuModel>();

            using (var con = DbConnection)
            {
                var list = con.Query(@"
SELECT P.Id              AS ProductId,
       p.Title           AS ProductName,
       PS.Id             AS SkuId,
       P.Specifications  AS ProductSpecifications,
       PS.Specifications AS ProductSkuSpecifications,
       PT.Specifications AS ProductTypeSpecifications,
       SUM(S.Quantity)   AS Stock
FROM Product P
         LEFT OUTER JOIN ProductType PT ON PT.Id = P.TypeId
         LEFT JOIN ProductSku PS ON P.Id = PS.ProductId
         LEFT JOIN ProductStock S ON PS.Id = S.SkuId
WHERE P.Id = @ProductId
GROUP BY Ps.Id
", new { ProductId = productId });

                ItemDefineModel[] productTypeSpecifications = null;
                KeyValuePair<Guid, string>[] productSpecifications = null;

                foreach (var item in list)
                {
                    if (productTypeSpecifications == null)
                    {
                        productTypeSpecifications = JsonHelper.Deserialize<ItemDefineModel[]>(item.ProductTypeSpecifications);
                    }

                    if (productSpecifications == null)
                    {
                        productSpecifications = JsonHelper.Deserialize<KeyValuePair<Guid, string>[]>(item.ProductSpecifications);
                    }

                    var productSkuSpecifications = JsonHelper.Deserialize<KeyValuePair<Guid, Guid>[]>(item.ProductSkuSpecifications);

                    result.Add(new SkuModel
                    {
                        Id = item.SkuId,
                        ProductId = item.ProductId,
                        //ProductName = item.ProductName,
                        //Stock = Convert.ToInt32(item.Stock),
                        //Specifications = Helpers.GetSpecifications(productTypeSpecifications, productSpecifications, productSkuSpecifications)
                    });
                }
            }

            return result.ToArray();
        }
    }
}
