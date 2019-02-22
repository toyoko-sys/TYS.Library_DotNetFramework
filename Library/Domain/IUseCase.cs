using System.Threading.Tasks;

namespace TYS.Library.Domain.UseCase
{
    public interface IUseCase
    {
        Task<bool> Execute(UseCaseArgs args);
    }

    public static class UseCaseCash<T>
        where T : IUseCase, new()
    {
        private static IUseCase cash = new T();
        private static readonly UseCaseFunction original;
        public static UseCaseFunction func;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static UseCaseCash()
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
