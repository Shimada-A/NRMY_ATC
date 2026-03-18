namespace Share.Common
{
    using System.Drawing;
    using System.Drawing.Imaging;

    /// <summary>
    /// アプリケーションの画像
    /// </summary>
    public class AppImage
    {
        /// <summary>
        /// Topが上にくるように回転する
        /// </summary>
        /// <param name="bmp">画像</param>
        public static void Rotate(Bitmap bmp)
        {
            // 向き情報を取得する
            byte? orientation = GetOrientation(bmp);

            // 画像を回転する
            RotateFlip(bmp, orientation);
        }

        /// <summary>
        /// 向き情報を取得する（EXIF情報から向き情報であるPropertyTagOrientationの値を取得する）
        /// </summary>
        /// <param name="bmp">画像</param>
        /// <returns>向き情報</returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/desktop/gdiplus/-gdiplus-constant-property-item-descriptions#propertytagorientation
        /// </remarks>
        private static byte? GetOrientation(Bitmap bmp)
        {
            foreach (PropertyItem prop in bmp.PropertyItems)
            {
                if (prop.Id == 0x0112)
                {
                    return prop.Value[0];
                }
            }

            return null;
        }

        /// <summary>
        /// 画像を回転する
        /// </summary>
        /// <param name="bmp">画像</param>
        /// <param name="orientation">向き情報</param>
        /// <remarks>
        /// https://www.officedaytime.com/tips/gdiplusimage.html
        /// </remarks>
        private static void RotateFlip(Bitmap bmp, byte? orientation)
        {
            var type = RotateFlipType.RotateNoneFlipNone;
            switch (orientation)
            {
                case 1:
                    // 上
                    // 左　右
                    //   下
                    // そのまま
                    break;

                case 2:
                    // 上
                    // 右　左
                    //   下
                    // 左右反転
                    type = RotateFlipType.RotateNoneFlipX;
                    break;

                case 3:
                    // 下
                    // 右　左
                    //   上
                    // 上下左右反転
                    type = RotateFlipType.RotateNoneFlipXY;
                    break;

                case 4:
                    // 下
                    // 左　右
                    //   上
                    // 上下反転
                    type = RotateFlipType.RotateNoneFlipY;
                    break;

                case 5:
                    // 左
                    // 上　下
                    //   右
                    // 90度回転後左右反転
                    type = RotateFlipType.Rotate90FlipX;
                    break;

                case 6:
                    // 右
                    // 上　下
                    // 　左
                    // 90度回転
                    type = RotateFlipType.Rotate90FlipNone;
                    break;

                case 7:
                    // 右
                    // 下　上
                    //   左
                    // 270度回転後左右反転
                    type = RotateFlipType.Rotate270FlipX;
                    break;

                case 8:
                    // 左
                    // 下　上
                    //   右
                    // 270度回転
                    type = RotateFlipType.Rotate270FlipNone;
                    break;

                default:
                    break;
            }

            if (type != RotateFlipType.RotateNoneFlipNone)
                bmp.RotateFlip(type);
        }
    }
}