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
            connection.Execute($@"
create table Category
(
	Id uniqueidentifier not null
		constraint Category_pk
			primary key,

	Name text not null,

	Parent uniqueidentifier
		constraint Category_Parent_Category_Id_fk
			references ""Category""
				on delete cascade,

	Attributes text not null,
    Specifications text not null
);

create table Product
(
	Id uniqueidentifier not null
		constraint Product_pk
			primary key,

	Title text not null,
	Images text not null,
	Description text not null,
	Attributes text not null,

	CategoryId uniqueidentifier not null
		constraint Product_CategoryId_Category_Id_fk
			references ""Category""
				on delete cascade,

	Enable int not null
);

create table Sku
(
	Id uniqueidentifier not null
		constraint Sku_pk
			primary key,

	ProductId uniqueidentifier not null
		constraint Sku_ProductId_Product_Id_fk
			references ""Product""
				on delete cascade,

	Name text,
	Specifications text not null,
	Price real not null,
	Tax real not null,
	Thumbnail text,
	Enable int not null
);

create table Stock
(
	Id uniqueidentifier not null
		constraint Stock_pk
			primary key,

	SkuId uniqueidentifier not null
		constraint Stock_SkuId_Sku_Id_fk
			references ""Sku""
				on delete cascade,

	ProductId uniqueidentifier not null
		constraint Stock_ProductId_Product_Id_fk
			references ""Product""
				on delete cascade,

	Quantity int not null,
	StockType int not null,
	DateTime int not null
);
");
        }
    }
}
