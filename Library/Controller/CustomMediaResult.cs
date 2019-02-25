using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace TYS.Library.Controller
{
    /// <summary>
    /// 画像系を返却するActionResultクラス
    /// </summary>
    public class CustomMediaResult : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly MemoryStream _stream;
        private readonly string _mediaType;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="request"></param>
        /// <param name="stream"></param>
        /// <param name="mediaType"></param>
        public CustomMediaResult(HttpRequestMessage request, MemoryStream stream, string mediaType)
        {
            _request = request;
            _stream = stream;
            _mediaType = mediaType;

        }

        /// <summary>
        /// Responseメッセージ作成
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => {
                var content = new ByteArrayContent(_stream.ToArray());
                content.Headers.ContentType = MediaTypeHeaderValue.Parse(_mediaType);

                var res = _request.CreateResponse();
                res.Content = content;

                return res;
            });
        }
    }
}
