using System;
using System.Threading.Tasks;
using TYS.Library.Domain.UseCase;

namespace TYS.Library
{
    public interface IUseCase
    {
        Task<bool> Execute(UseCaseArgs args);
    }

    public static class UseCaseCash<T>
        where T : IUseCase, new()
    {
        private static IUseCase cash = new T();
        public static UseCaseFunction func;

        static UseCaseCash()
        {
            func = cash.Execute;
        }
    }
}
