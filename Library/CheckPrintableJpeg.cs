using System.IO;

namespace TYS.Library
{
    /// <summary>
    /// CheckPrintableJpeg の概要の説明です
    /// </summary>
    public class CheckPrintableJpeg
    {
        /// <summary>
        /// アップロードファイル用
        /// </summary>
        public static bool Execute(Stream stream)
        {
            MemoryStream ms = new MemoryStream();
            BinaryReader binaryReader;
            bool bProgressive = false;
            bool bSOF0;
            byte NumberOfComponents = 0;

            try
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                binaryReader = new BinaryReader(ms);
            }
            catch
            {
                //Open file error
                ms.Close();
                return false;
            }
            finally
            {
                stream.Seek(0, SeekOrigin.Begin);
            }


            if (!checkSOI(binaryReader))
            {
                //Imageデータエラー
                return false;
            }
            for (; ; )
            {
                bSOF0 = ifStartOfFrame0(binaryReader, ref bProgressive);

                if (bProgressive == true)
                {
                    //       break;
                }


                if (bSOF0 == true)
                {
                    binaryReader.ReadByte();//Data precion
                    binaryReader.ReadBytes(2);
                    binaryReader.ReadBytes(2);
                    NumberOfComponents = binaryReader.ReadByte();
                    break;
                }

            }

            binaryReader.Close();
            ms.Close();

            //  if (bProgressive == false && NumberOfComponents == 3) return true;
            //  else return false;
            if (NumberOfComponents == 4) return false;
            else return true;

        }

        private static bool ifStartOfFrame0(BinaryReader binaryReader, ref bool bProgressive)
        {
            byte b1 = binaryReader.ReadByte();
            byte b2 = binaryReader.ReadByte();
            byte b3 = binaryReader.ReadByte();
            byte b4 = binaryReader.ReadByte();

            int nSize = 256 * b3 + b4;


            if ((b1 == 0xff) & (b2 == 0xc0))
            {
                return true;
            }
            else if ((b1 == 0xff) & (b2 == 0xc2))
            {
                bProgressive = true;
                return true;
            }
            else
            {
                binaryReader.BaseStream.Seek(nSize - 2, SeekOrigin.Current);
                return false;
            }

        }

        private static bool checkSOI(BinaryReader binaryReader)
        {
            byte b1 = 0;
            byte b2 = 0;

            try
            {
                b1 = binaryReader.ReadByte();
                b2 = binaryReader.ReadByte();
            }
            catch
            {
                return false;
            }

            if ((b1 == 0xff) & (b2 == 0xd8)) return true;
            else return false;

        }
    }
}