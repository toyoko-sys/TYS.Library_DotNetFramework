using System;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace TYS.Library.WebAPI
{
    /// <summary>
    /// 外部サーバーアクセスPost用
    /// </summary>
    public abstract class Post
    {
        // 認証実施時に設定
        // Azure AD テナントのディレクトリ ID
        protected string AdId = null;
        // 認証対象のクライアントID
        protected string ResourceApplicationId = null;
        // アクセス元 AD アプリのアプリケーションID
        protected string ClientApplicationId = null;
        // アクセス元 AD アプリで発行したキー
        protected string SecretKey = null;

        /// <summary>
        /// 呼び出し
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual async Task<dynamic> Call<T>(string url, HttpContent content)
        {
            try
            {
                HttpClient client = HttpClientManager.GetHttpClient(url, HttpClientManager.ClientAcceptType.Default, AdId, ResourceApplicationId, ClientApplicationId, SecretKey);
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    dynamic responseContent = await this.GetResponseData<T>(response);
                    return responseContent;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 結果取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        protected virtual async Task<T> GetResponseData<T>(HttpResponseMessage response)
        {
            T responseData = default(T);
            if (response.Content.Headers.ContentLength > 0)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var type = typeof(T);
                    var serializer = new DataContractJsonSerializer(type);
                    responseData = (T)serializer.ReadObject(responseStream);
                }
            }
            return responseData;
        }
    }
}
