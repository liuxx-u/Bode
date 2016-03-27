using System;
using System.Configuration;
namespace Bode.Plugin.WebControles.MvcCaptcha
{
    public class MvcCaptchaConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("textLength", IsRequired = false, DefaultValue = 4)]
        public int TextLength
        {
            get
            {
                int num = (int)base["textLength"];
                if (num >= 3)
                {
                    return num;
                }
                return 3;
            }
        }
        [ConfigurationProperty("textChars", IsRequired = false, DefaultValue = "ACDEFGHJKLMNPQRSTUVWXYZ2346789")]
        public string TextChars
        {
            get
            {
                string text = (string)base["textChars"];
                if (text.Length >= 3)
                {
                    return text;
                }
                return "ACDEFGHJKLMNPQRSTUVWXYZ2346789";
            }
        }
        [ConfigurationProperty("fontWarp", IsRequired = false, DefaultValue = Level.Medium)]
        public Level FontWarp
        {
            get
            {
                return (Level)base["fontWarp"];
            }
        }
        [ConfigurationProperty("lineNoise", IsRequired = false, DefaultValue = Level.Low)]
        public Level LineNoise
        {
            get
            {
                return (Level)base["lineNoise"];
            }
        }
        [ConfigurationProperty("backgroundNoise", IsRequired = false, DefaultValue = Level.Low)]
        public Level BackgroundNoise
        {
            get
            {
                return (Level)base["backgroundNoise"];
            }
        }
        public static MvcCaptchaConfigSection GetConfig()
        {
            return ConfigurationManager.GetSection("mvcCaptchaGroup/mvcCaptchaOptions") as MvcCaptchaConfigSection;
        }
    }
}
