namespace TYS.Library.WebAPI
{
    /// <summary>
    /// 外部サーバーアクセス　送信：Default　結果：Default(StatusCode)
    /// </summary>
    public class DeleteDefault : Delete
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="authentication">アクセス認証用設定値</param>
        /// <returns></returns>
        public static Delete Create(AuthenticationStruct? authentication = null)
        {
            var createClass = new DeleteDefault();
            createClass.AuthenticationData = authentication;
            return createClass;
        }
    }
}
