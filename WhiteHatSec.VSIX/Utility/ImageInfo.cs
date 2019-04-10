using System.Drawing;
using System.Windows.Forms;
using stdole;

namespace WhiteHatSec.VSIX.Utility
{
    /// <summary>
    /// </summary>
    public static class ImageInfo
    {
        /// <summary>
        ///     Create Picture
        /// </summary>
        /// <param name="bitmapIcon"></param>
        /// <returns></returns>
     
        public static StdPicture CreatePicture(Bitmap bitmapIcon)
        {
            return (StdPicture) AxHostWrapper.GetOlePicture(bitmapIcon);
        }

        /// <summary>
        ///     Private internal class exploiting protected method GetIPictureDispFromPicture
        ///     implemented by AxHost to convert activex Images.
        /// </summary>
     
        private class AxHostWrapper : AxHost
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="AxHostWrapper" /> class.
            /// </summary>
            public AxHostWrapper() : base(null)
            {
            }

            /// <summary>
            ///     Gets the OLE picture.
            /// </summary>
            /// <param name="image">The image.</param>
            /// <returns></returns>
            public static IPictureDisp GetOlePicture(Image image)
            {
                return (IPictureDisp) GetIPictureDispFromPicture(image);
            }
        }
    }
}