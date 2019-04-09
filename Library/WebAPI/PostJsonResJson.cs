using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TYS.Library.WebAPI
{
    /// <summary>
    /// 外部サーバーアクセス　送信：Json　結果：Json
    /// </summary>
    public class PostJsonResJson : Post
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="authentication">アクセス認証用設定値</param>
        /// <returns></returns>
        public static Post Create(AuthenticationStruct? authentication = null)
        {
            var createClass = new PostJsonResJson();
            createClass.AuthenticationData = authentication;
            return createClass;
        }

        /// <summary>
        /// 呼び出し
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public override async Task<dynamic> Call<T>(string url, HttpContent content)
        {
            try
            {
                HttpClient client = HttpClientManager.GetHttpClient(url, HttpClientManager.ClientAcceptType.Json, AuthenticationData);
                HttpResponseMessage response = await client.PostAsync(url, content);
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
                        ResetHttpClient(url, HttpClientManager.ClientAcceptType.Json);
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
