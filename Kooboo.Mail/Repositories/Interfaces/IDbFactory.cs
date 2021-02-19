using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Mail.Repositories
{
    public interface IDbFactory
    {
        OrgDb CreateOrgDb(Guid orgId);

        MailDb CreateMailDb(Guid userId, Guid orgId);
    }
}
