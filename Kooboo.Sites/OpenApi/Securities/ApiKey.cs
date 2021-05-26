using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.OpenApi.Securities
{
    public class ApiKey : Security
    {
        public override SecuritySchemeType Type => SecuritySchemeType.ApiKey;

        public override AuthorizeResult Authorize(OpenApiSecurityScheme scheme, Models.OpenApi.AuthorizeData data)
        {
            var result = new AuthorizeResult();

            switch (scheme.In)
            {
                case ParameterLocation.Query:
                    result.AddQuery(scheme.Name, data.AccessToken);
                    break;
                case ParameterLocation.Header:
                    result.AddHeader(scheme.Name, data.AccessToken);
                    break;
                case ParameterLocation.Path:
                    throw new Exception($"Not support security ApiKey In {scheme.Scheme}");
                case ParameterLocation.Cookie:
                    result.AddCookie(scheme.Name, data.AccessToken);
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
