using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Web;
namespace Bode.Plugin.WebControles.MvcCaptcha
{
    internal class MvcCaptchaImage
    {
        private static readonly string[] RandomFontFamily = new string[]
        {
            "arial",
            "arial black",
            "comic sans ms",
            "courier new",
            "estrangelo edessa",
            "franklin gothic medium",
            "georgia",
            "lucida console",
            "lucida sans unicode",
            "mangal",
            "microsoft sans serif",
            "palatino linotype",
            "sylfaen",
            "tahoma",
            "times new roman",
            "trebuchet ms",
            "verdana"
        };
        private static readonly Color[] RandomColor = new Color[]
        {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Black,
            Color.Purple,
            Color.Orange
        };
        private readonly Random _rand;
        internal string UniqueId
        {
            get;
            private set;
        }
        public string Text
        {
            get;
            private set;
        }
        public MvcCaptchaOptions CaptchaOptions
        {
            get;
            set;
        }
        public static MvcCaptchaImage GetCachedCaptcha(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return null;
            }
            return (MvcCaptchaImage)HttpContext.Current.Session[guid];
        }
        internal MvcCaptchaImage() : this(new MvcCaptchaOptions())
        {
        }
        internal MvcCaptchaImage(MvcCaptchaOptions options)
        {
            this.CaptchaOptions = options;
            this.UniqueId = Guid.NewGuid().ToString("N");
            this._rand = new Random();
        }
        internal void ResetText()
        {
            this.Text = this.GenerateRandomText();
        }
        private string GetRandomFontFamily()
        {
            return MvcCaptchaImage.RandomFontFamily[this._rand.Next(0, MvcCaptchaImage.RandomFontFamily.Length)];
        }
        private string GenerateRandomText()
        {
            string text = this.CaptchaOptions.TextChars;
            if (string.IsNullOrEmpty(text))
            {
                text = "ACDEFGHJKLMNPQRSTUVWXYZ2346789";
            }
            StringBuilder stringBuilder = new StringBuilder(this.CaptchaOptions.TextLength);
            int length = text.Length;
            for (int i = 0; i <= this.CaptchaOptions.TextLength - 1; i++)
            {
                stringBuilder.Append(text.Substring(this._rand.Next(length), 1));
            }
            return stringBuilder.ToString();
        }
        private PointF RandomPoint(int xmin, int xmax, int ymin, int ymax)
        {
            return new PointF((float)this._rand.Next(xmin, xmax), (float)this._rand.Next(ymin, ymax));
        }
        private Color GetRandomColor()
        {
            return MvcCaptchaImage.RandomColor[this._rand.Next(0, MvcCaptchaImage.RandomColor.Length)];
        }
        private PointF RandomPoint(Rectangle rect)
        {
            return this.RandomPoint(rect.Left, rect.Width, rect.Top, rect.Bottom);
        }
        private static GraphicsPath TextPath(string s, Font f, Rectangle r)
        {
            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near
            };
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddString(s, f.FontFamily, (int)f.Style, f.Size, r, format);
            return graphicsPath;
        }
        private Font GetFont()
        {
            string randomFontFamily = this.GetRandomFontFamily();
            float emSize;
            switch (this.CaptchaOptions.FontWarp)
            {
                case Level.Low:
                    emSize = (float)Convert.ToInt32((double)this.CaptchaOptions.Height * 0.8);
                    break;
                case Level.Medium:
                    emSize = (float)Convert.ToInt32((double)this.CaptchaOptions.Height * 0.85);
                    break;
                case Level.High:
                    emSize = (float)Convert.ToInt32((double)this.CaptchaOptions.Height * 0.9);
                    break;
                case Level.Extreme:
                    emSize = (float)Convert.ToInt32((double)this.CaptchaOptions.Height * 0.95);
                    break;
                default:
                    emSize = (float)Convert.ToInt32((double)this.CaptchaOptions.Height * 0.7);
                    break;
            }
            return new Font(randomFontFamily, emSize, FontStyle.Bold);
        }
        internal Bitmap RenderImage()
        {
            Bitmap bitmap = new Bitmap(this.CaptchaOptions.Width, this.CaptchaOptions.Height, PixelFormat.Format24bppRgb);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.White);
                int num = 0;
                double num2 = (double)(this.CaptchaOptions.Width / this.CaptchaOptions.TextLength);
                string text = this.Text;
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    using (Font font = this.GetFont())
                    {
                        using (Brush brush = new SolidBrush(this.GetRandomColor()))
                        {
                            Rectangle rectangle = new Rectangle(Convert.ToInt32((double)num * num2), 0, Convert.ToInt32(num2), this.CaptchaOptions.Height);
                            GraphicsPath graphicsPath = MvcCaptchaImage.TextPath(c.ToString(), font, rectangle);
                            this.WarpText(graphicsPath, rectangle);
                            graphics.FillPath(brush, graphicsPath);
                            num++;
                        }
                    }
                }
                Rectangle rect = new Rectangle(new Point(0, 0), bitmap.Size);
                this.AddNoise(graphics, rect);
                this.AddLine(graphics, rect);
            }
            return bitmap;
        }
        private void WarpText(GraphicsPath textPath, Rectangle rect)
        {
            float num;
            float num2;
            switch (this.CaptchaOptions.FontWarp)
            {
                case Level.Low:
                    num = 6f;
                    num2 = 1f;
                    break;
                case Level.Medium:
                    num = 5f;
                    num2 = 1.3f;
                    break;
                case Level.High:
                    num = 4.5f;
                    num2 = 1.4f;
                    break;
                case Level.Extreme:
                    num = 4f;
                    num2 = 1.5f;
                    break;
                default:
                    return;
            }
            RectangleF srcRect = new RectangleF(Convert.ToSingle(rect.Left), 0f, Convert.ToSingle(rect.Width), (float)rect.Height);
            int num3 = Convert.ToInt32((float)rect.Height / num);
            int num4 = Convert.ToInt32((float)rect.Width / num);
            int num5 = rect.Left - Convert.ToInt32((float)num4 * num2);
            int num6 = rect.Top - Convert.ToInt32((float)num3 * num2);
            int num7 = rect.Left + rect.Width + Convert.ToInt32((float)num4 * num2);
            int num8 = rect.Top + rect.Height + Convert.ToInt32((float)num3 * num2);
            if (num5 < 0)
            {
                num5 = 0;
            }
            if (num6 < 0)
            {
                num6 = 0;
            }
            if (num7 > this.CaptchaOptions.Width)
            {
                num7 = this.CaptchaOptions.Width;
            }
            if (num8 > this.CaptchaOptions.Height)
            {
                num8 = this.CaptchaOptions.Height;
            }
            PointF pointF = this.RandomPoint(num5, num5 + num4, num6, num6 + num3);
            PointF pointF2 = this.RandomPoint(num7 - num4, num7, num6, num6 + num3);
            PointF pointF3 = this.RandomPoint(num5, num5 + num4, num8 - num3, num8);
            PointF pointF4 = this.RandomPoint(num7 - num4, num7, num8 - num3, num8);
            PointF[] destPoints = new PointF[]
            {
                pointF,
                pointF2,
                pointF3,
                pointF4
            };
            Matrix matrix = new Matrix();
            matrix.Translate(0f, 0f);
            textPath.Warp(destPoints, srcRect, matrix, WarpMode.Perspective, 0f);
        }
        private void AddNoise(Graphics g, Rectangle rect)
        {
            int num;
            int num2;
            switch (this.CaptchaOptions.BackgroundNoise)
            {
                case Level.None:
                    return;
                case Level.Low:
                    num = 30;
                    num2 = 40;
                    break;
                case Level.Medium:
                    num = 18;
                    num2 = 40;
                    break;
                case Level.High:
                    num = 16;
                    num2 = 39;
                    break;
                case Level.Extreme:
                    num = 12;
                    num2 = 38;
                    break;
                default:
                    return;
            }
            SolidBrush solidBrush = new SolidBrush(this.GetRandomColor());
            int maxValue = Convert.ToInt32(Math.Max(rect.Width, rect.Height) / num2);
            for (int i = 0; i <= Convert.ToInt32(rect.Width * rect.Height / num); i++)
            {
                g.FillEllipse(solidBrush, this._rand.Next(rect.Width), this._rand.Next(rect.Height), this._rand.Next(maxValue), this._rand.Next(maxValue));
            }
            solidBrush.Dispose();
        }
        private void AddLine(Graphics g, Rectangle rect)
        {
            int num;
            float width;
            int num2;
            switch (this.CaptchaOptions.LineNoise)
            {
                case Level.None:
                    return;
                case Level.Low:
                    num = 4;
                    width = Convert.ToSingle((double)this.CaptchaOptions.Height / 31.25);
                    num2 = 1;
                    break;
                case Level.Medium:
                    num = 5;
                    width = Convert.ToSingle((double)this.CaptchaOptions.Height / 27.7777);
                    num2 = 1;
                    break;
                case Level.High:
                    num = 3;
                    width = Convert.ToSingle(this.CaptchaOptions.Height / 25);
                    num2 = 2;
                    break;
                case Level.Extreme:
                    num = 3;
                    width = Convert.ToSingle((double)this.CaptchaOptions.Height / 22.7272);
                    num2 = 3;
                    break;
                default:
                    return;
            }
            PointF[] array = new PointF[num + 1];
            using (Pen pen = new Pen(this.GetRandomColor(), width))
            {
                for (int i = 1; i <= num2; i++)
                {
                    for (int j = 0; j <= num; j++)
                    {
                        array[j] = this.RandomPoint(rect);
                    }
                    g.DrawCurve(pen, array, 1.75f);
                }
            }
        }
    }
}
