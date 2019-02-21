using System.Dynamic;

namespace TYS.Library.Domain.Translater
{
    /// <summary>
    /// Translater実行時の引数
    /// </summary>
    public class TranslaterArgs
    {
        /// <summary>
        /// モジュール識別番号
        /// </summary>
        public int ModuleId = 0;
        /// <summary>
        /// 入力引数
        /// </summary>
        public dynamic Entity = new ExpandoObject();
        /// <summary>
        /// 出力引数
        /// </summary>
        public dynamic Model = new ExpandoObject();
    }
}
