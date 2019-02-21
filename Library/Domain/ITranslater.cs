using System.Threading.Tasks;

namespace TYS.Library.Domain.Translater
{
    public interface ITranslater
    {
        Task<bool> Execute(TranslaterArgs args);
    }

    public static class TranslaterCash<T>
        where T : ITranslater, new()
    {
        private static ITranslater cash = new T();
        private static readonly TranslaterFunction original;
        public static TranslaterFunction func;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static TranslaterCash()
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
