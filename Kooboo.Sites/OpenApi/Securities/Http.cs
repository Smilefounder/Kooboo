using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace Kooboo.Sites.OpenApi.Securities
{
    public class Http : Security
    {
        public override SecuritySchemeType Type => SecuritySchemeType.Http;

        public override AuthorizeResult Authorize(OpenApiSecurityScheme scheme, Models.OpenApi.AuthorizeData data)
        {
            var result = new AuthorizeResult();

            switch (scheme.Scheme.ToLower())
            {
                case "basic":
                    Basic(result, data);
                    break;

                case "bearer":
                    Helpers.AddBearer(result, data);
                    break;
                default:
                    throw new Exception($"Not support security Http scheme {scheme.Scheme}");
            }

            return result;
        }

        

        private void Basic(AuthorizeResult result, Models.OpenApi.AuthorizeData data)
        {
            var token = Helpers.BasicAuthEncode(data.Username, data.Password);
            result.AddHeader("Authorization", $"Basic {token}");
        }
    }
}
