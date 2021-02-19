using System;
using System.Collections.Generic;
using System.Text;

using Kooboo.Mail.Repositories;

namespace Kooboo.Mail
{
    public abstract class OrgDb : IDisposable
    {
        protected OrgDb(Guid orgId)
        {
            OrganizationId = orgId;
        }

        public Guid OrganizationId { get; }

        public abstract IEmailAddressRepository EmailAddress { get; }

        public virtual void Dispose()
        {
        }
    }
}
