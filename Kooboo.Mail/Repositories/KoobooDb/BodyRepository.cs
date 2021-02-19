using Kooboo.IndexedDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Mail.Repositories.KoobooDb
{
    public class BodyRepository : IBodyRepository
    {
        private readonly object _lock = new object();
        private readonly MailDb _db;

        public BodyRepository(MailDb db)
        {
            _db = db;
        }

        private Sequence<string> _msgbody;
        public Kooboo.IndexedDB.Sequence<string> MsgBody
        {
            get
            {
                if (_msgbody == null)
                {
                    lock (_lock)
                    {
                        _msgbody = (_db as MailDbImpl).Db.GetSequenceOld<string>("MessageSource");
                    }
                }
                return _msgbody;
            }
        }

        public long Add(string body)
        {
            var pos = MsgBody.Add(body);
            MsgBody.Close();
            return pos;
        }

        public string Get(long pos)
        {
            return MsgBody.Get(pos);
        }
    }
}
