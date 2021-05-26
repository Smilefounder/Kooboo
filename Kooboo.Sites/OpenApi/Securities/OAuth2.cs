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
            var result = new AuthorizeResult();

            if (string.IsNullOrWhiteSpace(data.AccessToken) || (data.ExpiresIn != default(DateTime) && DateTime.UtcNow > data.ExpiresIn))
            {
                AuthorizeClientCredentials(flow, data);
                result.ShouldSaveData = true;
            }

            Helpers.AddBearer(result, data);
            return result;
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

            if (dic.ContainsKey(HttpSender.ErrorFieldName))
            {
                throw new Exception(JsonHelper.Serialize(dic));
            }

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

        private AuthorizeResult AuthorizationCode(OpenApiOAuthFlow flow, Models.OpenApi.AuthorizeData data)
        {
            var result = new AuthorizeResult();

            if (string.IsNullOrWhiteSpace(data.AccessToken))
            {
                throw new Exception("This API need challenge authorize first");
            }

            if ((data.ExpiresIn != default(DateTime) && DateTime.UtcNow > data.ExpiresIn))
            {
                RefreshToken(flow, data);
                result.ShouldSaveData = true;
            }

            Helpers.AddBearer(result, data);
            return result;
        }

        private void RefreshToken(OpenApiOAuthFlow flow, Models.OpenApi.AuthorizeData data)
        {
            var url = flow.RefreshUrl ?? flow.TokenUrl;
            if (url == null) return;

            var token = Helpers.BasicAuthEncode(data.ClientId, data.ClientSecret);

            var headers = new Dictionary<string, string>
            {
                {"Authorization", $"Basic {token}"}
            };

            var result = HttpSender.GetSender(ContentType).Send(flow.TokenUrl.ToString(), "POST", new Dictionary<string, string>
            {
                { "grant_type","refresh_token"},
                {"refresh_token",data.RefreshToken },
                {"client_id",data.ClientId },
                {"client_secret",data.ClientSecret },
            }, headers, null);

            var dic = result as IDictionary<string, object>;

            if (dic.ContainsKey(HttpSender.ErrorFieldName))
            {
                throw new Exception(JsonHelper.Serialize(dic));
            }

            if (dic.TryGetValue("access_token", out var access_token))
            {
                data.AccessToken = access_token.ToString();
            }

            if (dic.TryGetValue("refresh_token", out var refresh_token))
            {
                data.RefreshToken = refresh_token.ToString();
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
