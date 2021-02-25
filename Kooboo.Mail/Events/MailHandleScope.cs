using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Mail.Events
{
    public class MailHandleScope : IDisposable
    {
        private readonly IMailSession _session;

        public MailHandleScope(IMailSession session)
        {
            _session = session;

            Kooboo.Data.Events.EventBus.Raise(new Events.MailHandleStart(_session));
        }

        public void Dispose()
        {
            Kooboo.Data.Events.EventBus.Raise(new Events.MailHandleEnd(_session));
        }
    }
}
