using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Karaoke.Core.Graphics
{

    /// <summary>
    /// A utility class for working with graphics.
    /// </summary>
    public class GraphicUtility : IGraphicUtility
    {

        #region SINGLETON


        private static IGraphicUtility _instance = new GraphicUtility();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static IGraphicUtility Instance
        {
            get { return _instance ??= new GraphicUtility(); }
        }


        #endregion SINGLETON

        #region PUBLIC METHODS


        /// <summary>
        /// Gets the distinct colors from bitmap.
        /// </summary>
        /// <param name="bmp">The BMP.</param>
        /// <returns></returns>
        public List<Color> GetDistinctColorsFromBitmap(Bitmap bmp)
        {
            var list = new List<Color>();
            for (var y = 0; y < bmp.Height; y++)
            {
                for (var x = 0; x < bmp.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    if (!list.Contains(pixel))
                        list.Add(pixel);
                }
            }
            return list;
        }

        /// <summary>
        /// Reduces the amount of colors to the specified number.
        /// </summary>
        /// <param name="colors">The colors list to quantize.</param>
        /// <param name="count">The number of colors to return.</param>
        /// <returns></returns>
        public List<Color> QuantizeColors(List<Color> colors, Int32 count)
        {
            // TODO: Hookup color quantization.
            return colors;
        }

        /// <summary>
        /// Takes a bitmap from a file and turns it into a file stream.
        /// </summary>
        /// <param name="filename">The filename for the file to turn into a stream.</param>
        /// <returns>Returns a stream of the specified file.</returns>
        public Stream BitmapToStream(String filename)
        {
            Bitmap oldBmp = (Bitmap)Image.FromFile(filename);
            int width = oldBmp.Width;
            int height = oldBmp.Width;
            BitmapData oldData = oldBmp.LockBits(new Rectangle(0, 0, oldBmp.Width, oldBmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            int length = oldData.Stride * oldBmp.Height;
            byte[] stream = new byte[length];
            Marshal.Copy(oldData.Scan0, stream, 0, length);
            oldBmp.UnlockBits(oldData);
            oldBmp.Dispose();
            return new MemoryStream(stream);
        }

        /// <summary>
        /// Converts a file stream to a bitmap.
        /// </summary>
        /// <param name="stream">The stream to convert.</param>
        /// <param name="width">The width of the resulting bitmap.</param>
        /// <param name="height">The height of the resulting bitmap.</param>
        /// <returns>A bitmap from the file stream provided.</returns>
        public Bitmap StreamToBitmap(Stream stream, Int32 width, Int32 height)
        {
            //create a new bitmap
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            stream.Seek(0, SeekOrigin.Begin);
            //copy the stream of pixel
            for (var n = 0; n <= stream.Length - 1; n++)
            {
                byte[] temp = new byte[1];
                stream.Read(temp, 0, 1);
                Marshal.WriteByte(bmpData.Scan0, n, temp[0]);
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        /// <summary>
        /// Resizes the the bitmap to the specified sizes.
        /// </summary>
        /// <param name="bm">The bitmap to resize.</param>
        /// <param name="width">The new width of the bitmap.</param>
        /// <param name="height">The new height of the bitmap.</param>
        /// <returns>Returns a resized bitmap</returns>
        public Bitmap ResizeBitmap(Bitmap bm, Int32 width, Int32 height)
        {
            var thumb = new Bitmap(width, height);
            var g = System.Drawing.Graphics.FromImage(thumb);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(bm, new Rectangle(0, 0, width, height), new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
            g.Dispose();
            bm.Dispose();
            return thumb;
        }

        /// <summary>
        /// Merges the images with transparency.
        /// </summary>
        /// <param name="Pic1">The pic1.</param>
        /// <param name="pic2">The pic2.</param>
        /// <returns></returns>
        public Bitmap MergeImagesWithTransparency(Bitmap Pic1, Bitmap pic2)
        {
            var mergedImage = default(Image);
            var bm = new Bitmap(Pic1.Width, Pic1.Height);
            var gr = System.Drawing.Graphics.FromImage(bm);
            gr.DrawImage(Pic1, 0, 0);
            pic2.MakeTransparent(pic2.GetPixel(1, 1));
            gr.DrawImage(pic2, 0, 0);
            mergedImage = bm;
            gr.Dispose();
            return (Bitmap)mergedImage;
        }

        /// <summary>
        /// Converts an image to a byte array
        /// </summary>
        /// <param name="imageIn">Image to convert to a byte array</param>
        public Byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            var ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        /// <summary>
        /// Images to byte array.
        /// </summary>
        /// <param name="imageIn">The image in.</param>
        /// <returns></returns>
        public Byte[] ImageToByteArray(Bitmap imageIn)
        {
            var img = (Image)imageIn;
            return ImageToByteArray(img);
        }

        /// <summary>
        /// Converts a byte array to an image
        /// </summary>
        /// <param name="byteArrayIn">Byte array to convert to an image</param>
        public Image ByteArrayToImage(Byte[] byteArrayIn)
        {
            var ms = new MemoryStream(byteArrayIn);
            var returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }


        #endregion PUBLIC METHODS

    }


    /// <summary>
    /// 
    /// </summary>
    public interface IGraphicUtility
    {


        /// <summary>
        /// Gets the distinct colors from bitmap.
        /// </summary>
        /// <param name="bmp">The BMP.</param>
        /// <returns></returns>
        List<Color> GetDistinctColorsFromBitmap(Bitmap bmp);

        /// <summary>
        /// Reduces the amount of colors to the specified number.
        /// </summary>
        /// <param name="colors">The colors list to quantize.</param>
        /// <param name="count">The number of colors to return.</param>
        /// <returns></returns>
        List<Color> QuantizeColors(List<Color> colors, Int32 count);

        /// <summary>
        /// Takes a bitmap from a file and turns it into a file stream.
        /// </summary>
        /// <param name="filename">The filename for the file to turn into a stream.</param>
        /// <returns>Returns a stream of the specified file.</returns>
        Stream BitmapToStream(string filename);

        /// <summary>
        /// Converts a file stream to a bitmap.
        /// </summary>
        /// <param name="stream">The stream to convert.</param>
        /// <param name="width">The width of the resulting bitmap.</param>
        /// <param name="height">The height of the resulting bitmap.</param>
        /// <returns>A bitmap from the file stream provided.</returns>
        Bitmap StreamToBitmap(Stream stream, Int32 width, Int32 height);

        /// <summary>
        /// Resizes the the bitmap to the specified sizes.
        /// </summary>
        /// <param name="bm">The bitmap to resize.</param>
        /// <param name="width">The new width of the bitmap.</param>
        /// <param name="height">The new height of the bitmap.</param>
        /// <returns>Returns a resized bitmap</returns>
        Bitmap ResizeBitmap(Bitmap bm, Int32 width, Int32 height);

        /// <summary>
        /// Merges the images with transparency.
        /// </summary>
        /// <param name="Pic1">The pic1.</param>
        /// <param name="pic2">The pic2.</param>
        /// <returns></returns>
        Bitmap MergeImagesWithTransparency(Bitmap Pic1, Bitmap pic2);

        /// <summary>
        /// Converts an image to a byte array
        /// </summary>
        /// <param name="imageIn">The image in.</param>
        /// <returns></returns>
        Byte[] ImageToByteArray(System.Drawing.Image imageIn);

        /// <summary>
        /// Converts an image to a byte array
        /// </summary>
        /// <param name="imageIn">The image to convert.</param>
        /// <returns></returns>
        Byte[] ImageToByteArray(Bitmap imageIn);

        /// <summary>
        /// Converts a byte array to an image.
        /// </summary>
        /// <param name="byteArrayIn">The byte array in.</param>
        /// <returns></returns>
        Image ByteArrayToImage(Byte[] byteArrayIn);

    }

}
