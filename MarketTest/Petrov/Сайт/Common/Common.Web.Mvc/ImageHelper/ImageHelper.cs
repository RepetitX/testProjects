using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using System.Linq;

namespace Common.Web.Mvc
{
    public static class ImageHelper
    {
        public static bool StreamIsImage(Stream stream)
        {
            try
            {
                new Bitmap(stream);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Image ConvertBytesToImage(byte[] bytes)
        {
            return Image.FromStream(new MemoryStream(bytes, 0, bytes.Length), true);
        }

        public static Image ConvertStreamToImage(Stream stream)
        {
            return Image.FromStream(stream);
        }

        /// <summary>
        /// Масштабирует изображение до заданной ширины.
        /// </summary>
        /// <param name="image">Изображение.</param>
        /// <param name="width">Ширина в пикселях.</param>
        /// <returns></returns>
        public static Image Scale(Image image, int width)
        {
            var srcWidth = image.Width;
            var srcHeight = image.Height;

            var nPercent = ((float)width / (float)srcWidth);

            var destWidth = (int)(srcWidth * nPercent);
            var destHeight = (int)(srcHeight * nPercent);

            var destImage = new Bitmap(destWidth, destHeight);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, destWidth, destHeight);

                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                using (var attribute = new ImageAttributes())
                {
                    attribute.SetWrapMode(WrapMode.TileFlipXY);

                    graphics.DrawImage(
                        image,
                        new Rectangle(0, 0, destWidth, destHeight),
                        0,
                        0,
                        srcWidth,
                        srcHeight,
                        GraphicsUnit.Pixel,
                        attribute);
                }
            }

            return destImage;
        }

        /// <summary>
        /// Масштабирует изображение до заданных размеров с обрезкой.
        /// Изображение сжимается по меньшей стороне до указанного размера.
        /// Затем большая сторона обрезается до указанного размера в случае необходимости.
        /// </summary>
        /// <param name="image">Изображение.</param>
        /// <param name="width">Ширина в пикселях.</param>
        /// <param name="height">Высота в пикселях.</param>
        /// <returns></returns>
        public static Image ScaleAndCrop(Image image, int width, int height)
        {
            var srcWidth = image.Width;
            var srcHeight = image.Height;
            var destX = 0;
            var destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)width / (float)srcWidth);
            nPercentH = ((float)height / (float)srcHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentW;

                destY = (int)((height - (srcHeight * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentH;

                destX = (int)((width - (srcWidth * nPercent)) / 2);
            }

            var destWidth = (int)Math.Round(srcWidth * nPercent);
            var destHeight = (int)Math.Round(srcHeight * nPercent);

            var destImage = new Bitmap(width, height);
            destImage.SetResolution(destImage.HorizontalResolution, destImage.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);

                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                // use an image attribute in order to remove the black/gray border around image after resize
                // (most obvious on white images), see this post for more information:
                // http://www.codeproject.com/KB/GDI-plus/imgresizoutperfgdiplus.aspx
                using (var attribute = new ImageAttributes())
                {
                    attribute.SetWrapMode(WrapMode.TileFlipXY);

                    graphics.DrawImage(
                        image,
                        new Rectangle(destX, destY, destWidth, destHeight),
                        0,
                        0,
                        srcWidth,
                        srcHeight,
                        GraphicsUnit.Pixel,
                        attribute);
                }
            }

            return destImage;
        }

        public static Bitmap ResizeImage(byte[] bytes, int width, int height)
        {
            return ResizeImage(bytes, width, height, 0);
        }

        public static Bitmap ResizeImage(byte[] bytes, int width, int height, int dpi)
        {
            return ResizeImage(ConvertBytesToImage(bytes), width, height, dpi);
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            return ResizeImage(image, width, height, 0);
        }

        public static Bitmap ResizeImage(Image image, int width, int height, int dpi)
        {
            if (image.Width == width && image.Height == height)
                return new Bitmap(image);

            var result = new Bitmap(width, height);
            
            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }
            
            if (dpi > 0)
                result.SetResolution(dpi, dpi);
            
            //return the resulting bitmap
            return result;
        }

        public static Bitmap SquareImage(Image image)
        {
            int dimension = Math.Min(image.Width, image.Height);

            int x = 0;
            int y = 0;

            if (image.Width < image.Height)
                y = -1 * Convert.ToInt32((image.Height - dimension) / 2);
            else
                x = -1 * Convert.ToInt32((image.Width - dimension) / 2);

            var result = new Bitmap(dimension, dimension);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.DrawImage(image, x, y, image.Width, image.Height);
            }

            return result;
        }

        public static Bitmap SquareImage(byte[] image)
        {
            return SquareImage(ConvertBytesToImage(image));
        }

        public static string GetFormatName(ImageFormat format)
        {
            if (format.Equals(ImageFormat.Bmp))
                return "image/bmp";
            if (format.Equals(ImageFormat.Emf))
                return "image/emf";
            if (format.Equals(ImageFormat.Exif))
                return "image/exif";
            if (format.Equals(ImageFormat.Gif))
                return "image/gif";
            if (format.Equals(ImageFormat.Icon))
                return "image/icon";
            if (format.Equals(ImageFormat.Jpeg))
                return "image/jpeg";
            if (format.Equals(ImageFormat.MemoryBmp))
                return "image/bmp";
            if (format.Equals(ImageFormat.Png))
                return "image/png";
            if (format.Equals(ImageFormat.Tiff))
                return "image/tiff";
            if (format.Equals(ImageFormat.Wmf))
                return "image/wmf";
            return "image";
        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();

            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}
