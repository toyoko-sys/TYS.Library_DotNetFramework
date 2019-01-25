using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TYS.Library.Domain.Repository;
using TYS.Library.Domain.Translater;
using TYS.Library.Domain.UseCase;

namespace TYS.Library
{
    /// <summary>
    /// プラグイン作成用のベースI/F
    /// </summary>
    public interface IPluginCore
    { 
        Dictionary<string, UseCaseFunction> UseCaseList { get; }

        Dictionary<string, RepositoryFunction> RepositoryList { get; }

        Dictionary<string, TranslaterFunction> TranslaterList { get; }
    }
}
