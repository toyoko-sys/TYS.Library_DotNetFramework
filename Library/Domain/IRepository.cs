using System.Threading.Tasks;

namespace TYS.Library.Domain.Repository
{
    public interface IRepository
    {
        Task<bool> Execute(RepositoryArgs args);
    }

    public static class RepositoryCash<T>
        where T : IRepository, new()
    {
        private static IRepository cash = new T();
        private static readonly RepositoryFunction original;
        public static RepositoryFunction func;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static RepositoryCash()
        {
            func = cash.Execute;
            original = cash.Execute;
        }

        /// <summary>
        /// funcを書き換えた際にリセットする
        /// </summary>
        public static void ResetFunc()
        {
            func = original;
        }
    }
}
