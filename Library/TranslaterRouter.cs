using System.Collections.Generic;
using System.Threading.Tasks;

namespace TYS.Library.Domain.Translater
{
    public delegate Task<bool> TranslaterFunction(TranslaterArgs args);

    public class TranslaterRouter : Dictionary<string, TranslaterFunction>
    {
        public async Task<bool> Execute(string key, TranslaterArgs args)
        {
            if (!this.ContainsKey(key))
            {
                return false;
            }
            return await this[key](args);
        }
    }
}