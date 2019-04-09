namespace TYS.Library.WebAPI
{
    /// <summary>
    /// 外部サーバーアクセス　送信：Default　結果：Json
    /// </summary>
    public class PutDefault : Put
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="authentication">アクセス認証用設定値</param>
        /// <returns></returns>
        public static Put Create(AuthenticationStruct? authentication = null)
        {
            var createClass = new PutDefault();
            createClass.AuthenticationData = authentication;
            return createClass;
        }
    }
}
