using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Kooboo.Sites.Commerce.Migration.Records
{
    public class Migration_1_init : IMigration
    {
        public int Version => 1;

        public void Migrate(IDbConnection connection)
        {
            connection.Execute(@"
create table Category
(
	Id uniqueidentifier not null
		constraint ProductCategory_pk
			primary key,
	Name text not null,
	Parent uniqueidentifier
);
");
        }
    }
}
