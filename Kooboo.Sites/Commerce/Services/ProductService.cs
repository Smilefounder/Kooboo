using Dapper;
using FluentValidation;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models;
using Kooboo.Sites.Commerce.Models.Product;
using Kooboo.Sites.Commerce.Validators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
            DbConnection.ExecuteTask(con =>
            {
                _matchList = con.Query<MatchRule.TargetModels.Product>("select pro.Id,sku.Price,pro.Title,pro.TypeId from ProductSku sku left join Product pro on sku.ProductId=pro.Id").ToArray();
            });
        }

        public ProductService(RenderContext context) : base(context)
        {
        }

        public void Save(ProductModel viewModel, IDbConnection connection = null)
        {
            new ProductModelValidator().ValidateAndThrow(viewModel);

            (connection ?? DbConnection).ExecuteTask(con =>
            {
                var type = new ProductTypeService(Context).Get(viewModel.TypeId, con);
                if (type == null) throw new Exception("Can not find product type");
                //TODO CheckRestrain(viewModel, type);

                if (con.Exist<Product>(viewModel.Id))
                {
                    con.Update(viewModel.ToProduct());
                }
                else
                {
                    con.Insert(viewModel.ToProduct());
                }

                _matchList = null;
            }, connection == null, connection == null);
        }

        private void CheckRestrain(ProductModel viewModel, Models.Type.ProductTypeDetailModel type)
        {
            throw new NotImplementedException();
        }

        public void Deletes(Guid[] ids)
        {
            DbConnection.ExecuteTask(con => con.DeleteList<Product>(ids));
        }

        public ProductEditModel Get(Guid id)
        {
            return DbConnection.ExecuteTask(con =>
            {
                var entity = con.Get<Product>(id);

                return new ProductEditModel(entity)
                {
                    Skus = new ProductSkuService(Context).List(id, con),
                    Categories = new ProductCategoryService(Context).GetByProductId(id)
                };
            });
        }

        public PagedListModel<ProductListModel> Query(PagingQueryModel viewModel)
        {
            var result = new PagedListModel<ProductListModel>
            {
                List = new List<ProductListModel>()
            };

            DbConnection.ExecuteTask((con) =>
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
            });

            return result;
        }

        public KeyValuePair<Guid, string>[] KeyValue()
        {
            return DbConnection.ExecuteTask(c => c.Query<KeyValuePair<Guid, string>>("select Id as Key,title as value from Product").ToArray());
        }
    }
}
