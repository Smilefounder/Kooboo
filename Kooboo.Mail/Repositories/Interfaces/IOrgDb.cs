using System;
using System.Collections.Generic;
using System.Text;

using Kooboo.Mail.Repositories;

namespace Kooboo.Mail
{
    public interface IOrgDb : IDisposable
    {
        Guid OrganizationId { get; }

        IEmailAddressRepository EmailAddress { get; }
    }
}
