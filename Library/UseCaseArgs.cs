using System.Dynamic;

namespace TYS.Library.Domain.UseCase
{
    /// <summary>
    /// UseCase実行時の引数
    /// </summary>
    public class UseCaseArgs
    {
        /// <summary>
        /// RepositoryRouter
        /// </summary>
        public Repository.RepositoryRouter Repository = null;
        /// <summary>
        /// TranslaterRouter
        /// </summary>
        public Translater.TranslaterRouter Translater = null;
        /// <summary>
        /// モジュールの識別番号
        /// </summary>
        public int ModuleId = 0;
        /// <summary>
        /// 入力引数
        /// </summary>
        public dynamic Input = new ExpandoObject();
        /// <summary>
        /// 出力引数
        /// </summary>
        public dynamic Model = new ExpandoObject();
    }
}
