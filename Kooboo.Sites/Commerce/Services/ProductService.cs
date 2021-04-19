using Dapper;
using FluentValidation;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Cache;
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
        readonly SiteCache _cache;

        public ProductService(RenderContext context) : base(context)
        {
            _cache = CommerceCache.GetCache(Context);
        }

        public void Save(ProductModel viewModel, IDbConnection connection = null)
        {
            new ProductModelValidator().ValidateAndThrow(viewModel);

            (connection ?? DbConnection).ExecuteTask(con =>
            {
                var type = new ProductTypeService(Context).Get(viewModel.TypeId, con);
                if (type == null) throw new Exception("Can not find product type");
                CheckRestrain(viewModel, type);

                if (con.Exist<Product>(viewModel.Id))
                {
                    con.Update(viewModel.ToProduct());
                }
                else
                {
                    con.Insert(viewModel.ToProduct());
                }

                _cache.ClearMatchProducts();
            }, connection == null, connection == null);
        }

        public object GetForKscript(Guid id)
        {
            return null;
            //return DbConnection.ExecuteTask(con =>
            //{
            //    //con.get
            //    return null;
            //});
        }

        private void CheckRestrain(ProductModel viewModel, Models.Type.ProductTypeModel type)
        {
            var modelStr = string.Join(string.Empty, viewModel.Specifications.Select(s => s.Id.ToString()).OrderBy(o => o));
            var typeStr = string.Join(string.Empty, type.Specifications.Select(s => s.Id.ToString()).OrderBy(o => o));
            if (modelStr != typeStr) throw new Exception("Specifications doesnot match");

            foreach (var item in viewModel.Specifications)
            {
                var typeSpecification = type.Specifications.First(f => f.Id == item.Id);
                item.Type = typeSpecification.Type;

                if (item.Type == ItemDefineModel.DefineType.Text)
                {
                    item.Value = item.Options.Select(s => s.Key).ToArray();
                }
                else
                {
                    item.Options = new KeyValuePair<Guid, string>[0];

                    if (item.Value.Except(typeSpecification.Options.Select(s => s.Key)).Any())
                    {
                        throw new Exception("Specification value doesnot match");
                    }
                }
            }
        }

        public void Deletes(Guid[] ids)
        {
            DbConnection.ExecuteTask(con => con.DeleteList<Product>(ids));
            _cache.ClearMatchProducts();
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
                    Offset = result.GetOffset()
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
            return _cache.GetMatchProducts(Context).Select(s => new KeyValuePair<Guid, string>(s.Id, s.Title)).Distinct().ToArray();
        }

        public KeyValuePair<Guid, string>[] GetByTypeId(Guid id)
        {
            return DbConnection.ExecuteTask(con =>
            {
                return con.Query<KeyValuePair<Guid, string>>("select Id as Key,Title as Value from Product where TypeId=@Id", new { Id = id }).ToArray();
            });
        }

        public KeyValuePair<Guid, string>[] GetByCatrgoryId(Guid id)
        {
            var category = _cache.GetCategories(Context).FirstOrDefault(w => w.Id == id);
            if (category == null) throw new Exception("Category not found");
            switch (category.Type)
            {
                case Category.AddingType.Manual:
                    var products = new ProductCategoryService(Context).GetByCategoryId(id);
                    return _cache.GetMatchProducts(Context).Where(w => products.Contains(w.Id)).Select(s => new KeyValuePair<Guid, string>(s.Id, s.Title)).Distinct().ToArray();
                case Category.AddingType.Auto:
                    var rule = JsonHelper.Deserialize<MatchRule.Rule>(category.Rule);
                    return _cache.GetMatchProducts(Context).Where(c => c.Match(rule)).Select(s => new KeyValuePair<Guid, string>(s.Id, s.Title)).Distinct().ToArray();
                default:
                    return null;
            }
        }
    }
}
