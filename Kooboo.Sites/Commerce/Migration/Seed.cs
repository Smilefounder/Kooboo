using Dapper;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.ViewModels.Product;
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
    }
}
