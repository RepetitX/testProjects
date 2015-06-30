using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

namespace Common.Web.Mvc
{
    public class CAPTCHAImageResult : ActionResult
    {
        private readonly int _textLength;
        private readonly int _height;
        private readonly int _width;
        private readonly Color _backgroundColor;
        private readonly Color _textColor;
        private readonly string _fontFamilyName;
        private readonly AllowedSymbols _allowedSymbols;
        private readonly FontWarpLevel _fontWarpLevel;
        private readonly BackgroundNoiseLevel _backgroundNoiseLevel;
        private readonly LineNoiseLevel _lineNoiseLevel;

        private readonly Random _rand;
        private readonly string _fontsList;

        public CAPTCHAImageResult(
            int textLength = 5,
            int height = 30,
            int width = 100,
            Color? backgroundColor = null,
            Color? textColor = null,
            string fontFamilyName = null,
            AllowedSymbols allowedSymbols = AllowedSymbols.All,
            FontWarpLevel fontWarpLevel = FontWarpLevel.None,
            BackgroundNoiseLevel backgroundNoiseLevel = BackgroundNoiseLevel.None,
            LineNoiseLevel lineNoiseLevel = LineNoiseLevel.None)
        {
            _textLength = textLength;
            _height = height;
            _width = width;
            _backgroundColor = backgroundColor ?? Color.White;
            _textColor = textColor ?? Color.Black;
            _fontFamilyName = fontFamilyName;
            _allowedSymbols = allowedSymbols;
            _fontWarpLevel = fontWarpLevel;
            _backgroundNoiseLevel = backgroundNoiseLevel;
            _lineNoiseLevel = lineNoiseLevel;

            _rand = new Random();
            _fontsList = "arial;arial black;comic sans ms;courier new;estrangelo edessa;franklin gothic medium;" +
                "georgia;lucida console;lucida sans unicode;mangal;microsoft sans serif;palatino linotype;" +
                "sylfaen;tahoma;times new roman;trebuchet ms;verdana";
        }

        public override void ExecuteResult(ControllerContext context)
        {
            RenderImage(context);
        }

        private void RenderImage(ControllerContext context)
        {
            Font font = null;
            var bmp = new Bitmap(_width, _height, PixelFormat.Format32bppArgb);
            var gr = Graphics.FromImage(bmp);
            gr.SmoothingMode = SmoothingMode.AntiAlias;

            // fill an empty white rectangle
            var rect = new Rectangle(0, 0, _width, _height);
            Brush br = new SolidBrush(_backgroundColor);
            gr.FillRectangle(br, rect);

            var charOffset = 0;
            double charWidth = _width / _textLength;

            var randomText = GenerateRandomText();
            HttpContext.Current.Session["CaptchaValidationText"] = randomText;

            foreach (var c in randomText)
            {
                // establish font and draw area
                font = GetFont();
                var rectChar = new Rectangle(Convert.ToInt32(charOffset * charWidth), 0, Convert.ToInt32(charWidth), _height);

                // warp the character
                var gp = TextPath(c.ToString(), font, rectChar);
                WarpText(gp, rectChar);

                // draw the character
                br = new SolidBrush(_textColor);
                gr.FillPath(br, gp);

                charOffset += 1;
            }

            AddNoise(gr, rect);
            AddLine(gr, rect);

            context.HttpContext.Response.ContentType = "image/GF";
            bmp.Save(context.HttpContext.Response.OutputStream, ImageFormat.Gif);

            // clean up unmanaged resources
            if (font != null)
                font.Dispose();

            br.Dispose();
            gr.Dispose();
        }

        private string GenerateRandomText()
        {
            var symbols = new List<char>();

            switch (_allowedSymbols)
            {
                case AllowedSymbols.All:
                    {
                        for (var charPos = 65; charPos < 65 + 26; charPos++)
                            symbols.Add((char)charPos);

                        for (var intPos = 48; intPos <= 57; intPos++)
                            symbols.Add((char)intPos);

                        break;
                    }
                case AllowedSymbols.Chars:
                    {
                        for (var charPos = 65; charPos < 65 + 26; charPos++)
                            symbols.Add((char)charPos);

                        break;
                    }
                case AllowedSymbols.Numbers:
                    {
                        for (var intPos = 48; intPos <= 57; intPos++)
                            symbols.Add((char)intPos);

                        break;
                    }
            }

            var sb = new StringBuilder(_textLength);

            for (var i = 0; i < _textLength; i++)
                sb.Append(symbols.ElementAt(_rand.Next(symbols.Count)).ToString());

            return sb.ToString();
        }

        private Font GetFont()
        {
            float fontSize;
            var fontName = _fontFamilyName;

            if (string.IsNullOrEmpty(fontName))
                fontName = RandomFontFamily();

            switch (_fontWarpLevel)
            {
                case FontWarpLevel.Low:
                    fontSize = Convert.ToInt32(_height * 0.8);
                    break;
                case FontWarpLevel.Medium:
                    fontSize = Convert.ToInt32(_height * 0.85);
                    break;
                case FontWarpLevel.High:
                    fontSize = Convert.ToInt32(_height * 0.9);
                    break;
                case FontWarpLevel.Extreme:
                    fontSize = Convert.ToInt32(_height * 0.95);
                    break;
                default:
                    fontSize = Convert.ToInt32(_height * 0.7);
                    break;
            }

            return new Font(fontName, fontSize, FontStyle.Bold | FontStyle.Italic);
        }

        private string RandomFontFamily()
        {
            var fontsArray = _fontsList.Split(';');

            return fontsArray[_rand.Next(0, fontsArray.Length)];
        }

