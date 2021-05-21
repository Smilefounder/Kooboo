using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.OpenApi
{
    public class Security
    {
        public Security(OpenApiSecurityScheme scheme)
        {

            Type = scheme.Type;
            Description = scheme.Description;
            Name = scheme.Name;
            In = scheme.In;
            Scheme = scheme.Scheme;
            BearerFormat = scheme.BearerFormat;
            OpenIdConnectUrl = scheme.OpenIdConnectUrl?.ToString();
            if (scheme.Flows != null) Flows = new OAuthFlows(scheme.Flows);
        }

        public SecuritySchemeType Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public ParameterLocation In { get; set; }
        public string Scheme { get; set; }
        public string BearerFormat { get; set; }
        public string OpenIdConnectUrl { get; set; }
        public OAuthFlows Flows { get; set; }

        public class OAuthFlows
        {
            public OAuthFlows(OpenApiOAuthFlows flows)
            {
                if (flows.Implicit != null) Implicit = new OAuthFlow(flows.Implicit);
                if (flows.Password != null) Password = new OAuthFlow(flows.Password);
                if (flows.ClientCredentials != null) ClientCredentials = new OAuthFlow(flows.ClientCredentials);
                if (flows.AuthorizationCode != null) AuthorizationCode = new OAuthFlow(flows.AuthorizationCode);
            }

            public OAuthFlow Implicit { get; set; }
            public OAuthFlow Password { get; set; }
            public OAuthFlow ClientCredentials { get; set; }
            public OAuthFlow AuthorizationCode { get; set; }
        }

        public class OAuthFlow
        {
            public OAuthFlow(OpenApiOAuthFlow flow)
            {
                AuthorizationUrl = flow.AuthorizationUrl.ToString();
                TokenUrl = flow.TokenUrl.ToString();
                RefreshUrl = flow.RefreshUrl.ToString();
                Scopes = flow.Scopes;
            }

            public string AuthorizationUrl { get; set; }
            public string TokenUrl { get; set; }
            public string RefreshUrl { get; set; }
            public IDictionary<string, string> Scopes { get; set; }
        }
    }
}
