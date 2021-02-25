using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Mail.Events
{
    public class MailHandleScope<TSession> : IDisposable
    {
        private readonly TSession _session;

        public MailHandleScope(TSession session)
        {
            _session = session;

            Kooboo.Data.Events.EventBus.Raise(new Events.MailHandleStart<TSession>(_session));
        }

        public void Dispose()
        {
            Kooboo.Data.Events.EventBus.Raise(new Events.MailHandleEnd<TSession>(_session));
        }
    }
}
