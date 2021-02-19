using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Mail.Repositories
{
    public interface IDbFactory
    {
        IOrgDb CreateOrgDb(Guid orgId);

        IMailDb CreateMailDb(Guid userId, Guid orgId);
    }
}
