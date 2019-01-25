using System;
using System.IO;

namespace TYS.Library
{
    /// <summary>
    /// JSONファイルユーティリティクラス
    /// </summary>
    public class JsonFileUtility
    {
        /// <summary>
        /// ファイルを読み込み、JSON文字列として返す
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>JSON文字列</returns>
        public static string ReadJsonString(string filePath)
        {
            var jsonString = "";

            try
            {
                using (var sr = new StreamReader(filePath))
                {
                    jsonString = sr.ReadToEnd();
                }
            }
            catch
            {
            }

            return jsonString;
        }

        /// <summary>
        /// JSON文字列を渡し、ファイルに書き込む
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="jsonString">JSON文字列</param>
        /// <returns>書き込み結果 TRUE:成功 FALSE:失敗</returns>
        public static bool WriteJsonFile(string filePath, string jsonString)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(jsonString);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}