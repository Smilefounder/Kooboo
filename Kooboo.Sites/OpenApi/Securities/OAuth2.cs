using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.OpenApi.Securities
{
    public class OAuth2 : Security
    {
        public override SecuritySchemeType Type => SecuritySchemeType.OAuth2;

        public override AuthorizeResult Authorize(OpenApiSecurityScheme scheme, Models.OpenApi.AuthorizeData data)
        {
            throw new NotImplementedException();
        }
    }
}
