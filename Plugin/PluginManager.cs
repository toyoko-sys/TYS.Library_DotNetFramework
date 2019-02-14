using System.Threading.Tasks;
using TYS.Library.Domain.UseCase;

namespace TYS.Library
{
    public sealed class PluginManager
    {
        #region *****  *****
        private static readonly PluginManager _instance = new PluginManager();
        private TYS.Library.Domain.UseCase.UseCaseRouter UseCaseRouter;
        #endregion

        /// <summary>
        /// インスタンスを取得します。
        /// </summary>
        /// <returns></returns>
        public static PluginManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// UseCaseの実行
        /// </summary>
        /// <param name="useCaseKey"></param>
        public static async Task<bool> Execute<T>(UseCaseArgs useCaseArgs)
            where T : IUseCase, new()
        {
            return await Instance.UseCaseRouter.Execute<T>(useCaseArgs);
        }

        #region ***** private Method *****
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private PluginManager()
        {
            this.UseCaseRouter = new Domain.UseCase.UseCaseRouter()
            {
                Repository = new Domain.Repository.RepositoryRouter(),
                Translater = new Domain.Translater.TranslaterRouter()
            };
        }
        #endregion
    }
}
