using Kooboo.Data.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Mail.Events
{
    public abstract class MailSessionEvent : IEvent
    {
        public MailSessionEvent(IMailSession session)
        {
            Session = session;
        }

        public IMailSession Session { get; }
    }
}
