using Dapper;
using FluentValidation;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Cache;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models;
using Kooboo.Sites.Commerce.Models.Promotion;
using Kooboo.Sites.Commerce.Validators;
using System;
using System.Linq;

namespace Kooboo.Sites.Commerce.Services
{
    public class PromotionService : ServiceBase
    {

        public PromotionService(RenderContext context) : base(context)
        {
        }

        public PromotionModel Get(Guid id)
        {
            return DbConnection.ExecuteTask(con =>
            {
                var entity = con.Get<Promotion>(id);
                if (entity == null) throw new Exception("Not found promotion");
                return new PromotionModel(entity);
            });
        }

        public PagedListModel<PromotionListModel> List(PagingQueryModel model)
        {
            var result = new PagedListModel<PromotionListModel>();

            return DbConnection.ExecuteTask(con =>
            {
                var count = con.Count<Promotion>();
                result.SetPageInfo(model, count);

                result.List = con.Query<PromotionListModel>(@"
SELECT id,
       name,
       type,
       priority,
       exclusive,
       discount,
       target,
       starttime,
       endtime,
       enable
FROM Promotion
LIMIT @Limit OFFSET @Offset
", new
                {
                    Limit = model.Size,
                    Offset = result.GetOffset()
                }).ToList();

                return result;
            });
        }

        public void Save(PromotionModel model)
        {
            new PromotionModelValidator().ValidateAndThrow(model);

            DbConnection.ExecuteTask(con =>
            {
                var entity = model.ToPromotion();

                if (con.Exist<Promotion>(entity.Id))
                {
                    con.Update(entity);
                }
                else
                {
                    con.Insert(entity);
                }

                CommerceCache.GetCache(Context).ClearPromotions();
            });
        }

        public void Deletes(Guid[] ids)
        {
            DbConnection.ExecuteTask(c => c.DeleteList<Promotion>(ids));
            CommerceCache.GetCache(Context).ClearPromotions();
        }
    }
}
