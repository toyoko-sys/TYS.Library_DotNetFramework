﻿using System;
using System.Diagnostics;
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
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
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
            catch (Exception ex)
            {
                throw ex;
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
            T compositeImageIds;
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var type = typeof(T);
                var serializer = new DataContractJsonSerializer(type);
                compositeImageIds = (T)serializer.ReadObject(responseStream);
            }
            return compositeImageIds;
        }
    }
}
