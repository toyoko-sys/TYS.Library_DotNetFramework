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
        private static Dictionary<ClientAcceptType, HttpClient> clientList = null;

        /// <summary>
        /// HttpClient作成時の設定タイプ
        /// </summary>
        public enum ClientAcceptType
        {
            Default = 0,
            Json,
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns></returns>
        private static void Initialize()
        {
            if (clientList == null)
            {
                // タイプごとのリストを作成
                clientList = new Dictionary<ClientAcceptType, HttpClient>();
                foreach (ClientAcceptType type in Enum.GetValues(typeof(ClientAcceptType)))
                {
                    clientList.Add(type, null);
                }
            }
        }

        /// <summary>
        /// 使用できるHttpClientを取得
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpClient GetHttpClient(string url, ClientAcceptType clientType)
        {
            Initialize();

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
    }
}
