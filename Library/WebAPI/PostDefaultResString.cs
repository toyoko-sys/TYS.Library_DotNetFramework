using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace TYS.Library.WebAPI
{
    /// <summary>
    /// 外部サーバーアクセス　送信：Default　結果：String
    /// </summary>
    public class PostDefaultResString : Post
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="authentication">アクセス認証用設定値</param>
        /// <returns></returns>
        public static Post Create(AuthenticationStruct? authentication = null)
        {
            var createClass = new PostDefaultResString();
            createClass.AuthenticationData = authentication;
            return createClass;
        }

        /// <summary>
        /// 結果取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        protected override async Task<T> GetResponseData<T>(HttpResponseMessage response)
        {
            T responseData;
            var responseStream = await response.Content.ReadAsStringAsync();
            responseData = JsonConvert.DeserializeObject<T>(responseStream);
            return responseData;
        }
    }
}
