using System;
using System.Threading.Tasks;
using TYS.Library.Domain.Repository;

namespace TYS.Library
{
    public interface IRepository
    {
        Task<bool> Execute(RepositoryArgs args);
    }

    public static class RepositoryCash<T>
        where T : IRepository, new()
    {
        private static IRepository cash = new T();
        public static RepositoryFunction func;

        static RepositoryCash()
        {
            func = cash.Execute;
        }
    }
}
