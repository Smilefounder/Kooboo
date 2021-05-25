using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Kooboo.Sites.Scripting.KDefine
{
    public class Define
    {
        private string name;

        public string Namespace { get; set; }
        public string Name { get => StandardName(name); set => name = value; }
        public string Discription { get; set; }
        public List<string> Extends { get; set; } = new List<string>();
        public List<Property> Properties { get; set; }
        public List<Method> Methods { get; set; }
        public Dictionary<string, string> Enums { get; set; }
        public string ValueType { get; set; }

        public class Property
        {
            private string name;

            public string Type { get; set; }
            public string Name { get => StandardName(name); set => name = value; }
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
                private string name;

                public string Type { get; set; }
                public string Name { get => StandardName(name); set => name = value; }
            }
        }

        static string StandardName(string name)
        {
            if (!Regex.IsMatch(name, "^[a-zA-Z_$]([a-zA-Z0-9_$\\?]+)?$")) return "_";
            return name;
        }
    }
}
