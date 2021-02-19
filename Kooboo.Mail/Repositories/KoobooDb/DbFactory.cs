using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Mail.Repositories.KoobooDb
{
    public class DbFactory : IDbFactory
    {
        public IMailDb CreateMailDb(Guid userId, Guid orgId)
        {
            return new MailDb(userId, orgId);
        }

        public IOrgDb CreateOrgDb(Guid orgId)
        {
            return new OrgDb(orgId);
        }
    }
}
