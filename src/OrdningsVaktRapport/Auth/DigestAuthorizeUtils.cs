using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace OrdningsVaktRapport.Auth
{
    public class DigestAuthorizeUtils
    {
        private static readonly ConcurrentDictionary<string, NonceModel> Nonces = new ConcurrentDictionary<string, NonceModel>();

        public static string GenerateNonce()
        {
            var bytes = new byte[16];
            var rngProvider = new RNGCryptoServiceProvider();
            rngProvider.GetBytes(bytes);
            var hash = GenerateMd5Hash(bytes);
            Nonces.TryAdd(hash, new NonceModel { NonceCount = 0, NonceExpirationDate = DateTime.Now.AddHours(1)});
            return hash;
        }

        public static bool CheckNonce(string nonce, string nonceCount)
        {
            ClearUnsedNonces();

            try
            {
                var currentNonce = Nonces[nonce];

                if (currentNonce != null)
                {
                    if (Int32.Parse(nonceCount) > currentNonce.NonceCount)
                    {
                        if (currentNonce.NonceExpirationDate > DateTime.Now)
                        {
                            Nonces[nonce] = new NonceModel { NonceCount = currentNonce.NonceCount + 1, NonceExpirationDate = currentNonce.NonceExpirationDate };
                            return true;
                        }
                    }
                }

            }
            catch (Exception)
            {

                return false;
            }

            return false;
        }

        public static void ClearUnsedNonces()
        {
            foreach (var item in Nonces)
            {
                if (DateTime.Now > item.Value.NonceExpirationDate)
                {
                    NonceModel nonceToRemove;
                    Nonces.TryRemove(item.Key, out nonceToRemove);
                }
            }
        }

        public static bool RemoveNonce(string nonce)
        {
            NonceModel nonceToRemove;
            return Nonces.TryRemove(nonce, out nonceToRemove);
        }

        public static string ConvertStringToMd5Hash(string hashString)
        {
            return GenerateMd5Hash(Encoding.UTF8.GetBytes(hashString));
        }

        public static HeaderModel ExtractHeaderValues(string headers, string method)
        {
            var newHeaders = headers.Replace("\"", "");
            var splitHeaders = newHeaders.Split(',');
            var values = new Dictionary<string, string>();

            foreach (var item in splitHeaders)
            {
                var splitItems = item.Split('=');
                var key = splitItems[0].Trim();
                var value = splitItems[1].Trim();
                values.Add(key, value);
            }

            if(!values.ContainsKey("method")) values.Add("method", method);

            return new HeaderModel
                {
                    Nonce = values["nonce"],
                    CNonce = values["cnonce"],
                    NonceCount = values["nc"],
                    Method = values["method"],
                    Realm = values["realm"],
                    Response = values["response"],
                    Uri = values["uri"],
                    UserName = values["username"],
                    QoP = values["qop"]
                };
        }

        public static string GenerateMd5Hash(byte[] bytes)
        {
            var hash = new StringBuilder();
            var md5 = MD5.Create();
            md5.ComputeHash(bytes).ToList().ForEach(b => hash.AppendFormat("{0:x2}", b));
            return hash.ToString();
        }
    }
}