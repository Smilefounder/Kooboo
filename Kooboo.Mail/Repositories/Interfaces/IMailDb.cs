using System;
using System.Collections.Generic;
using System.Text;

using Kooboo.Mail.Repositories;

namespace Kooboo.Mail
{
    public interface IMailDb
    {
        Guid OrganizationId { get; }

        Guid UserId { get; }


        IFolderRepository Folders { get; }

        IMessageRepository Messages { get; }

        ITargetAddressRepository TargetAddresses { get; }

        IBodyRepository Bodies { get; }
    }
}
