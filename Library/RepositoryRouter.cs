using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TYS.Library.Domain.Repository
{
    public delegate Task<bool> RepositoryFunction(RepositoryArgs args);

    public class RepositoryRouter
    {
        public async Task<bool> Execute<T>(RepositoryArgs args)
            where T : IRepository, new()
        {
            return await RepositoryCash<T>.func(args);
        }
    }
}