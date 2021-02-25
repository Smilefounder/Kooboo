using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Mail.Events
{
    public class MailHandleEnd : MailSessionEvent
    {
        public MailHandleEnd(IMailSession session)
            : base(session)
        {
        }
    }
}
