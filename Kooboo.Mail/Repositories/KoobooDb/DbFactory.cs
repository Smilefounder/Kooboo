using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Mail.Repositories.KoobooDb
{
    public class DbFactory : IDbFactory
    {
        public MailDb CreateMailDb(Guid userId, Guid orgId)
        {
            return new MailDbImpl(userId, orgId);
        }

        public OrgDb CreateOrgDb(Guid orgId)
        {
            return new OrgDbImpl(orgId);
        }
    }
}
