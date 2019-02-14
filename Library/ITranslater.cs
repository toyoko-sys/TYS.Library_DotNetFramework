using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TYS.Library.Domain.Translater;

namespace TYS.Library
{
    public interface ITranslater
    {
        Task<bool> Execute(TranslaterArgs args);
    }

    public static class TranslaterCash<T>
        where T : ITranslater, new()
    {
        private static ITranslater cash = new T();
        public static TranslaterFunction func;

        static TranslaterCash()
        {
            func = cash.Execute;
        }
    }
}
