using System;
using System.Collections.Generic;
using System.Text;

using Kooboo.Mail.Repositories;

namespace Kooboo.Mail
{
    public abstract class MailDb : IDisposable
    {
        protected MailDb(Guid userId, Guid orgId)
        {
            UserId = userId;
            OrganizationId = orgId;
        }

        public Guid OrganizationId { get; }

        public Guid UserId { get; }


        public abstract IFolderRepository Folders { get; }

        public abstract IMessageRepository Messages { get; }

        public abstract ITargetAddressRepository TargetAddresses { get; }

        public abstract IBodyRepository MsgBody { get; }

        public virtual void Dispose()
        {
        }
    }
}
