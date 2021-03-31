using Dapper;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Kooboo.Sites.Commerce.Entities.Promotion;
using static Kooboo.Sites.Commerce.Models.Promotion.PromotionModel;

namespace Kooboo.Sites.Commerce.Services
{
    public class PromotionService : ServiceBase
    {
        static PromotionMatchModel[] _matchList = null;
        readonly static object _locker = new object();

        public PromotionService(RenderContext context) : base(context)
        {

        }

        public IEnumerable<PromotionMatchModel> MatchList
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

                return _matchList.Where(w => w.StartTime <= DateTime.UtcNow);
            }
        }

        private void GetMatchList()
        {
            using (var con = DbConnection)
            {
                var list = con.Query<Promotion>(@"
SELECT Id,
       Name,
       Type,
       Priority,
       Exclusive,
       Discount,
       Rules,
       Target,
       StartTime
FROM Promotion
WHERE CURRENT_TIMESTAMP <= DATETIME(Promotion.EndTime)
");
                _matchList = list.Select(s => new PromotionMatchModel
                {
                    Discount = s.Discount,
                    Exclusive = s.Exclusive,
                    Id = s.Id,
                    Name = s.Name,
                    Priority = s.Priority,
                    Rules = JsonHelper.Deserialize<PromotionRules>(s.Rules),
                    StartTime = s.StartTime.ToUniversalTime(),
                    Target = s.Target,
                    Type = s.Type
                }).OrderByDescending(o => o.Exclusive).ThenByDescending(t => t.Priority).ToArray();
            }
        }

        public PromotionModel Get(Guid id)
        {
            using (var con = DbConnection)
            {
                var entity = con.Get<Promotion>(id);
                return new PromotionModel(entity);
            }
        }

        public PromotionListModel[] List()
        {
            var result = new List<PromotionListModel>();
            using (var con = DbConnection)
            {
                var list = con.Query("SELECT Id,Name,StartTime,EndTime,Exclusive,Priority,Target,Discount,Type FROM Promotion");

                foreach (var item in list)
                {
                    var startTime = ((DateTime)item.StartTime).ToUniversalTime();
                    var endTime = ((DateTime)item.EndTime).ToUniversalTime();
                    result.Add(new PromotionListModel
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

        public void Save(PromotionModel model)
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

            _matchList = null;
        }

        public void Deletes(Guid[] ids)
        {
            using (var con = DbConnection)
            {
                con.DeleteList<Promotion>(ids);
            }

            _matchList = null;
        }
    }
}
