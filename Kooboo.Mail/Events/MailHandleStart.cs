using System;
using System.Collections.Generic;
using System.Text;

using Kooboo.Data.Events;

namespace Kooboo.Mail.Events
{
    public class MailHandleStart<TSession> : MailSessionEvent<TSession>
    {
        public MailHandleStart(TSession session)
            : base(session)
        {
        }
    }
}
