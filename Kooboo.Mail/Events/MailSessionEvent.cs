using Kooboo.Data.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Mail.Events
{
    public abstract class MailSessionEvent<TSession> : IEvent
    {
        public MailSessionEvent(TSession session)
        {
            Session = session;
        }

        public TSession Session { get; }
    }
}
