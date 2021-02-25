using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Mail.Events
{
    public class MailHandleEnd<TSession> : MailSessionEvent<TSession>
    {
        public MailHandleEnd(TSession session)
            : base(session)
        {
        }
    }
}
