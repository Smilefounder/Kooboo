using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Scripting.KDefine
{
    public class Define
    {
        public string Namespace { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public List<string> Extends { get; set; } = new List<string>();
        public List<Property> Properties { get; set; }
        public List<Method> Methods { get; set; }
        public Dictionary<string, string> Enums { get; set; }
        public string ValueType { get; set; }

        public class Property
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Discription { get; set; }
        }

        public class Method
        {
            public string Name { get; set; }
            public string ReturnType { get; set; }
            public List<Param> Params { get; set; } = new List<Param>();
            public string Discription { get; set; }
            public class Param
            {
                public string Type { get; set; }
                public string Name { get; set; }
            }
        }
    }
}
