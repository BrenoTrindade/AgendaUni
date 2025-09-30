using System.Security.Claims;
using System.Text.Json;

namespace AgendaUni.Web.Services.Authentication
{
    public class JwtHelper
    {
        public IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());
                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }
                keyValuePairs.Remove(ClaimTypes.Role);
            }

            if (keyValuePairs.TryGetValue("unique_name", out object uniqueName) && uniqueName != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, uniqueName.ToString()));
                keyValuePairs.Remove("unique_name");
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }

        public DateTime? GetTokenExpiration(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            try
            {
                var payload = token.Split('.')[1];
                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }

                var bytes = Convert.FromBase64String(payload);
                var json = System.Text.Encoding.UTF8.GetString(bytes);
                var doc = System.Text.Json.JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("exp", out var exp))
                {
                    var dateTime = DateTimeOffset.FromUnixTimeSeconds(exp.GetInt64()).UtcDateTime;
                    return dateTime;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
