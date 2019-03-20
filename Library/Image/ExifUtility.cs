using System;
using System.Diagnostics;
using System.Drawing;

namespace TYS.Library.Image
{
    /// <summary>
    /// Exif操作クラス
    /// </summary>
    public class ExifUtility
    {
        /// <summary>
        /// Exif情報から回転情報を取得
        /// </summary>
        /// <param name="imgBmp">ビットマップクラス</param>
        /// <remarks>
        /// 1 不要(回転・反転なし)
        /// 2 水平方向に反転
        /// 3 時計回りに180度回転
        /// 4 垂直方向に反転
        /// 5 水平方向に反転 + 時計回りに270度回転
        /// 6 時計回りに90度回転
        /// 7 水平方向に反転 + 時計回りに90度回転
        /// 8 時計回りに270度回転
        /// </remarks>
        /// <returns></returns>
        public static bool GetExifOrientation(int moduleId, Bitmap imgBmp, out short orientation)
        {
            // 初期値は「1 不要(回転・反転なし)」とする
            orientation = 1;
            bool ret = false;

            int[] pils = imgBmp.PropertyIdList;

            //処理速度向上のため、PropertyIdListが3未満はJFIFとする
            if (pils.Length < 3) return ret;

            // Exif情報から回転情報を取得する(0x0112：回転情報)
            int index = Array.IndexOf(pils, (int)Exif.Orientation);

            try
            {
                if (index >= 0)
                {
                    System.Drawing.Imaging.PropertyItem pi = imgBmp.PropertyItems[index];
                    if (pi.Len == 2)
                    {
                        orientation = (short)pi.Value[0];
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.EventLogWriter.Write(EventLog.Const.LogSourceName, ex, EventLogEntryType.Error, moduleId, EventLog.Const.ClassId.DataStore);
            }
            return ret;
        }

        /// <summary>
        /// Exif情報に回転情報を保存
        /// </summary>
        /// <param name="imgBmp"></param>
        /// <param name="orientation"></param>
        public static void SetExifOrientation(int moduleId, Bitmap imgBmp, short orientation)
        {
            int[] pils = imgBmp.PropertyIdList;

            //処理速度向上のため、PropertyIdListが3未満はJFIFとする
            if (pils.Length < 3) return;

            // Exif情報から回転情報を取得する(0x0112：回転情報)
            int index = Array.IndexOf(pils, (int)Exif.Orientation);

            try
            {
                if (index >= 0)
                {
                    System.Drawing.Imaging.PropertyItem pi = imgBmp.PropertyItems[index];
                    if (pi.Len == 2)
                    {
                        pi.Value = BitConverter.GetBytes(orientation);
                        imgBmp.SetPropertyItem(pi);　//これを呼ばないと設定されないので要注意
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.EventLogWriter.Write(EventLog.Const.LogSourceName, ex, EventLogEntryType.Error, moduleId, EventLog.Const.ClassId.DataStore);
            }
        }

        /// <summary>
        /// Orientationによる補正回転角を取得
        /// </summary>
        /// <param name="orientation">Exifのorientation情報</param>
        /// <remarks>
        /// 1 不要(回転・反転なし)
        /// 2 水平方向に反転
        /// 3 時計回りに180度回転
        /// 4 垂直方向に反転
        /// 5 水平方向に反転 + 時計回りに270度回転
        /// 6 時計回りに90度回転
        /// 7 水平方向に反転 + 時計回りに90度回転
        /// 8 時計回りに270度回転
        /// </remarks>
        /// <returns>
        /// 角度(°)
        /// </returns>
        public static short GetRotaionOrientated(short orientation)
        {
            short rotation = 0;

            switch (orientation)
            {
                case 2:
                    // 水平反転（水平反転は回転で補正出来ないのでそのまま）
                    rotation = 0;
                    break;
                case 3:
                    // 180度回転 
                    rotation = 180;
                    break;
                case 4:
                    // 垂直反転（垂直反転は回転で補正出来ないのでそのまま）
                    rotation = 0;
                    break;
                case 5:
                    // 水平反転、時計周りに270度回転（水平反転は回転で補正出来ないので2の状態にする）
                    rotation = 270;
                    break;
                case 6:
                    // 時計周りに90度回転
                    rotation = 90;
                    break;
                case 7:
                    // 垂直反転、時計周りに90度回転（垂直判定は回転で補正出来ないので4の状態にする）
                    rotation = 90;
                    break;
                case 8:
                    // 時計周りに270度回転
                    rotation = 270;
                    break;
                default:
                    rotation = 0;
                    break;
            }

            return rotation;
        }

        /// <summary>
        /// Exifから回転処理を行う
        /// </summary>
        /// <param name="imgBmp"></param>
        /// <param name="rotation"></param>
        public static void RotateBitmapImage(Bitmap imgBmp, int rotation)
        {
            switch (rotation)
            {
                case 90:
                    imgBmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 180:
                    imgBmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case 270:
                    imgBmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                default:
                    break;
            }
        }
    }
}
