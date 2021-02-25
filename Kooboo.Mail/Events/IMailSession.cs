using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Mail
{
    public interface IMailSession
    {
        Dictionary<string, object> Items { get; }
    }
}
