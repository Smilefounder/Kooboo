using Kooboo.Data.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Mail.Events
{
    public class MailRecevied : IEvent
    {
        public Message Message { get; set; }

        public string Body { get; set; }
    }
}
