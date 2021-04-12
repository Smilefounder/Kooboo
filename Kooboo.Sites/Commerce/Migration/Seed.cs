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
");
        }

        private static void ProductSku(IDbConnection connection)
        {
            connection.Execute(@"
");
        }

        private static void ProductStock(IDbConnection connection)
        {
            connection.Execute(@"
");
        }

        private static void Promotion(IDbConnection connection)
        {
            connection.Execute(@"
");
        }
    }
}
