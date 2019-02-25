using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace TYS.Library.Controller
{
    /// <summary>
    /// APIControllerクラスの規程クラス
    /// </summary>
    public abstract class AbstractController : ApiController
    {
        /// <summary>
        /// 返却用のActionResultを作成します
        /// </summary>
        /// <param name="args">返却用データ</param>
        /// <returns>IHttpActionResult</returns>
        protected IHttpActionResult CreateDefaultResult(ResponseArgs args)
        {
            IHttpActionResult ret;

            if (args.Result)
            {
                ret = Ok(args.Model);
            }
            else
            {
                if (args.Model is HttpStatusCode code)
                {
                    ret = StatusCode(code);
                }
                else
                {
                    ret = InternalServerError();
                }
            }

            return ret;
        }

        /// <summary>
        /// 画像の返却結果を作成します
        /// </summary>
        /// <param name="args"></param>
        /// <param name="mediaType">MIMEタイプ</param>
        /// <returns></returns>
        public IHttpActionResult CreateMediaResult(ResponseArgs args, string mediaType = "image/jpeg")
        {
            IHttpActionResult ret;

            if (args.Result)
            {
                // DataStoreの時点でbyte[]で受ければロスが減るのだけど…
                Stream stream = args.Model;

                MemoryStream ms = stream as MemoryStream;
                if (ms == null)
                {
                    ms = new MemoryStream();
                    stream.CopyTo(ms);
                    stream.Dispose();
                }

                ret = new CustomMediaResult(Request, ms, mediaType);
            }
            else
            {
                if (args.Model is HttpStatusCode code)
                {
                    ret = StatusCode(code);
                }
                else
                {
                    ret = InternalServerError();
                }
            }

            return ret;
        }

        /// <summary>
        /// エラーログを書き込みます
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ex"></param>
        protected void ErrorLogWrite(int id, Exception ex)
        {
            var eventId = EventLog.EventLogWriter.GetEventId(id, EventLog.Const.ClassId.Controller);
            EventLog.EventLogWriter.Write(EventLog.Const.LogSourceName, ex, System.Diagnostics.EventLogEntryType.Error, eventId);
        }
    }
}