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
create table 'ProductType'
(
	'Id' uniqueidentifier not null
		constraint ProductType_pk
			primary key,
	
	'Name' text not null,
	'Attributes' text not null,
    'Specifications' text not null
);

create table 'Product'
(
	'Id' uniqueidentifier not null
		constraint Product_pk
			primary key,

	'Title' text not null,
	'Images' text not null,
	'Description' text not null,
	'Attributes' text not null,
	'Specifications' text not null,
	'CreateTime' datetime not null,

	'TypeId' uniqueidentifier not null
		constraint Product_TypeId_ProductType_Id_fk
			references 'ProductType'
				on delete cascade,

	'Enable' bool not null
);

create table 'ProductSku'
(
	'Id' uniqueidentifier not null
		constraint ProductSku_pk
			primary key,

	'ProductId' uniqueidentifier not null
		constraint ProductSku_ProductId_Product_Id_fk
			references 'Product'
				on delete cascade,

	'Name' text,
	'Specifications' text not null,
	'Price' real not null,
	'Tax' real not null,
	'Image' text,
	'Enable' bool not null
);

create table 'ProductStock'
(
	'SkuId' uniqueidentifier not null
		constraint Stock_SkuId_Sku_Id_fk
			references 'ProductSku'
				on delete cascade,

	'ProductId' uniqueidentifier not null
		constraint ProductStock_ProductId_Product_Id_fk
			references 'Product'
				on delete cascade,

	'Quantity' int not null,
	'Type' int not null,
	'DateTime' datetime not null,
	'OrderItemId' uniqueidentifier
);

create table 'Category'
(
	Id uniqueidentifier not null
		constraint ProductCategory_pk
			primary key,

	Type int not null,
	Name text not null,
	CreateTime datetime not null,
	Rule text,
	Enable bool not null
);

create table 'ProductCategory'
(
	'CategoryId' uniqueidentifier not null
		constraint ProductCategory_CategoryId_Category_Id_fk
			references 'Category'
				on delete cascade,

	'ProductId' uniqueidentifier not null
		constraint ProductCategory_ProductId_Product_Id_fk
			references 'Product'
				on delete cascade
);

create table 'Promotion'
(
	'Id' uniqueidentifier not null
		constraint Promotion_pk
			primary key,

	'Name' Text not null,
	'Description' Text,
	'Type' int not null,
	'Priority' int not null,
	'Exclusive' bool not null,
	'Discount' real not null,
	'Rules' text not null,
	'Target' int not null,
	'StartTime' datetime not null,
	'EndTime' datetime not null,
	'Enable' bool not null
);

create table 'Customer'
(
	'Id' uniqueidentifier not null
		constraint Customer_pk
			primary key,

	'UserName' text not null,
	'Password' text not null,
	'CreateTime' datetime not null
);

create table 'CartItem'
(
	'Id' uniqueidentifier not null
		constraint CartItem_pk
			primary key,

	'CustomerId' uniqueidentifier not null
		constraint CartItem_CustomerId_Customer_Id_fk
			references 'Customer'
				on delete cascade,

	'Selected' int not null,

	'ProductId' uniqueidentifier not null
		constraint CartItem_ProductId_Product_Id_fk
			references 'Product'
				on delete cascade,

	'SkuId' uniqueidentifier not null
		constraint CartItem_SkuId_Sku_Id_fk
			references 'ProductSku'
				on delete cascade,

	'Quantity' int not null,
	'EditTime' datetime not null
);

create table 'Consignee'
(
	'Id' uniqueidentifier not null
		constraint Consignee_pk
			primary key,

	'CustomerId' uniqueidentifier not null
		constraint Consignee_CustomerId_Customer_Id_fk
			references 'Customer'
				on delete cascade,

	'County' text not null,
	'State' text,
	'City' text not null,
	'Address' text not null,
	'Name' text not null,
	'Phone' text not null
);

create table 'Order'
(
	'Id' uniqueidentifier not null
		constraint Order_pk
			primary key,

	'CustomerId' uniqueidentifier not null
		constraint Order_CustomerId_Customer_Id_fk
			references 'Customer'
				on delete cascade,

	'CreateTime' datetime not null,
	'PaymentTime' datetime,
	'Amount' real not null,
	'PaymentMethod' text not null,
	'Consignee' text not null,
	'State' int not null,
	'Promotions' text not null
);

create table 'OrderItem'
(
	'Id' uniqueidentifier not null
		constraint OrderItem_pk
			primary key,

	'OrderId' uniqueidentifier not null
		constraint OrderItem_OrderId_Order_Id_fk
			references 'Order'
				on delete cascade,

	'ProductId' uniqueidentifier not null,
	'SkuId' uniqueidentifier not null,
	'ProductName' text not null,
	'Specifications' text not null,
	'Price' real not null,
	'Tax' real not null,
	'Quantity' int not null,
	'State' int not null,
	'Promotions' text not null
);
");
        }
    }
}
