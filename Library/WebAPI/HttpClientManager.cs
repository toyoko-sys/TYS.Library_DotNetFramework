using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TYS.Library.WebAPI
{
    /// <summary>
    /// 外部サーバーアクセスGet用
    /// </summary>
    public static class HttpClientManager
    {
        private static Dictionary<string, int> domainList = new Dictionary<string, int>();
        private static Dictionary<ClientAcceptType, HttpClient> clientList = new Dictionary<ClientAcceptType, HttpClient>();

        /// <summary>
        /// HttpClient作成時の設定タイプ
        /// </summary>
        public enum ClientAcceptType
        {
            Default = 0,
            Json,
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <returns></returns>
        static HttpClientManager()
        {
            // タイプごとのリストを作成
            foreach (ClientAcceptType type in Enum.GetValues(typeof(ClientAcceptType)))
            {
                clientList.Add(type, null);
            }
        }

        /// <summary>
        /// 使用できるHttpClientを取得
        /// </summary>
        /// <param name="url"></param>
        /// <param name="clientType"></param>
        /// <param name="adId">Azure AD テナントのディレクトリ ID</param>
        /// <param name="resourceApplicationId">認証対象のクライアントID</param>
        /// <param name="clientApplicationId">アクセス元 AD アプリのアプリケーションID</param>
        /// <param name="secretKey">アクセス元 AD アプリで発行したキー</param>
        /// <returns></returns>
        public static HttpClient GetHttpClient(string url, ClientAcceptType clientType, string adId = null, string resourceApplicationId = null, string clientApplicationId = null, string secretKey = null)
        {
            Uri uri = new Uri(url);
            string domain = uri.GetLeftPart(UriPartial.Authority);
            // ドメインのリサイクル設定が行われているか確認
            if (!domainList.ContainsKey(domain))
            {
                // コネクションの自動リサイクル設定
                var sp = ServicePointManager.FindServicePoint(new Uri(domain));
                sp.ConnectionLeaseTimeout = 60 * 1000; // 1 minute

                lock (domainList)
                {
                    domainList.Add(domain, 0);
                }
            }

            // 同じClientTypeのHttpClientを取得、存在しなければ生成
            HttpClient client = clientList[clientType];
            if (client == null)
            {
                client = CreateHttpClient(clientType);

                if (adId != null && resourceApplicationId != null && clientApplicationId != null && secretKey != null)
                {
                    // 値が指定されていれば認証実施
                    var authHeader = GetAuthenticationHeader(adId, resourceApplicationId, clientApplicationId, secretKey);
                    client.DefaultRequestHeaders.Add("Authorization", authHeader);
                }

                clientList[clientType] = client;
            }

            return client;
        }

        /// <summary>
        /// HttpClientの生成
        /// </summary>
        /// <param name="clientType"></param>
        /// <returns></returns>
        private static HttpClient CreateHttpClient(ClientAcceptType clientType)
        {
            HttpClient client = new HttpClient();

            switch (clientType)
            {
                case ClientAcceptType.Json:
                    client.DefaultRequestHeaders.Remove("Accept");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    break;
            }

            return client;
        }

        /// <summary>
        /// Azure AD アクセス認証用ヘッダー付加情報取得
        /// </summary>
        /// <param name="adId">Azure AD テナントのディレクトリ ID</param>
        /// <param name="resourceApplicationId">認証対象のクライアントID</param>
        /// <param name="clientApplicationId">アクセス元 AD アプリのアプリケーションID</param>
        /// <param name="secretKey">アクセス元 AD アプリで発行したキー</param>
        private static string GetAuthenticationHeader(string adId, string resourceApplicationId, string clientApplicationId, string secretKey)
        {
            /* Azure AD テナントの設定 */
            // トークン発行元
            string authority = "https://login.microsoftonline.com/" + adId;

            /* トークン生成 */
            // ADAL の AuthenticationContext オブジェクト
            AuthenticationContext authContext = new AuthenticationContext(authority);

            // クライアント資格情報
            var clientCredential = new ClientCredential(clientApplicationId, secretKey);

            // トークン要求
            var authResult = authContext.AcquireTokenAsync(resourceApplicationId, clientCredential);

            // Bearer トークン
            string bearerToken = authResult.Result.CreateAuthorizationHeader();

            return bearerToken;
        }
    }
}
