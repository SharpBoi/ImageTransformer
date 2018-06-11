using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageTransformer.Utilities
{
    public static class Utility
    {
        #region MyRegion
        public static int[] ParseRect(string csvRectString)
        {
            int[] ret = new int[4];

            string number = "";
            int numCnt = 0;
            for (int i = 0; i < csvRectString.Length; i++)
            {
                if (csvRectString[i] != ',')
                    number += csvRectString[i];
                if (csvRectString[i] == ',' || i == csvRectString.Length - 1)
                {
                    ret[numCnt] = Convert.ToInt32(number);
                    numCnt++;
                    number = "";
                }
            }

            return ret;
        }

        /// <summary>
        /// Преобразует байты изображения в объект изображения
        /// </summary>
        /// <param name="png"></param>
        /// <returns></returns>
        public static Bitmap PNGBytesToBitmap(byte[] png)
        {
            return (Bitmap)new ImageConverter().ConvertFrom(png);
        }

        /// <summary>
        /// Преобразует изображение в байты
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] BitmapToPNGBytes(Bitmap img)
        {
            return (byte[])new ImageConverter().ConvertTo(img, typeof(byte[]));
        }
        #endregion
    }
}
