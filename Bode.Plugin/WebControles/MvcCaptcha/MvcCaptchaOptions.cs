using System;
namespace Bode.Plugin.WebControles.MvcCaptcha
{
    public class MvcCaptchaOptions
    {
        private int _width;
        private int _height;
        private int _length;
        private string _chars;
        private string _inputBoxId;
        private string _captchaImageContainerId;
        public int TextLength
        {
            get
            {
                return this._length;
            }
            set
            {
                this._length = ((value < 3) ? 3 : value);
            }
        }
        public string TextChars
        {
            get
            {
                return this._chars;
            }
            set
            {
                this._chars = ((string.IsNullOrEmpty(value) || value.Trim().Length < 3) ? "ACDEFGHJKLMNPQRSTUVWXYZ2346789" : value.Trim());
            }
        }
        public Level FontWarp
        {
            get;
            set;
        }
        public Level BackgroundNoise
        {
            get;
            set;
        }
        public Level LineNoise
        {
            get;
            set;
        }
        public int Width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = ((value < this.TextLength * 18) ? (this.TextLength * 18) : value);
            }
        }
        public int Height
        {
            get
            {
                return this._height;
            }
            set
            {
                this._height = ((value < 32) ? 32 : value);
            }
        }
        public string ValidationInputBoxId
        {
            get
            {
                if (this.DelayLoad && string.IsNullOrEmpty(this._inputBoxId))
                {
                    throw new ArgumentNullException("ValidationInputBoxId", "设置DelayLoad为true时必须指定ValidationInputBoxId的值");
                }
                return this._inputBoxId;
            }
            set
            {
                this._inputBoxId = value;
            }
        }
        public string CaptchaImageContainerId
        {
            get
            {
                if (this.DelayLoad && string.IsNullOrEmpty(this._captchaImageContainerId))
                {
                    throw new ArgumentNullException("CaptchaImageContainerId", "设置DelayLoad为true时必须指定CaptchaImageContainerId的值");
                }
                return this._captchaImageContainerId;
            }
            set
            {
                this._captchaImageContainerId = value;
            }
        }
        public string ReloadLinkText
        {
            get;
            set;
        }
        public bool DelayLoad
        {
            get;
            set;
        }
        public MvcCaptchaOptions()
        {
            this.FontWarp = Level.Medium;
            this.BackgroundNoise = Level.Low;
            this.LineNoise = Level.Low;
            this.ReloadLinkText = "换一张";
            this.Width = 160;
            this.Height = 40;
            this.TextLength = 4;
        }
    }
}
