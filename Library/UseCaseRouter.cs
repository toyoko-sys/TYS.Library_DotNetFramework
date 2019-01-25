using System.Collections.Generic;
using System.Threading.Tasks;

namespace TYS.Library.Domain.UseCase
{
    public delegate Task<bool> UseCaseFunction(UseCaseArgs args);

    public class UseCaseRouter : Dictionary<string, UseCaseFunction>
    {
        public Domain.Repository.RepositoryRouter Repository { get; set; } = null;
        public Domain.Translater.TranslaterRouter Translater { get; set; } = null;

        public async Task<bool> Execute(string key, UseCaseArgs args)
        {
            if(!this.ContainsKey(key))
            {
                return false;
            }
            if(this.Repository == null)
            {
                return false;
            }
            if(this.Translater == null)
            {
                return false;
            }
            args.Repository = this.Repository;
            args.Translater = this.Translater;
            return await this[key](args);
        }
    }
}