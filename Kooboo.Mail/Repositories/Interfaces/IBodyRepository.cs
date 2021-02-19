using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Mail.Repositories
{
    public interface IBodyRepository
    {
        long Add(string body);

        string Get(long pos);
    }
}
