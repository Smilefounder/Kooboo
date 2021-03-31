using Dapper;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models.Product;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Migration
{
    public static class Seed
    {
        public static void Insert(IDbConnection connection)
        {
            ProductType(connection);
            Product(connection);
            ProductSku(connection);
            ProductStock(connection);
            Promotion(connection);
        }

        private static void ProductType(IDbConnection connection)
        {
            connection.Execute(@"
INSERT INTO ProductType (Id, Name, Attributes, Specifications) VALUES ('0091733c-6f1b-4994-824b-1a1e05f4713c', '衬衫', '[
  {
    ""id"": ""b8d046ab-d653-4ced-86ba-4d74c24a2c2c"",
    ""name"": ""产地"",
    ""type"": 1,
    ""options"": [
      {
        ""key"": ""45563789-d3ad-464f-a4ae-54d45ebe374a"",
        ""value"": ""中国""
      },
      {
        ""key"": ""4756a5db-67ce-443c-95fe-c56111ed51e9"",
        ""value"": ""越南""
      }
    ]
  },
  {
    ""id"": ""f7c310ec-724e-4b72-96c1-bb9c9c772c25"",
    ""name"": ""风格"",
    ""type"": 0,
    ""options"": []
  },
  {
    ""id"": ""a6b3ec81-39de-40f8-82ae-517e8d23c8ff"",
    ""name"": ""年龄段"",
    ""type"": 1,
    ""options"": [
      {
        ""key"": ""a3d9806c-6b96-4e93-8dca-21e08121952b"",
        ""value"": ""少年""
      },
      {
        ""key"": ""3e76ca9c-6600-48de-afb3-3af5cd06db12"",
        ""value"": ""青年""
      },
      {
        ""key"": ""19cd7b45-1b57-4562-9bc6-2ec827951f1d"",
        ""value"": ""中年""
      }
    ]
  },
  {
    ""id"": ""3f5661e6-8e90-455b-950e-1ad0d6e7fad9"",
    ""name"": ""材质"",
    ""type"": 0,
    ""options"": []
  }
]', '[
  {
    ""id"": ""8d5877f8-924a-4672-89c0-f6231e33a2f1"",
    ""name"": ""尺寸"",
    ""type"": 1,
    ""options"": [
      {
        ""key"": ""acf95c38-cb05-4c77-962f-c764b20f3d11"",
        ""value"": ""M""
      },
      {
        ""key"": ""ec7a30fd-9853-4dbf-a869-6270182746a5"",
        ""value"": ""L""
      },
      {
        ""key"": ""dcdc650c-2620-437b-93ae-4fda294e97e5"",
        ""value"": ""XL""
      }
    ]
  },
  {
    ""id"": ""492a2c89-4b6f-4475-813c-44107c4309d7"",
    ""name"": ""颜色"",
    ""type"": 0,
    ""options"": []
  },
  {
    ""id"": ""9f6315e4-e3f5-4b3f-8053-17824e873b1b"",
    ""name"": ""性别"",
    ""type"": 1,
    ""options"": [
      {
        ""key"": ""46fc087c-e34e-4a43-a151-4e15470f0e0f"",
        ""value"": ""男""
      },
      {
        ""key"": ""0693e43e-619e-4288-9478-018b0b6464c4"",
        ""value"": ""女""
      }
    ]
  }
]');
");
        }

        private static void Product(IDbConnection connection)
        {
            connection.Execute(@"
INSERT INTO Product (Id, Title, Images, Description, Attributes, Specifications, CreateDate, TypeId, Enable) VALUES ('0584b229-3bbb-4208-9afc-b803274b6e17', '优衣库短袖格子衫', '[]', '<p>优衣库短袖格子衫</p>
<p>优衣库短袖格子衫</p>
<p>优衣库短袖格子衫</p>
<p>优衣库短袖格子衫</p>', '[
  {
    ""key"": ""b8d046ab-d653-4ced-86ba-4d74c24a2c2c"",
    ""value"": ""45563789-d3ad-464f-a4ae-54d45ebe374a""
  },
  {
    ""key"": ""f7c310ec-724e-4b72-96c1-bb9c9c772c25"",
    ""value"": ""日系""
  },
  {
    ""key"": ""a6b3ec81-39de-40f8-82ae-517e8d23c8ff"",
    ""value"": ""3e76ca9c-6600-48de-afb3-3af5cd06db12""
  },
  {
    ""key"": ""3f5661e6-8e90-455b-950e-1ad0d6e7fad9"",
    ""value"": ""棉""
  }
]', '[
  {
    ""key"": ""f818a72e-c373-4184-bd0f-7697d7bd1976"",
    ""value"": ""白色""
  },
  {
    ""key"": ""1ed11697-bef5-41ac-8e77-13ad54f2980d"",
    ""value"": ""黄色""
  }
]', '0001-01-01 00:00:00', '0091733c-6f1b-4994-824b-1a1e05f4713c', 1);
INSERT INTO Product (Id, Title, Images, Description, Attributes, Specifications, CreateDate, TypeId, Enable) VALUES ('2ad7c374-8384-4050-b6b5-e0f399f8245b', '无印良品亚麻衬衫', '[]', '<p>无印良品亚麻衬衫</p>
<p>无印良品亚麻衬衫</p>
<p>无印良品亚麻衬衫</p>
<p>无印良品亚麻衬衫</p>', '[
  {
    ""key"": ""b8d046ab-d653-4ced-86ba-4d74c24a2c2c"",
    ""value"": ""45563789-d3ad-464f-a4ae-54d45ebe374a""
  },
  {
    ""key"": ""f7c310ec-724e-4b72-96c1-bb9c9c772c25"",
    ""value"": ""日系""
  },
  {
    ""key"": ""a6b3ec81-39de-40f8-82ae-517e8d23c8ff"",
    ""value"": ""a3d9806c-6b96-4e93-8dca-21e08121952b""
  },
  {
    ""key"": ""3f5661e6-8e90-455b-950e-1ad0d6e7fad9"",
    ""value"": ""麻""
  }
]', '[
  {
    ""key"": ""fee17e97-5165-470d-9e33-4230c506d966"",
    ""value"": ""褐色""
  }
]', '0001-01-01 00:00:00', '0091733c-6f1b-4994-824b-1a1e05f4713c', 1);
");
        }

        private static void ProductSku(IDbConnection connection)
        {
            connection.Execute(@"
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Thumbnail, Enable) VALUES ('33dc9f81-9984-3317-806d-07ee533bbb1c', '0584b229-3bbb-4208-9afc-b803274b6e17', '', '[
  {
    ""key"": ""8d5877f8-924a-4672-89c0-f6231e33a2f1"",
    ""value"": ""acf95c38-cb05-4c77-962f-c764b20f3d11""
  },
  {
    ""key"": ""492a2c89-4b6f-4475-813c-44107c4309d7"",
    ""value"": ""f818a72e-c373-4184-bd0f-7697d7bd1976""
  },
  {
    ""key"": ""9f6315e4-e3f5-4b3f-8053-17824e873b1b"",
    ""value"": ""46fc087c-e34e-4a43-a151-4e15470f0e0f""
  }
]', 128, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Thumbnail, Enable) VALUES ('8252db9e-663e-3fe6-9643-e2aeb480e260', '0584b229-3bbb-4208-9afc-b803274b6e17', '', '[
  {
    ""key"": ""8d5877f8-924a-4672-89c0-f6231e33a2f1"",
    ""value"": ""acf95c38-cb05-4c77-962f-c764b20f3d11""
  },
  {
    ""key"": ""492a2c89-4b6f-4475-813c-44107c4309d7"",
    ""value"": ""1ed11697-bef5-41ac-8e77-13ad54f2980d""
  },
  {
    ""key"": ""9f6315e4-e3f5-4b3f-8053-17824e873b1b"",
    ""value"": ""46fc087c-e34e-4a43-a151-4e15470f0e0f""
  }
]', 128, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Thumbnail, Enable) VALUES ('d4c5487e-0672-3443-9798-4e7ab06514f4', '0584b229-3bbb-4208-9afc-b803274b6e17', '', '[
  {
    ""key"": ""8d5877f8-924a-4672-89c0-f6231e33a2f1"",
    ""value"": ""ec7a30fd-9853-4dbf-a869-6270182746a5""
  },
  {
    ""key"": ""492a2c89-4b6f-4475-813c-44107c4309d7"",
    ""value"": ""f818a72e-c373-4184-bd0f-7697d7bd1976""
  },
  {
    ""key"": ""9f6315e4-e3f5-4b3f-8053-17824e873b1b"",
    ""value"": ""46fc087c-e34e-4a43-a151-4e15470f0e0f""
  }
]', 128, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Thumbnail, Enable) VALUES ('a52fa77c-134e-374d-b951-5e2b629bbb92', '0584b229-3bbb-4208-9afc-b803274b6e17', '', '[
  {
    ""key"": ""8d5877f8-924a-4672-89c0-f6231e33a2f1"",
    ""value"": ""ec7a30fd-9853-4dbf-a869-6270182746a5""
  },
  {
    ""key"": ""492a2c89-4b6f-4475-813c-44107c4309d7"",
    ""value"": ""1ed11697-bef5-41ac-8e77-13ad54f2980d""
  },
  {
    ""key"": ""9f6315e4-e3f5-4b3f-8053-17824e873b1b"",
    ""value"": ""46fc087c-e34e-4a43-a151-4e15470f0e0f""
  }
]', 128, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Thumbnail, Enable) VALUES ('5f5c334b-d8d4-3e07-a95c-846d89ef504a', '0584b229-3bbb-4208-9afc-b803274b6e17', '', '[
  {
    ""key"": ""8d5877f8-924a-4672-89c0-f6231e33a2f1"",
    ""value"": ""dcdc650c-2620-437b-93ae-4fda294e97e5""
  },
  {
    ""key"": ""492a2c89-4b6f-4475-813c-44107c4309d7"",
    ""value"": ""f818a72e-c373-4184-bd0f-7697d7bd1976""
  },
  {
    ""key"": ""9f6315e4-e3f5-4b3f-8053-17824e873b1b"",
    ""value"": ""46fc087c-e34e-4a43-a151-4e15470f0e0f""
  }
]', 128, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Thumbnail, Enable) VALUES ('f9293df1-a428-3b36-9321-21220b7c74ca', '0584b229-3bbb-4208-9afc-b803274b6e17', '', '[
  {
    ""key"": ""8d5877f8-924a-4672-89c0-f6231e33a2f1"",
    ""value"": ""dcdc650c-2620-437b-93ae-4fda294e97e5""
  },
  {
    ""key"": ""492a2c89-4b6f-4475-813c-44107c4309d7"",
    ""value"": ""1ed11697-bef5-41ac-8e77-13ad54f2980d""
  },
  {
    ""key"": ""9f6315e4-e3f5-4b3f-8053-17824e873b1b"",
    ""value"": ""46fc087c-e34e-4a43-a151-4e15470f0e0f""
  }
]', 128, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Thumbnail, Enable) VALUES ('cbf1f430-d512-3785-bad6-6cff82948f49', '2ad7c374-8384-4050-b6b5-e0f399f8245b', '', '[
  {
    ""key"": ""8d5877f8-924a-4672-89c0-f6231e33a2f1"",
    ""value"": ""acf95c38-cb05-4c77-962f-c764b20f3d11""
  },
  {
    ""key"": ""492a2c89-4b6f-4475-813c-44107c4309d7"",
    ""value"": ""fee17e97-5165-470d-9e33-4230c506d966""
  },
  {
    ""key"": ""9f6315e4-e3f5-4b3f-8053-17824e873b1b"",
    ""value"": ""46fc087c-e34e-4a43-a151-4e15470f0e0f""
  }
]', 300, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Thumbnail, Enable) VALUES ('3e59898d-450d-3703-a513-f09187f3bda1', '2ad7c374-8384-4050-b6b5-e0f399f8245b', '', '[
  {
    ""key"": ""8d5877f8-924a-4672-89c0-f6231e33a2f1"",
    ""value"": ""ec7a30fd-9853-4dbf-a869-6270182746a5""
  },
  {
    ""key"": ""492a2c89-4b6f-4475-813c-44107c4309d7"",
    ""value"": ""fee17e97-5165-470d-9e33-4230c506d966""
  },
  {
    ""key"": ""9f6315e4-e3f5-4b3f-8053-17824e873b1b"",
    ""value"": ""46fc087c-e34e-4a43-a151-4e15470f0e0f""
  }
]', 300, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Thumbnail, Enable) VALUES ('629439c4-97f1-3878-80b7-5807dd6e73fb', '2ad7c374-8384-4050-b6b5-e0f399f8245b', '', '[
  {
    ""key"": ""8d5877f8-924a-4672-89c0-f6231e33a2f1"",
    ""value"": ""dcdc650c-2620-437b-93ae-4fda294e97e5""
  },
  {
    ""key"": ""492a2c89-4b6f-4475-813c-44107c4309d7"",
    ""value"": ""fee17e97-5165-470d-9e33-4230c506d966""
  },
  {
    ""key"": ""9f6315e4-e3f5-4b3f-8053-17824e873b1b"",
    ""value"": ""46fc087c-e34e-4a43-a151-4e15470f0e0f""
  }
]', 300, 0, null, 1);
");
        }

        private static void ProductStock(IDbConnection connection)
        {
            connection.Execute(@"
INSERT INTO ProductStock (SkuId, ProductId, Quantity, StockType, DateTime) VALUES ('33dc9f81-9984-3317-806d-07ee533bbb1c', '0584b229-3bbb-4208-9afc-b803274b6e17', 10, 0, '2021-03-31 09:13:14.3864508Z');
INSERT INTO ProductStock (SkuId, ProductId, Quantity, StockType, DateTime) VALUES ('8252db9e-663e-3fe6-9643-e2aeb480e260', '0584b229-3bbb-4208-9afc-b803274b6e17', 10, 0, '2021-03-31 09:13:14.3867889Z');
INSERT INTO ProductStock (SkuId, ProductId, Quantity, StockType, DateTime) VALUES ('d4c5487e-0672-3443-9798-4e7ab06514f4', '0584b229-3bbb-4208-9afc-b803274b6e17', 10, 0, '2021-03-31 09:13:14.3867894Z');
INSERT INTO ProductStock (SkuId, ProductId, Quantity, StockType, DateTime) VALUES ('a52fa77c-134e-374d-b951-5e2b629bbb92', '0584b229-3bbb-4208-9afc-b803274b6e17', 10, 0, '2021-03-31 09:13:14.3867896Z');
INSERT INTO ProductStock (SkuId, ProductId, Quantity, StockType, DateTime) VALUES ('5f5c334b-d8d4-3e07-a95c-846d89ef504a', '0584b229-3bbb-4208-9afc-b803274b6e17', 10, 0, '2021-03-31 09:13:14.3867898Z');
INSERT INTO ProductStock (SkuId, ProductId, Quantity, StockType, DateTime) VALUES ('f9293df1-a428-3b36-9321-21220b7c74ca', '0584b229-3bbb-4208-9afc-b803274b6e17', 10, 0, '2021-03-31 09:13:14.3867907Z');
INSERT INTO ProductStock (SkuId, ProductId, Quantity, StockType, DateTime) VALUES ('cbf1f430-d512-3785-bad6-6cff82948f49', '2ad7c374-8384-4050-b6b5-e0f399f8245b', 8, 0, '2021-03-31 09:32:12.1873292Z');
INSERT INTO ProductStock (SkuId, ProductId, Quantity, StockType, DateTime) VALUES ('3e59898d-450d-3703-a513-f09187f3bda1', '2ad7c374-8384-4050-b6b5-e0f399f8245b', 8, 0, '2021-03-31 09:32:12.1873305Z');
INSERT INTO ProductStock (SkuId, ProductId, Quantity, StockType, DateTime) VALUES ('629439c4-97f1-3878-80b7-5807dd6e73fb', '2ad7c374-8384-4050-b6b5-e0f399f8245b', 8, 0, '2021-03-31 09:32:12.1873308Z');
");
        }

        private static void Promotion(IDbConnection connection)
        {
            connection.Execute(@"
INSERT INTO Promotion (Id, Name, Description, Type, Priority, Exclusive, Discount, Rules, Target, StartTime, EndTime) VALUES ('97f38de0-e15f-4dd3-aab6-47ee3d44c97e', '订单满300减50', '订单满300减50订单满300减50订单满300减50订单满300减50', 0, 0, 0, 50, '{
  ""order"": {
    ""type"": ""All"",
    ""conditions"": [
      {
        ""id"": ""56287df2-4bc6-48b4-bb8e-1dc3e723e83d"",
        ""left"": ""Amount"",
        ""comparer"": ""GreaterThanOrEqual"",
        ""right"": ""300""
      }
    ]
  },
  ""orderItem"": {
    ""type"": ""All"",
    ""conditions"": []
  }
}', 0, '2021-03-31 09:42:00Z', '2025-06-30 09:42:00Z');
");
        }
    }
}
