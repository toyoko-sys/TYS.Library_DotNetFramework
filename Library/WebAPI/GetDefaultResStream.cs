using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TYS.Library.WebAPI
{
    /// <summary>
    /// 外部サーバーアクセス　送信：Default　結果：Stream
    /// </summary>
    public class GetDefaultResStream : Get
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="authentication">アクセス認証用設定値</param>
        /// <returns></returns>
        public static Get Create(AuthenticationStruct? authentication = null)
        {
            var createClass = new GetDefaultResStream();
            createClass.AuthenticationData = authentication;
            return createClass;
        }

        /// <summary>
        /// 呼び出し
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public override async Task<dynamic> Call<T>(string url)
        {
            try
            {
                HttpClient client = HttpClientManager.GetHttpClient(url, HttpClientManager.ClientAcceptType.Default, AuthenticationData);
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    return stream;
                }
                else
                {
                    // トークンエラーの場合情報更新
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        ResetHttpClient(url, HttpClientManager.ClientAcceptType.Default);
                    }

                    // リトライ
                    if (RetryCount < MAX_RETRY_COUNT)
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
    }
}
