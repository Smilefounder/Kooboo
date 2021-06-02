using Kooboo.Lib.Helper;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Validations;
using Microsoft.OpenApi.Writers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Kooboo.Sites.OpenApi
{
    public static class Helpers
    {
        public static string StandardName(string name)
        {
            name = Regex.Replace(name, "[^\\w]", "_");
            if (Regex.Match(name, "^\\d+").Success) name = "_" + name;
            return name;
        }

        public static string StandardPath(string name, OperationType type)
        {
            return $"{StandardName(name)}_{type}";
        }

        public static string BasicAuthEncode(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new Exception($"Http basic auth username can not empty");
            if (string.IsNullOrWhiteSpace(password)) throw new Exception($"Http basic auth password can not empty");
            var plainTextBytes = Encoding.UTF8.GetBytes($"{username}:{password}");
            return Convert.ToBase64String(plainTextBytes);
        }

        public static void AddBearer(Security.AuthorizeResult result, Models.OpenApi.AuthorizeData data)
        {
            var bearer = string.IsNullOrWhiteSpace(data.Name) ? "Bearer" : data.Name;
            result.AddHeader("Authorization", $"{bearer} {data.AccessToken}");
        }
    }
}
