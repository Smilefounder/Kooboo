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
            Category(connection);
            ProductCategory(connection);
            Promotion(connection);
        }

        private static void ProductType(IDbConnection connection)
        {
            connection.Execute(@"
INSERT INTO ProductType (Id, Name, Attributes, Specifications) VALUES ('d21822dd-2db8-43df-b0d1-1f1ad0a9ba2d', 'Shirts', '[
  {
    ""id"": ""aff28007-460b-412a-ab8a-2a4417da19a4"",
    ""name"": ""Gender"",
    ""type"": ""Option"",
    ""options"": [
      {
        ""key"": ""2e9b0ffc-71d8-4c17-8611-616e64112edb"",
        ""value"": ""Male""
      },
      {
        ""key"": ""4c9b95f4-2fb6-45e8-8067-6ffa92b5e523"",
        ""value"": ""Female""
      }
    ]
  },
  {
    ""id"": ""8a725e64-79db-48b4-97a7-725124db5f85"",
    ""name"": ""Material"",
    ""type"": ""Text"",
    ""options"": []
  },
  {
    ""id"": ""dbc50f14-ae38-4182-8cd8-1dcf94700c76"",
    ""name"": ""Country of Origin"",
    ""type"": ""Text"",
    ""options"": []
  }
]', '[
  {
    ""id"": ""7fcc2086-e8eb-41f0-8405-337005a2f49b"",
    ""name"": ""Color"",
    ""type"": ""Text"",
    ""options"": []
  },
  {
    ""id"": ""83ddf252-096e-4181-a929-187a8b5382f5"",
    ""name"": ""Size"",
    ""type"": ""Option"",
    ""options"": [
      {
        ""key"": ""61a6305e-1d95-47e8-b0c4-4e8c57f1a798"",
        ""value"": ""S""
      },
      {
        ""key"": ""dca37bf9-09a3-4932-86a5-ce336c8f2762"",
        ""value"": ""M""
      },
      {
        ""key"": ""140cb097-34b6-431c-bdd0-ec802b70eeda"",
        ""value"": ""L""
      },
      {
        ""key"": ""e77ebff3-16ff-465d-8f18-cdba18e802b6"",
        ""value"": ""XL""
      },
      {
        ""key"": ""9862890c-f326-4307-8fef-3a3478ba1635"",
        ""value"": ""XXL""
      }
    ]
  }
]');
INSERT INTO ProductType (Id, Name, Attributes, Specifications) VALUES ('b2b0e76e-74ba-4990-9c25-1330c90e7ca4', 'Hats & Caps', '[
  {
    ""id"": ""546ffe7e-a9df-4979-8247-e43bbaa0566e"",
    ""name"": ""Style"",
    ""type"": ""Text"",
    ""options"": []
  },
  {
    ""id"": ""295f8b92-ac82-40b0-b139-e4134aa58905"",
    ""name"": ""Material"",
    ""type"": ""Text"",
    ""options"": []
  }
]', '[
  {
    ""id"": ""1c0837d6-9928-41b7-b136-0a0f312f12a9"",
    ""name"": ""Color"",
    ""type"": ""Text"",
    ""options"": []
  }
]');
INSERT INTO ProductType (Id, Name, Attributes, Specifications) VALUES ('63e2cacf-0d6a-4b57-9254-f54b5906bf29', 'Pants', '[]', '[
  {
    ""id"": ""c49f29af-3f3e-437c-bdbe-dbd5cdeba56c"",
    ""name"": ""Color"",
    ""type"": ""Text"",
    ""options"": []
  }
]');
INSERT INTO ProductType (Id, Name, Attributes, Specifications) VALUES ('324fde9f-1284-459e-ac82-22ef3c5706b8', 'Belts', '[
  {
    ""id"": ""debe16a8-2d8a-4522-abfc-0a9f8d3ad53e"",
    ""name"": ""Material"",
    ""type"": ""Text"",
    ""options"": []
  }
]', '[]');
");
        }

        private static void Product(IDbConnection connection)
        {
            connection.Execute(@"
INSERT INTO Product (Id, Title, Images, Description, Attributes, Specifications, CreateTime, TypeId, Enable) VALUES ('3ad592c8-96b7-44d2-9f49-a707125b1fd4', 'Youngor Men''s Spring 2021 Young Men''s Casual Dress Shirt GLDP16384BJA Bleach 39', '[]', '<p><span style=""color: #333333; font-family: Arial, ''Microsoft YaHei'', ''\微软雅黑'', ''\宋体'', ''Malgun Gothic'', Meiryo, sans-serif; font-size: 14px; background-color: #f7f8fa;"">Youngor Men''s Spring 2021 Young Men''s Casual Dress Shirt GLDP16384BJA Bleach 39</span></p>', '[
  {
    ""key"": ""aff28007-460b-412a-ab8a-2a4417da19a4"",
    ""value"": ""2e9b0ffc-71d8-4c17-8611-616e64112edb""
  },
  {
    ""key"": ""8a725e64-79db-48b4-97a7-725124db5f85"",
    ""value"": ""cotton""
  },
  {
    ""key"": ""dbc50f14-ae38-4182-8cd8-1dcf94700c76"",
    ""value"": ""China""
  }
]', '[
  {
    ""id"": ""7fcc2086-e8eb-41f0-8405-337005a2f49b"",
    ""options"": [
      {
        ""key"": ""1f91b961-9dd8-41c9-a01b-d8fa7bf271f3"",
        ""value"": ""White""
      },
      {
        ""key"": ""036e34fe-1668-4957-b9e5-08811f40909b"",
        ""value"": ""Black""
      }
    ],
    ""value"": [
      ""1f91b961-9dd8-41c9-a01b-d8fa7bf271f3"",
      ""036e34fe-1668-4957-b9e5-08811f40909b""
    ],
    ""type"": ""Text""
  },
  {
    ""id"": ""83ddf252-096e-4181-a929-187a8b5382f5"",
    ""options"": [],
    ""value"": [
      ""61a6305e-1d95-47e8-b0c4-4e8c57f1a798"",
      ""dca37bf9-09a3-4932-86a5-ce336c8f2762"",
      ""140cb097-34b6-431c-bdd0-ec802b70eeda""
    ],
    ""type"": ""Option""
  }
]', '2021-04-13 02:28:12.1039473Z', 'd21822dd-2db8-43df-b0d1-1f1ad0a9ba2d', 1);
INSERT INTO Product (Id, Title, Images, Description, Attributes, Specifications, CreateTime, TypeId, Enable) VALUES ('e1971329-2e12-4dcf-8f28-82da04aa2f03', 'Fluorescent cap acrylic wool cap men and women couples autumn and winter knitted caps', '[]', '<h1 style=""margin: 0px 0px 15px; padding: 0px; border: 0px; font-variant-numeric: inherit; font-variant-east-asian: inherit; font-stretch: inherit; font-size: 18px; line-height: 1em; font-family: Helvetica, Verdana, Arial; vertical-align: baseline; color: #222222; background-color: #f5f3f3;"">Fluorescent cap acrylic wool cap men and women couples autumn and winter knitted caps</h1>', '[
  {
    ""key"": ""546ffe7e-a9df-4979-8247-e43bbaa0566e"",
    ""value"": ""Street""
  },
  {
    ""key"": ""295f8b92-ac82-40b0-b139-e4134aa58905"",
    ""value"": ""NYLON""
  }
]', '[
  {
    ""id"": ""1c0837d6-9928-41b7-b136-0a0f312f12a9"",
    ""options"": [
      {
        ""key"": ""cddf9035-4c79-49ee-a676-b555a98c0175"",
        ""value"": ""White""
      }
    ],
    ""value"": [
      ""cddf9035-4c79-49ee-a676-b555a98c0175""
    ],
    ""type"": ""Text""
  }
]', '2021-04-13 02:32:02.2097701Z', 'b2b0e76e-74ba-4990-9c25-1330c90e7ca4', 1);
INSERT INTO Product (Id, Title, Images, Description, Attributes, Specifications, CreateTime, TypeId, Enable) VALUES ('4db04ad7-380a-4af8-b9bc-03f61c6b0a95', 'ELLE HOMME belt men''s leather automatic buckle business dress leather belt Korean men''s youth pants belt tide EE884700435 black black', '[]', '<h1 style=""margin: 0px 0px 15px; padding: 0px; border: 0px; font-variant-numeric: inherit; font-variant-east-asian: inherit; font-stretch: inherit; font-size: 18px; line-height: 1em; font-family: Helvetica, Verdana, Arial; vertical-align: baseline; color: #222222; background-color: #f5f3f3;"">ELLE HOMME belt men''s leather automatic buckle business dress leather belt Korean men''s youth pants belt tide EE884700435 black black</h1>', '[
  {
    ""key"": ""debe16a8-2d8a-4522-abfc-0a9f8d3ad53e"",
    ""value"": ""cowhide""
  }
]', '[]', '2021-04-13 02:39:12.4956983Z', '324fde9f-1284-459e-ac82-22ef3c5706b8', 1);
INSERT INTO Product (Id, Title, Images, Description, Attributes, Specifications, CreateTime, TypeId, Enable) VALUES ('4b92a2c3-abf6-4f44-9931-40ae955b4407', 'Fashion belt men''s leather retro cowhide belt wild casual leather belts zigzag belt', '[]', '<h1 style=""margin: 0px 0px 15px; padding: 0px; border: 0px; font-variant-numeric: inherit; font-variant-east-asian: inherit; font-stretch: inherit; font-size: 18px; line-height: 1em; font-family: Helvetica, Verdana, Arial; vertical-align: baseline; color: #222222; background-color: #f5f3f3;"">Fashion belt men''s leather retro cowhide belt wild casual leather belts zigzag belt</h1>', '[
  {
    ""key"": ""debe16a8-2d8a-4522-abfc-0a9f8d3ad53e"",
    ""value"": "" cowhide""
  }
]', '[]', '2021-04-13 02:40:21.9071192Z', '324fde9f-1284-459e-ac82-22ef3c5706b8', 0);
INSERT INTO Product (Id, Title, Images, Description, Attributes, Specifications, CreateTime, TypeId, Enable) VALUES ('3aae1d51-6298-40e9-8397-be12f043f03c', 'Summer men''s business casual short-sleeved shirt', '[]', '', '[
  {
    ""key"": ""aff28007-460b-412a-ab8a-2a4417da19a4"",
    ""value"": ""2e9b0ffc-71d8-4c17-8611-616e64112edb""
  },
  {
    ""key"": ""8a725e64-79db-48b4-97a7-725124db5f85"",
    ""value"": ""nylon""
  },
  {
    ""key"": ""dbc50f14-ae38-4182-8cd8-1dcf94700c76"",
    ""value"": ""USA""
  }
]', '[
  {
    ""id"": ""7fcc2086-e8eb-41f0-8405-337005a2f49b"",
    ""options"": [
      {
        ""key"": ""82cbb2b7-8dd4-4052-8c3b-a26063c5a545"",
        ""value"": ""Blue""
      }
    ],
    ""value"": [
      ""82cbb2b7-8dd4-4052-8c3b-a26063c5a545""
    ],
    ""type"": ""Text""
  },
  {
    ""id"": ""83ddf252-096e-4181-a929-187a8b5382f5"",
    ""options"": [],
    ""value"": [
      ""61a6305e-1d95-47e8-b0c4-4e8c57f1a798""
    ],
    ""type"": ""Option""
  }
]', '2021-04-13 02:42:17.0627907Z', 'd21822dd-2db8-43df-b0d1-1f1ad0a9ba2d', 1);
");
        }

        private static void ProductSku(IDbConnection connection)
        {
            connection.Execute(@"
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Image, Enable) VALUES ('928cae59-9aa8-4b3c-afdf-bbdfb29d6f70', '3ad592c8-96b7-44d2-9f49-a707125b1fd4', 'SWS1282930', '[
  {
    ""key"": ""7fcc2086-e8eb-41f0-8405-337005a2f49b"",
    ""value"": ""1f91b961-9dd8-41c9-a01b-d8fa7bf271f3""
  },
  {
    ""key"": ""83ddf252-096e-4181-a929-187a8b5382f5"",
    ""value"": ""61a6305e-1d95-47e8-b0c4-4e8c57f1a798""
  }
]', 128, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Image, Enable) VALUES ('8a142a71-56d8-4d85-b6a0-8b025329f48b', '3ad592c8-96b7-44d2-9f49-a707125b1fd4', 'SWM1282930', '[
  {
    ""key"": ""7fcc2086-e8eb-41f0-8405-337005a2f49b"",
    ""value"": ""1f91b961-9dd8-41c9-a01b-d8fa7bf271f3""
  },
  {
    ""key"": ""83ddf252-096e-4181-a929-187a8b5382f5"",
    ""value"": ""dca37bf9-09a3-4932-86a5-ce336c8f2762""
  }
]', 128, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Image, Enable) VALUES ('7ca5a0db-101b-4d55-a0d0-178e1ee69066', '3ad592c8-96b7-44d2-9f49-a707125b1fd4', 'SWS1282930', '[
  {
    ""key"": ""7fcc2086-e8eb-41f0-8405-337005a2f49b"",
    ""value"": ""036e34fe-1668-4957-b9e5-08811f40909b""
  },
  {
    ""key"": ""83ddf252-096e-4181-a929-187a8b5382f5"",
    ""value"": ""61a6305e-1d95-47e8-b0c4-4e8c57f1a798""
  }
]', 128, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Image, Enable) VALUES ('dd3a722e-1dbf-4708-bd66-a1e985bfafca', '3ad592c8-96b7-44d2-9f49-a707125b1fd4', 'SWM1282930', '[
  {
    ""key"": ""7fcc2086-e8eb-41f0-8405-337005a2f49b"",
    ""value"": ""036e34fe-1668-4957-b9e5-08811f40909b""
  },
  {
    ""key"": ""83ddf252-096e-4181-a929-187a8b5382f5"",
    ""value"": ""dca37bf9-09a3-4932-86a5-ce336c8f2762""
  }
]', 128, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Image, Enable) VALUES ('aa6ab95e-6d5d-453c-95de-121e14d873d0', '3ad592c8-96b7-44d2-9f49-a707125b1fd4', 'SWL1282930', '[
  {
    ""key"": ""7fcc2086-e8eb-41f0-8405-337005a2f49b"",
    ""value"": ""1f91b961-9dd8-41c9-a01b-d8fa7bf271f3""
  },
  {
    ""key"": ""83ddf252-096e-4181-a929-187a8b5382f5"",
    ""value"": ""140cb097-34b6-431c-bdd0-ec802b70eeda""
  }
]', 128, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Image, Enable) VALUES ('c2b782fc-d3b3-43ce-a754-05e48cea2bbd', '3ad592c8-96b7-44d2-9f49-a707125b1fd4', 'SBL1282930', '[
  {
    ""key"": ""7fcc2086-e8eb-41f0-8405-337005a2f49b"",
    ""value"": ""036e34fe-1668-4957-b9e5-08811f40909b""
  },
  {
    ""key"": ""83ddf252-096e-4181-a929-187a8b5382f5"",
    ""value"": ""140cb097-34b6-431c-bdd0-ec802b70eeda""
  }
]', 128, 0, null, 0);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Image, Enable) VALUES ('6c57eb8c-2635-4e42-bc52-c654009e30e8', 'e1971329-2e12-4dcf-8f28-82da04aa2f03', 'WC123X', '[
  {
    ""key"": ""1c0837d6-9928-41b7-b136-0a0f312f12a9"",
    ""value"": ""cddf9035-4c79-49ee-a676-b555a98c0175""
  }
]', 256, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Image, Enable) VALUES ('42a1709e-0759-4286-885c-36ba7669c863', '4db04ad7-380a-4af8-b9bc-03f61c6b0a95', 'EE884700435', '[]', 99, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Image, Enable) VALUES ('c84226a2-fa43-425d-973f-0e161199f12d', '4b92a2c3-abf6-4f44-9931-40ae955b4407', '', '[]', 300, 0, null, 1);
INSERT INTO ProductSku (Id, ProductId, Name, Specifications, Price, Tax, Image, Enable) VALUES ('244ab6bc-e58b-46cb-8078-c3b35627f748', '3aae1d51-6298-40e9-8397-be12f043f03c', '', '[
  {
    ""key"": ""7fcc2086-e8eb-41f0-8405-337005a2f49b"",
    ""value"": ""82cbb2b7-8dd4-4052-8c3b-a26063c5a545""
  },
  {
    ""key"": ""83ddf252-096e-4181-a929-187a8b5382f5"",
    ""value"": ""61a6305e-1d95-47e8-b0c4-4e8c57f1a798""
  }
]', 1999, 0, null, 1);
");
        }

        private static void ProductStock(IDbConnection connection)
        {
            connection.Execute(@"
INSERT INTO ProductStock (SkuId, ProductId, Quantity, Type, DateTime, OrderItemId) VALUES ('928cae59-9aa8-4b3c-afdf-bbdfb29d6f70', '3ad592c8-96b7-44d2-9f49-a707125b1fd4', 10, 0, '2021-04-13 02:28:12.1211069Z', null);
INSERT INTO ProductStock (SkuId, ProductId, Quantity, Type, DateTime, OrderItemId) VALUES ('8a142a71-56d8-4d85-b6a0-8b025329f48b', '3ad592c8-96b7-44d2-9f49-a707125b1fd4', 15, 0, '2021-04-13 02:28:12.1215167Z', null);
INSERT INTO ProductStock (SkuId, ProductId, Quantity, Type, DateTime, OrderItemId) VALUES ('7ca5a0db-101b-4d55-a0d0-178e1ee69066', '3ad592c8-96b7-44d2-9f49-a707125b1fd4', 20, 0, '2021-04-13 02:28:12.1215171Z', null);
INSERT INTO ProductStock (SkuId, ProductId, Quantity, Type, DateTime, OrderItemId) VALUES ('dd3a722e-1dbf-4708-bd66-a1e985bfafca', '3ad592c8-96b7-44d2-9f49-a707125b1fd4', 5, 0, '2021-04-13 02:28:12.1215178Z', null);
INSERT INTO ProductStock (SkuId, ProductId, Quantity, Type, DateTime, OrderItemId) VALUES ('aa6ab95e-6d5d-453c-95de-121e14d873d0', '3ad592c8-96b7-44d2-9f49-a707125b1fd4', 10, 0, '2021-04-13 02:28:12.1215182Z', null);
INSERT INTO ProductStock (SkuId, ProductId, Quantity, Type, DateTime, OrderItemId) VALUES ('6c57eb8c-2635-4e42-bc52-c654009e30e8', 'e1971329-2e12-4dcf-8f28-82da04aa2f03', 10, 0, '2021-04-13 02:32:02.2202605Z', null);
INSERT INTO ProductStock (SkuId, ProductId, Quantity, Type, DateTime, OrderItemId) VALUES ('42a1709e-0759-4286-885c-36ba7669c863', '4db04ad7-380a-4af8-b9bc-03f61c6b0a95', 200, 0, '2021-04-13 02:39:12.5063119Z', null);
INSERT INTO ProductStock (SkuId, ProductId, Quantity, Type, DateTime, OrderItemId) VALUES ('c84226a2-fa43-425d-973f-0e161199f12d', '4b92a2c3-abf6-4f44-9931-40ae955b4407', 10, 0, '2021-04-13 02:40:21.9142841Z', null);
INSERT INTO ProductStock (SkuId, ProductId, Quantity, Type, DateTime, OrderItemId) VALUES ('244ab6bc-e58b-46cb-8078-c3b35627f748', '3aae1d51-6298-40e9-8397-be12f043f03c', 3, 0, '2021-04-13 02:45:39.1024627Z', null);
");
        }

        private static void Category(IDbConnection connection)
        {
            connection.Execute(@"
INSERT INTO Category (Id, Type, Name, CreateTime, Rule, Enable) VALUES ('7783813d-a426-4974-b2b5-f9bdad59c70f', 1, 'men', '2021-04-16 11:28:31.7341047Z', '{
  ""type"": ""All"",
  ""conditions"": [
    {
      ""id"": ""8a821f0c-4ddd-49f1-a57e-4851da41c88a"",
      ""left"": ""Title"",
      ""comparer"": ""Contains"",
      ""right"": ""men""
    }
  ]
}', 1);
INSERT INTO Category (Id, Type, Name, CreateTime, Rule, Enable) VALUES ('63e692ec-344b-469d-ba4a-72c60eeb4750', 1, 'women', '2021-04-16 11:29:05.1804257Z', '{
  ""type"": ""All"",
  ""conditions"": [
    {
      ""id"": ""e567fe57-b319-4fdd-9ba8-230c12c82a75"",
      ""left"": ""Title"",
      ""comparer"": ""Contains"",
      ""right"": ""women""
    }
  ]
}', 1);
INSERT INTO Category (Id, Type, Name, CreateTime, Rule, Enable) VALUES ('d343e513-347d-42ff-8e64-e07cd2dd08ed', 0, 'Belts', '2021-04-16 11:29:47.9012504Z', '{
  ""type"": ""All"",
  ""conditions"": []
}', 1);
INSERT INTO Category (Id, Type, Name, CreateTime, Rule, Enable) VALUES ('2b1cf62a-214e-4380-a122-09e5afb24809', 1, '0-99', '2021-04-16 11:30:24.362346Z', '{
  ""type"": ""All"",
  ""conditions"": [
    {
      ""id"": ""263bcdec-467d-4eee-b30f-c6d908ee5b1f"",
      ""left"": ""Price"",
      ""comparer"": ""GreaterThanOrEqual"",
      ""right"": ""0""
    },
    {
      ""id"": ""aa9411f7-16c9-4cd6-b2d6-7d24982c5545"",
      ""left"": ""Price"",
      ""comparer"": ""LessThanOrEqual"",
      ""right"": ""99""
    }
  ]
}', 1);
INSERT INTO Category (Id, Type, Name, CreateTime, Rule, Enable) VALUES ('4e29d66d-35a0-46bb-801f-faf90719f474', 1, '100-199', '2021-04-16 11:30:56.6438849Z', '{
  ""type"": ""All"",
  ""conditions"": [
    {
      ""id"": ""aa0ca427-e376-40d9-9aa0-cb03443d606d"",
      ""left"": ""Price"",
      ""comparer"": ""GreaterThanOrEqual"",
      ""right"": ""100""
    },
    {
      ""id"": ""28eceea6-a35f-4a09-9039-7ebdfee1d988"",
      ""left"": ""Price"",
      ""comparer"": ""LessThanOrEqual"",
      ""right"": ""199""
    }
  ]
}', 1);
");
        }
        private static void ProductCategory(IDbConnection connection)
        {
            connection.Execute(@"
INSERT INTO ProductCategory (CategoryId, ProductId) VALUES ('d343e513-347d-42ff-8e64-e07cd2dd08ed', '4db04ad7-380a-4af8-b9bc-03f61c6b0a95');
INSERT INTO ProductCategory (CategoryId, ProductId) VALUES ('d343e513-347d-42ff-8e64-e07cd2dd08ed', '4b92a2c3-abf6-4f44-9931-40ae955b4407');
");
        }
        private static void Promotion(IDbConnection connection)
        {
            connection.Execute(@"
");
        }
    }
}
