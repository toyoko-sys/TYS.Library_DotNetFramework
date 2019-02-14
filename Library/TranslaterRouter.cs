using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TYS.Library.Domain.Translater
{
    public delegate Task<bool> TranslaterFunction(TranslaterArgs args);

    public class TranslaterRouter
    {
        public async Task<bool> Execute<T>(TranslaterArgs args)
            where T : ITranslater, new()
        {
            return await TranslaterCash<T>.func(args);
        }
    }
}