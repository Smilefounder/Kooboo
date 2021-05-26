using Kooboo.Sites.FrontEvent;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Models
{
    [Kooboo.Attributes.Diskable]
    public class OpenApi : CoreObject
    {
        private Dictionary<string, AuthorizeData> securities;

        public string JsonData { get; set; }
        public string Url { get; set; }
        public bool IsRemote { get; set; }
        public Dictionary<string, AuthorizeData> Securities
        {
            get
            {
                if (securities == null) securities = new Dictionary<string, AuthorizeData>();
                return securities;
            }
            set => securities = value;
        }

        public class AuthorizeData
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
            public string TokenType { get; set; }
            public DateTime ExpiresIn { get; set; }
            public string RedirectUrl { get; set; }
            public string Name { get; set; }
        }

        //TODO Cache

        public override int GetHashCode()
        {
            var un = Name;
            un += Online.ToString();
            un += JsonData;
            un += Url;
            un += IsRemote;

            if (Securities != null)
            {
                foreach (var item in Securities)
                {
                    un += item.Key;
                    un += item.Value?.Username;
                    un += item.Value?.Password;
                    un += item.Value?.ClientId;
                    un += item.Value?.ClientSecret;
                    un += item.Value?.AccessToken;
                    un += item.Value?.RefreshToken;
                    un += item.Value?.ExpiresIn;
                    un += item.Value?.RedirectUrl;
                }
            }

            return Lib.Security.Hash.ComputeIntCaseSensitive(un);
        }
    }
}
