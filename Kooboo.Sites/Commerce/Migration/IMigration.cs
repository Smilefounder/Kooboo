using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Kooboo.Sites.Commerce.Migration
{
    public interface IMigration
    {
        int Version { get; }

        void Migrate(IDbConnection connection);
    }
}
