using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.OpenApi
{
    public abstract class Security
    {
        static readonly List<Security> _securities = Lib.IOC.Service.GetInstances<Security>();

        public abstract SecuritySchemeType Type { get; }

        public abstract AuthorizeResult Authorize(OpenApiSecurityScheme scheme, Models.OpenApi.AuthorizeData data);

        public static Security Get(SecuritySchemeType type)
        {
            var security = _securities.First(f => f.Type == type);
            if (security == null) throw new Exception($"Not support security type {type}");
            return security;
        }

        public class AuthorizeResult
        {
            public Dictionary<string, string> Querys { get; private set; }
            public Dictionary<string, string> Headers { get; private set; }
            public Dictionary<string, string> Cookies { get; private set; }

            public void AddQuery(string name, string value)
            {
                if (Querys == null) Querys = new Dictionary<string, string>();
                Querys.Add(name, value);
            }

            public void AddHeader(string name, string value)
            {
                if (Headers == null) Headers = new Dictionary<string, string>();
                Headers.Add(name, value);
            }

            public void AddCookie(string name, string value)
            {
                if (Cookies == null) Cookies = new Dictionary<string, string>();
                Cookies.Add(name, value);
            }
        }
    }
}
