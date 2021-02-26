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
create table ProductType
(
	Id uniqueidentifier not null
		constraint ProductType_pk
			primary key,
	
	Name text not null,
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
	Specifications text not null,

	TypeId uniqueidentifier not null
		constraint Product_TypeId_ProductType_Id_fk
			references ""ProductType""
				on delete cascade,

	Enable int not null
);

create table ProductSku
(
	Id uniqueidentifier not null
		constraint ProductSku_pk
			primary key,

	ProductId uniqueidentifier not null
		constraint ProductSku_ProductId_Product_Id_fk
			references ""Product""
				on delete cascade,

	Name text,
	Specifications text not null,
	Price real not null,
	Tax real not null,
	Thumbnail text,
	Enable int not null
);

create table ProductStock
(
	SkuId uniqueidentifier not null
		constraint Stock_SkuId_Sku_Id_fk
			references ""ProductSku""
				on delete cascade,

	ProductId uniqueidentifier not null
		constraint ProductStock_ProductId_Product_Id_fk
			references ""Product""
				on delete cascade,

	Quantity int not null,
	StockType int not null,
	DateTime int not null
);

create table Category
(
	Id uniqueidentifier not null
		constraint ProductCategory_pk
			primary key,

	Type int not null,
	Name text not null,
	Rules text
);

create table ProductCategory
(
	CategoryId uniqueidentifier not null
		constraint ProductCategory_CategoryId_Category_Id_fk
			references ""Category""
				on delete cascade,

	ProductId uniqueidentifier not null
		constraint ProductCategory_ProductId_Product_Id_fk
			references ""Product""
				on delete cascade
);
");
        }
    }
}
