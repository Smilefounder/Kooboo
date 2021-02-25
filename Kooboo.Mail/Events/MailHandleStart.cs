using System;
using System.Collections.Generic;
using System.Text;

using Kooboo.Data.Events;

namespace Kooboo.Mail.Events
{
    public class MailHandleStart : MailSessionEvent
    {
        public MailHandleStart(IMailSession session)
            : base(session)
        {
        }
    }
}
