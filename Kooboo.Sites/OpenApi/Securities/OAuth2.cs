using Kooboo.Lib.Helper;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.OpenApi.Securities
{
    public class OAuth2 : Security
    {
        public override SecuritySchemeType Type => SecuritySchemeType.OAuth2;
        public static string ContentType => "application/x-www-form-urlencoded";

        public override AuthorizeResult Authorize(OpenApiSecurityScheme scheme, Models.OpenApi.AuthorizeData data)
        {
            if (scheme.Flows.ClientCredentials != null) return ClientCredentials(scheme.Flows.ClientCredentials, data);
            if (scheme.Flows.AuthorizationCode != null) return AuthorizationCode(scheme.Flows.AuthorizationCode, data);
            throw new Exception($"Not support OAuth2 Flow password or implicit");
        }

        private AuthorizeResult ClientCredentials(OpenApiOAuthFlow flow, Models.OpenApi.AuthorizeData data)
        {
            if (string.IsNullOrWhiteSpace(data.AccessToken) || DateTime.UtcNow > data.ExpiresIn)
            {
                AuthorizeClientCredentials(flow, data);
            }

            return GetAuthorizeResult(data);
        }

        private void AuthorizeClientCredentials(OpenApiOAuthFlow flow, Models.OpenApi.AuthorizeData data)
        {
            var body = new Dictionary<string, object>
            {
                {"grant_type","client_credentials" },
                {"client_id",data.ClientId },
                {"client_secret",data.ClientSecret },
            };

            if (flow.Scopes != null)
            {
                body.Add("scope", string.Join(" ", flow.Scopes.Select(s => s.Key)));
            }

            var token = Helpers.BasicAuthEncode(data.ClientId, data.ClientSecret);

            var headers = new Dictionary<string, string>
            {
                {"Authorization", $"Basic {token}"}
            };

            var result = HttpSender.GetSender(ContentType).Send(flow.TokenUrl.ToString(), "POST", body, headers, null);
            var dic = result as IDictionary<string, object>;

            if (dic.TryGetValue("access_token", out var access_token))
            {
                data.AccessToken = access_token.ToString();
            }

            if (dic.TryGetValue("token_type", out var token_type))
            {
                data.TokenType = token_type.ToString();
            }

            if (dic.TryGetValue("expires_in", out var expires_in))
            {
                data.ExpiresIn = DateTime.UtcNow.AddSeconds(Convert.ToInt32(expires_in));
            }
        }

        private AuthorizeResult GetAuthorizeResult(Models.OpenApi.AuthorizeData data)
        {
            var result = new AuthorizeResult();
            result.AddHeader("Authorization", $"Bearer {data.AccessToken}");
            return result;
        }

        private AuthorizeResult AuthorizationCode(OpenApiOAuthFlow authorizationCode, Models.OpenApi.AuthorizeData data)
        {
            throw new NotImplementedException();
        }

        public class Result
        {
            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public int expires_in { get; set; }
            public string scope { get; set; }
            public string token_type { get; set; }
        }
    }
}
