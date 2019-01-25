using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using TYS.Library.Domain.Repository;
using TYS.Library.Domain.Translater;
using TYS.Library.Domain.UseCase;

namespace TYS.Library
{
    public sealed class PluginManager
    {
        #region *****  *****
        private static readonly PluginManager _instance = new PluginManager();
        private TYS.Library.Domain.UseCase.UseCaseRouter UseCaseRouter;
        #endregion

        /// <summary>
        /// インスタンスを取得します。
        /// </summary>
        /// <returns></returns>
        public static PluginManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// UseCaseの実行
        /// </summary>
        /// <param name="useCaseKey"></param>
        public static async Task<bool> Execute(string useCaseKey, UseCaseArgs useCaseArgs)
        {
            return await Instance.UseCaseRouter.Execute(useCaseKey, useCaseArgs);
        }

        #region ***** private Method *****
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private PluginManager()
        {
            this.UseCaseRouter = new Domain.UseCase.UseCaseRouter()
            {
                Repository = new Domain.Repository.RepositoryRouter(),
                Translater = new Domain.Translater.TranslaterRouter()
            };

            this.LoadPluginDll();
        }

        /// <summary>
        /// 同一フォルダ内のDLLをプラグインとして読み込みます
        /// </summary>
        private void LoadPluginDll()
        {
            ArrayList plugins = new ArrayList();

            //var current = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var current = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            var dlls = Directory.GetFiles(current, "TYS*.dll");
            var IfName = typeof(IPluginCore).FullName;

            foreach (string dll in dlls)
            {
                try
                {
                    Assembly asm = Assembly.LoadFrom(dll);
                    foreach(Type t in asm.GetTypes())
                    {
                        // アセンブリチェック
                        if (t.IsClass && t.IsPublic && !t.IsAbstract && t.GetInterface(IfName) != null)
                        {
                            var pluginCore = (IPluginCore)asm.CreateInstance(t.FullName);

                            AppendUseCase(pluginCore);
                            AppendRepository(pluginCore);
                            AppendTranslater(pluginCore);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// プラグインが持つUseCaseをRouterに追加します
        /// </summary>
        /// <param name="plugin"></param>
        private void AppendUseCase(IPluginCore plugin)
        {
            var useCase = plugin.UseCaseList;
            foreach (KeyValuePair<string, UseCaseFunction> item in useCase)
            {
                UseCaseRouter.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// プラグインが持つRepositoryをRouterに追加します
        /// </summary>
        /// <param name="plugin"></param>
        private void AppendRepository(IPluginCore plugin)
        {
            var repository = plugin.RepositoryList;
            foreach (KeyValuePair<string, RepositoryFunction> item in repository)
            {
                UseCaseRouter.Repository.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// プラグインが持つTranslaterをRouterに追加します
        /// </summary>
        /// <param name="plugin"></param>
        private void AppendTranslater(IPluginCore plugin)
        {
            var translater = plugin.TranslaterList;
            foreach (KeyValuePair<string, TranslaterFunction> item in translater)
            {
                UseCaseRouter.Translater.Add(item.Key, item.Value);
            }
        }
        #endregion
    }
}
