using Dapper;
using Kooboo.Sites.Commerce.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Kooboo.Sites.Commerce.Migration
{
    public static class Seed
    {
        public static void Insert(IDbConnection connection)
        {
            var categorys = new[] {
                new Category{
                    Id=Guid.Parse("a0078376-53b4-436c-a37d-5a273b130771"),
                    Name="Clothes",
                    Attributes="[]",
                    Specifications="[]"
                },
                new Category{
                    Id=Guid.Parse("a0078376-53b4-436c-a37d-5a273b130774"),
                    Attributes="[{\"id\":\"9ee7be45-91e5-39c8-b372-9c51d0b20682\",\"name\":\"Style\",\"type\":0,\"editingItem\":\"\",\"options\":[]},{\"id\":\"7c8c5b0c-01a5-361a-8d43-59130e2d739c\",\"name\":\"Sleeve\",\"type\":1,\"editingItem\":\"\",\"options\":[{\"id\":\"9b8928b7-c72c-3be4-abb3-af03c93d6835\",\"value\":\"Long\"},{\"id\":\"922f012b-6252-317e-9134-87d5edae1df0\",\"value\":\"Short\"}]}]",
                    Name="Shirt",
                    Parent=Guid.Parse("a0078376-53b4-436c-a37d-5a273b130771"),
                    Specifications="[{\"id\":\"3a73ef56-ccac-3593-bd8c-55e9e6a3c77f\",\"name\":\"Size\",\"type\":1,\"editingItem\":\"\",\"options\":[{\"id\":\"228d6f86-0c68-30a0-b5d2-e4d14864f6cc\",\"value\":\"S\"},{\"id\":\"5fac8d97-38a0-3501-9b00-cf23c4e65c6b\",\"value\":\"M\"},{\"id\":\"804aedc4-8cc0-3498-a918-a23883aacca2\",\"value\":\"L\"},{\"id\":\"60a7d228-6298-3a86-b323-4e15df892ae9\",\"value\":\"XL\"}]},{\"id\":\"f63e5109-a907-351d-896d-c4a471513c76\",\"name\":\"Color\",\"type\":0,\"editingItem\":\"\",\"options\":[]}]"
                },
                new Category{
                    Id=Guid.Parse("b0078376-53b4-436c-a37d-5a273b130771"),
                    Name="Shoes",
                    Attributes="[]",
                    Specifications="[]"
                },
                new Category{
                    Id=Guid.Parse("c0078376-53b4-436c-a37d-5a273b130771"),
                    Name="Headgear",
                    Attributes="[]",
                    Specifications="[]"
                },
            };

            connection.Insert(categorys);
        }
    }
}
