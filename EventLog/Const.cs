using System;
using System.Diagnostics;

namespace EventLog
{
    /// <summary>
    /// イベントID設定用定義クラス
    /// </summary>
    /// <remarks>
    /// 使用するアプリで別途モジュールIDを定義し、クラスIDと併用する
    /// 例）モジュールID：10 + クラスID：1 = イベントID：11
    /// </remarks>
    public class Const
    {
        public const string LogSourceName = "Application";

        /// <summary>
        /// クラスID
        /// </summary>
        public enum ClassId
        {
            Controller = 1,
            UseCase,
            DataStore,
            Translater,
        }

    }
}