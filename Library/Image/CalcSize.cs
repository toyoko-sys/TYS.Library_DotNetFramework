namespace TYS.Library.Image
{
    public class CalcSize
    {
        /// <summary>
        /// 長辺に合わせてリサイズ
        /// </summary>
        /// <param name="longSide"></param>
        /// <param name="orgWidth"></param>
        /// <param name="orgHeight"></param>
        /// <returns></returns>
        public static (int Width, int Height) ToFitLongSide(int longSide, int orgWidth, int orgHeight)
        {
            int resizeWidth = 0;
            int resizeHeight = 0;
            if ((orgWidth > orgHeight ? orgWidth : orgHeight) <= longSide)
            {
                resizeWidth = orgWidth;
                resizeHeight = orgHeight;
            }
            else
            {
                if (orgWidth > orgHeight)
                {
                    resizeWidth = longSide;
                    resizeHeight = (int)((double)orgHeight * ((double)longSide / (double)orgWidth));
                }
                else if (orgHeight > orgWidth)
                {
                    resizeWidth = (int)((double)orgWidth * ((double)longSide / (double)orgHeight));
                    resizeHeight = longSide;
                }
                else
                {
                    resizeWidth = longSide;
                    resizeHeight = longSide;
                }
            }
            return (resizeWidth, resizeHeight);
        }

        /// <summary>
        /// 短辺に合わせてリサイズ
        /// </summary>
        /// <param name="shortSide"></param>
        /// <param name="orgWidth"></param>
        /// <param name="orgHeight"></param>
        /// <returns></returns>
        public static (int Width, int Height) ToFitShortSide(int shortSide, int orgWidth, int orgHeight)
        {
            int resizeWidth = 0;
            int resizeHeight = 0;
            if ((orgWidth > orgHeight ? orgHeight : orgWidth) <= shortSide)
            {
                resizeWidth = orgWidth;
                resizeHeight = orgHeight;
            }
            else
            {
                if (orgWidth > orgHeight)
                {
                    resizeWidth = (int)((double)orgWidth * ((double)shortSide / (double)orgHeight));
                    resizeHeight = shortSide;
                }
                else if (orgHeight > orgWidth)
                {
                    resizeWidth = shortSide;
                    resizeHeight = (int)((double)orgHeight * ((double)shortSide / (double)orgWidth));
                }
                else
                {
                    resizeWidth = shortSide;
                    resizeHeight = shortSide;
                }
            }
            return (resizeWidth, resizeHeight);
        }
    }
}
