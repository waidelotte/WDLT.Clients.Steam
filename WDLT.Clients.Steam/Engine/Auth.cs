using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using WDLT.Clients.Steam.Exceptions;
using WDLT.Clients.Steam.Models.Json;

namespace WDLT.Clients.Steam.Engine
{
    public class Auth
    {
        public CookieCollection CookieCollection { get; private set; }

        private readonly SteamClient _client;

        public Auth(SteamClient client)
        {
            _client = client;
        }

        public void Reset()
        {
            CookieCollection = null;
        }

        public string Session()
        {
            if (CookieCollection != null)
            {
                var id = CookieCollection["sessionid"]?.Value;

                if (!string.IsNullOrEmpty(id))
                {
                    return id;
                }
            }

            throw new SteamException("Cannot find Session in Cookies");
        }

        public void SetCookies(IEnumerable<KeyValuePair<string, string>> cookies)
        {
            CookieCollection ??= new CookieCollection();

            foreach (var item in cookies)
            {
                var cookie = new Cookie(item.Key, item.Value);
                var exist = CookieCollection.FirstOrDefault(f => f.Name == cookie.Name);

                if (exist == null)
                {
                    CookieCollection.Add(cookie);
                }
                else
                {
                    exist.Value = cookie.Value;
                }
            }
        }

        public void SetCookies(IEnumerable<RestResponseCookie> cookies)
        {
            SetCookies(cookies.Select(s => new KeyValuePair<string, string>(s.Name,s.Value)));
        }

        public async Task<JLogin> LoginAsync(string login, string password, string captchaGID = null, string captchaUserText = null, string steamGuard = null, string emailID = null)
        {
            var rsa = await RSAAsync(login);

            var encPass = EncryptPassword(password, rsa.Module, rsa.Exponent);

            var request = new RestRequest(SteamEndpoints.DO_LOGIN, Method.POST);
            request.AddParameter("password", encPass);
            request.AddParameter("username", login);
            request.AddParameter("rsatimestamp", rsa.Timestamp);
            request.AddParameter("remember_login", true.ToString());
            request.AddParameter("loginfriendlyname", "");
            request.AddParameter("l", "en");

            if (!string.IsNullOrEmpty(captchaGID) && !string.IsNullOrEmpty(captchaUserText))
            {
                request.AddParameter("captchagid", captchaGID);
                request.AddParameter("captcha_text", captchaUserText);
            }

            if (!string.IsNullOrEmpty(steamGuard) && !string.IsNullOrEmpty(emailID))
            {
                request.AddParameter("emailsteamid", emailID);
                request.AddParameter("emailauth", steamGuard);
            }
            else if (!string.IsNullOrEmpty(steamGuard))
            {
                request.AddParameter("twofactorcode", steamGuard);
            }

            var response = await _client.SteamRequestAsync<JLogin>(request, true);

            if (response.Success)
            {
                // Need for update Session ID
                await _client.SteamRequestRawAsync(new RestRequest("/market"), true);
            }

            return response;
        }

        private Task<JRSA> RSAAsync(string login)
        {
            var request = new RestRequest(SteamEndpoints.RSA, Method.POST);
            request.AddParameter("username", login);
            return _client.SteamRequestAsync<JRSA>(request, false);
        }

        private static string EncryptPassword(string password, string modval, string expval)
        {
            var rsa = new RSACryptoServiceProvider();
            var rsaParams = new RSAParameters
            {
                Modulus = StringToByteArray(modval),
                Exponent = StringToByteArray(expval)
            };

            rsa.ImportParameters(rsaParams);
            var encodedPass = rsa.Encrypt(Encoding.ASCII.GetBytes(password), false);

            return Convert.ToBase64String(encodedPass);
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }
    }
}