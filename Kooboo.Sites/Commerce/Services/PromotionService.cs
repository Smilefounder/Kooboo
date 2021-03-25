using Dapper;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.ViewModels.Promotion;
using System;
using System.Collections.Generic;
using System.Text;
using static Kooboo.Sites.Commerce.Entities.Promotion;

namespace Kooboo.Sites.Commerce.Services
{
    public class PromotionService : ServiceBase
    {
        public PromotionService(RenderContext context) : base(context)
        {

        }


        public PromotionViewModel Get(Guid id)
        {
            using (var con = DbConnection)
            {
                var entity = con.Get<Promotion>(id);
                return new PromotionViewModel(entity);
            }
        }

        public PromotionListViewModel[] List()
        {
            var result = new List<PromotionListViewModel>();
            using (var con = DbConnection)
            {
                var list = con.Query("SELECT Id,Name,StartTime,EndTime,Exclusive,Priority,Target,Discount,Type FROM Promotion");

                foreach (var item in list)
                {
                    var startTime = ((DateTime)item.StartTime).ToUniversalTime();
                    var endTime = ((DateTime)item.EndTime).ToUniversalTime();
                    result.Add(new PromotionListViewModel
                    {
                        Active = DateTime.UtcNow >= startTime && DateTime.UtcNow <= endTime,
                        Exclusive = Convert.ToBoolean(item.Exclusive),
                        Id = item.Id,
                        Name = item.Name,
                        Priority = item.Priority,
                        Target = (PromotionTarget)item.Target,
                        Discount = item.Type == 0 ? $"-{item.Discount}" : $"{item.Discount}%"
                    }); ;
                }
            }

            return result.ToArray();
        }

        public void Save(PromotionViewModel model)
        {
            var entity = model.ToPromotion();

            using (var con = DbConnection)
            {
                if (con.Exist<Promotion>(entity.Id))
                {
                    con.Update(entity);
                }
                else
                {
                    con.Insert(entity);
                }
            }
        }

        public void Deletes(Guid[] ids)
        {
            using (var con = DbConnection)
            {
                con.DeleteList<Promotion>(ids);
            }
        }
    }
}
