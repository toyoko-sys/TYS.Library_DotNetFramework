using System.Dynamic;

namespace TYS.Library.Controller
{
    /// <summary>
    /// Controller返却用のデータ
    /// </summary>
    public class ResponseArgs
    {
        /// <summary>
        /// UseCase結果
        /// </summary>
        public bool Result = false;

        /// <summary>
        /// 返却オブジェクト
        /// </summary>
        public dynamic Model = new ExpandoObject();
    }
}
