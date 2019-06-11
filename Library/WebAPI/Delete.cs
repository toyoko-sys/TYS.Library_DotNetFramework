using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TYS.Library.WebAPI
{
    /// <summary>
    /// 外部サーバーアクセスDelete用
    /// </summary>
    public abstract class Delete
    {
        // 認証設定値
        protected AuthenticationStruct? AuthenticationData = null;
        // リトライ回数
        protected const int MAX_RETRY_COUNT = 5;
        protected int RetryCount = 0;
        private readonly TimeSpan delay = TimeSpan.FromSeconds(5);

        /// <summary>
        /// 呼び出し
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual async Task<dynamic> Call<T>(string url)
        {
            try
            {
                HttpClient client = HttpClientManager.GetHttpClient(url, HttpClientManager.ClientAcceptType.Default, AuthenticationData);
                HttpResponseMessage response = await client.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    // 結果データなし
                }
                else
                {
                    // トークンエラーの場合情報更新
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await ResetHttpClient(url, HttpClientManager.ClientAcceptType.Default);
                    }

                    // リトライ
                    if (RetryCount < MAX_RETRY_COUNT)
                    {
                        RetryCount++;
                        return await Call<T>(url);
                    }
                }

                return response.StatusCode;
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
        protected async Task<bool> ResetHttpClient(string url, HttpClientManager.ClientAcceptType type)
        {
            HttpClientManager.UpdateAuthorizationHeader(url, type, AuthenticationData);
            await Task.Delay(delay);
            return false;
        }
    }
}
