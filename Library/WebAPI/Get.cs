using System;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace TYS.Library.WebAPI
{
    /// <summary>
    /// 外部サーバーアクセスGet用
    /// </summary>
    public abstract class Get
    {
        // 認証設定値
        protected AuthenticationStruct? AuthenticationData = null;
        // リトライ回数
        protected const int MAX_RETRY_COUNT = 5;
        protected int RetryCount = 0;
        protected readonly TimeSpan delay = TimeSpan.FromSeconds(5);

        /// <summary>
        /// 呼び出し
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<dynamic> Call<T>(string url)
        {
            try
            {
                HttpClient client = HttpClientManager.GetHttpClient(url, HttpClientManager.ClientAcceptType.Default, AuthenticationData);
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    dynamic responseContent = await this.GetResponseData<T>(response);
                    return responseContent;
                }
                else
                {
                    // トークンエラーの場合情報更新
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        ResetHttpClient(url, HttpClientManager.ClientAcceptType.Default);
                    }

                    // リトライ
                    if(RetryCount < MAX_RETRY_COUNT)
                    {
                        RetryCount++;
                        return await Call<T>(url);
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        /// HttpClient設定を再設定
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        protected async void ResetHttpClient(string url, HttpClientManager.ClientAcceptType type)
        {
            HttpClientManager.UpdateAuthorizationHeader(url, type, AuthenticationData);
            await Task.Delay(delay);
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
