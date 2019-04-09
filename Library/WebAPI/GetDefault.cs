namespace TYS.Library.WebAPI
{
    /// <summary>
    /// 外部サーバーアクセス　送信：Default　結果：Default
    /// </summary>
    public class GetDefault : Get
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="authentication">アクセス認証用設定値</param>
        /// <returns></returns>
        public static Get Create(AuthenticationStruct? authentication = null)
        {
            var createClass = new GetDefault();
            createClass.AuthenticationData = authentication;
            return createClass;
        }
    }
}
