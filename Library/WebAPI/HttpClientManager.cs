using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace TYS.Library.WebAPI
{
    /// <summary>
    /// 外部サーバーアクセスGet用
    /// </summary>
    public static class HttpClientManager
    {
        private static Dictionary<string, int> domainList = new Dictionary<string, int>();
        private static Dictionary<string, Dictionary<ClientAcceptType, HttpClient>> clientList = new Dictionary<string, Dictionary<ClientAcceptType, HttpClient>>();

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
        }

        /// <summary>
        /// 使用できるHttpClientを取得
        /// </summary>
        /// <param name="url"></param>
        /// <param name="clientType"></param>
        /// <param name="authenticationData">アクセス認証用設定値</param>
        /// <returns></returns>
        public static HttpClient GetHttpClient(string url, ClientAcceptType clientType, AuthenticationStruct? authenticationData = null)
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

                lock (clientList)
                {
                    // ドメイン・タイプごとのリストを作成
                    clientList.Add(domain, new Dictionary<ClientAcceptType, HttpClient>());
                    foreach (ClientAcceptType type in Enum.GetValues(typeof(ClientAcceptType)))
                    {
                        clientList[domain].Add(type, null);
                    }
                }
            }

            // 同じドメイン・ClientTypeのHttpClientを取得、存在しなければ生成
            HttpClient client = clientList[domain][clientType];
            if (client == null)
            {
                client = CreateHttpClient(clientType);

                if (authenticationData.HasValue)
                {
                    // 値が指定されていれば認証実施
                    var authHeader = GetAuthenticationHeader(authenticationData.Value);
                    client.DefaultRequestHeaders.Add("Authorization", authHeader);
                }

                clientList[domain][clientType] = client;
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
        /// <param name="authenticationData">アクセス認証用設定値</param>
        private static string GetAuthenticationHeader(AuthenticationStruct authenticationData)
        {
            /* Azure AD テナントの設定 */
            // トークン発行元
            string authority = "https://login.microsoftonline.com/" + authenticationData.AdId;

            /* トークン生成 */
            // ADAL の AuthenticationContext オブジェクト
            var authContext = new AuthenticationContext(authority);

            // クライアント資格情報
            var clientCredential = new ClientCredential(authenticationData.ClientApplicationId, authenticationData.SecretKey);

            // トークン要求
            var authResult = authContext.AcquireTokenAsync(authenticationData.ResourceApplicationId, clientCredential);

            // Bearer トークン
            string bearerToken = authResult.Result.CreateAuthorizationHeader();

            return bearerToken;
        }
    }
}
