using System.Collections.Generic;
using System.Threading.Tasks;

namespace TYS.Library.Domain.Repository
{
    public delegate Task<bool> RepositoryFunction(RepositoryArgs args);

    public class RepositoryRouter : Dictionary<string, RepositoryFunction>
    {
        public async Task<bool> Execute(string key, RepositoryArgs args)
        {
            if (!this.ContainsKey(key))
            {
                return false;
            }
            return await this[key](args);
        }
    }
}