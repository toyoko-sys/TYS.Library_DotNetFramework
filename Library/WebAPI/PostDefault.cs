namespace TYS.Library.WebAPI
{
    /// <summary>
    /// 外部サーバーアクセス　送信：Default　結果：Json
    /// </summary>
    public class PostDefault : Post
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="authentication">アクセス認証用設定値</param>
        /// <returns></returns>
        public static Post Create(AuthenticationStruct? authentication = null)
        {
            var createClass = new PostDefault();
            createClass.AuthenticationData = authentication;
            return createClass;
        }
    }
}