        private static GraphicsPath TextPath(string text, Font font, Rectangle rect)
        {
            var sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            var gp = new GraphicsPath();

            gp.AddString(text, font.FontFamily, (int)font.Style, font.Size, rect, sf);

            return gp;
        }

        private void WarpText(GraphicsPath textPath, Rectangle rect)
        {
            var warpDivisor = 0.0;
            var rangeModifier = 0.0;

            switch (_fontWarpLevel)
            {
                case FontWarpLevel.None:
                    return;
                case FontWarpLevel.Low:
                    warpDivisor = 6;
                    rangeModifier = 1;
                    break;
                case FontWarpLevel.Medium:
                    warpDivisor = 5;
                    rangeModifier = 1.3;
                    break;
                case FontWarpLevel.High:
                    warpDivisor = 4.5;
                    rangeModifier = 1.4;
                    break;
                case FontWarpLevel.Extreme:
                    warpDivisor = 4;
                    rangeModifier = 1.5;
                    break;
            }

            var rectF = new RectangleF(Convert.ToSingle(rect.Left), 0, Convert.ToSingle(rect.Width), rect.Height);

            var heightRange = Convert.ToInt32(rect.Height / warpDivisor);
            var widthRange = Convert.ToInt32(rect.Width / warpDivisor);
            var left = rect.Left - Convert.ToInt32(widthRange * rangeModifier);
            var top = rect.Top - Convert.ToInt32(heightRange * rangeModifier);
            var width = rect.Left + rect.Width + Convert.ToInt32(widthRange * rangeModifier);
            var height = rect.Top + rect.Height + Convert.ToInt32(heightRange * rangeModifier);

            if (left < 0)
                left = 0;
            if (top < 0)
                top = 0;
            if (width > _width)
                width = _width;
            if (height > _height)
                height = _height;

            var leftTop = RandomPoint(left, left + widthRange, top, top + heightRange);
            var rightTop = RandomPoint(width - widthRange, width, top, top + heightRange);
            var leftBottom = RandomPoint(left, left + widthRange, height - heightRange, height);
            var rightBottom = RandomPoint(width - widthRange, width, height - heightRange, height);

            var points = new[] { leftTop, rightTop, leftBottom, rightBottom };

            var matrix = new Matrix();
            matrix.Translate(0, 0);

            textPath.Warp(points, rectF, matrix, WarpMode.Perspective, 0);
        }

        private PointF RandomPoint(Rectangle rect)
        {
            return RandomPoint(rect.Left, rect.Width, rect.Top, rect.Bottom);
        }

        private PointF RandomPoint(int xMin, int xMax, int yMin, int yMax)
        {
            return new PointF(_rand.Next(xMin, xMax), _rand.Next(yMin, yMax));
        }

        private void AddNoise(Graphics graphics, Rectangle rect)
        {
            var density = 0;
            var size = 0;

            switch (_backgroundNoiseLevel)
            {
                case BackgroundNoiseLevel.None:
                    return;
                case BackgroundNoiseLevel.Low:
                    density = 30;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.Medium:
                    density = 18;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.High:
                    density = 16;
                    size = 39;
                    break;
                case BackgroundNoiseLevel.Extreme:
                    density = 12;
                    size = 38;
                    break;
            }

            var br = new SolidBrush(_textColor);
            var max = Convert.ToInt32(Math.Max(rect.Width, rect.Height) / size);

            for (var i = 0; i <= Convert.ToInt32((rect.Width * rect.Height) / density); i++)
                graphics.FillEllipse(br, _rand.Next(rect.Width), _rand.Next(rect.Height), _rand.Next(max), _rand.Next(max));

            br.Dispose();
        }

        private void AddLine(Graphics graphics, Rectangle rect)
        {
            var length = 0;
            float width = 0;
            var lineCount = 0;

            switch (_lineNoiseLevel)
            {
                case LineNoiseLevel.None:
                    return;
                case LineNoiseLevel.Low:
                    length = 4;
                    width = Convert.ToSingle(_height / 31.25);
                    // 1.6
                    lineCount = 1;
                    break;
                case LineNoiseLevel.Medium:
                    length = 5;
                    width = Convert.ToSingle(_height / 27.7777);
                    // 1.8
                    lineCount = 1;
                    break;
                case LineNoiseLevel.High:
                    length = 3;
                    width = Convert.ToSingle(_height / 25);
                    // 2.0
                    lineCount = 2;
                    break;
                case LineNoiseLevel.Extreme:
                    length = 3;
                    width = Convert.ToSingle(_height / 22.7272);
                    // 2.2
                    lineCount = 3;
                    break;
            }

            var pf = new PointF[length + 1];
            var p = new Pen(_textColor, width);

            for (var l = 1; l <= lineCount; l++)
            {
                for (var i = 0; i <= length; i++)
                    pf[i] = RandomPoint(rect);

                graphics.DrawCurve(p, pf, 1.75f);
            }

            p.Dispose();
        }
    }

    #region Public Enums

    public enum AllowedSymbols
    {
        All,
        Chars,
        Numbers
    }

    /// <summary>
    /// Amount of random font warping to apply to rendered text
    /// </summary>
    public enum FontWarpLevel
    {
        None,
        Low,
        Medium,
        High,
        Extreme
    }

    /// <summary>
    /// Amount of background noise to add to rendered image
    /// </summary>
    public enum BackgroundNoiseLevel
    {
        None,
        Low,
        Medium,
        High,
        Extreme
    }

    /// <summary>
    /// Amount of curved line noise to add to rendered image
    /// </summary>
    public enum LineNoiseLevel
    {
        None,
        Low,
        Medium,
        High,
        Extreme
    }

    #endregion
}