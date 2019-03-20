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
        private static int[] EXIF_USING_ID_FOR_IAGENT = {
            EXIF_IMAGE_WIDTH,
            EXIF_IMAGE_HEIGHT,
            0x010f, /* モデル名 */
            EXIF_ORIENTATION,
            0x8827, /* ISOスピードレート */
            0x829a, /* 露出時間 */
            0x829d, /* Fナンバー */
            0x9003, /* データ生成日時 */
            0x9201, /* シャッタースピード */
            0x9202, /* 絞り値 */
            0x9203, /* 輝度値 */
            0x9206, /* 被写体距離 */
            0x9209, /* フラッシュ */
            0xa002, /* 実行画像幅(横方向のピクセル数) */
            0xa003, /* 実行画像高さ（高さ方向のピクセル数） */
            0xa402, /* 露出モード */
            0xa403, /* ホワイトバランス */
            0xa406  /* 撮影シーンタイプ */
        };

        private const int EXIF_IMAGE_WIDTH = 0xA002;
        private const int EXIF_IMAGE_HEIGHT = 0xA003;
        private const int EXIF_ORIENTATION = 0x0112;

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
            int index = Array.IndexOf(pils, EXIF_ORIENTATION);

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
            int index = Array.IndexOf(pils, EXIF_ORIENTATION);

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
                    // 上下反転(上下鏡像?)
                    rotation = 0;
                    break;
                case 3:
                    // 180度回転 
                    rotation = 180;
                    break;
                case 4:
                    // 左右反転
                    rotation = 0;
                    break;
                case 5:
                    // 上下反転、時計周りに270度回転
                    rotation = 270;
                    break;
                case 6:
                    // 時計周りに90度回転
                    rotation = 90;
                    break;
                case 7:
                    // 上下反転、時計周りに90度回転
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
