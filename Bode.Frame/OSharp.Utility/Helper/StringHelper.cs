
namespace OSharp.Utility.Helper
{
    public static class StringHelper
    {
        /// <summary>  
        /// 把提取的字母变成小写  
        /// </summary>  
        /// <param name="strText">需要转换的字符串</param>  
        /// <returns>转换结果</returns>  
        public static string GetLowerChineseSpell(string strText)
        {
            return GetHeadSpell(strText).ToLower();
        }
        /// <summary>  
        /// 把提取的字母变成大写  
        /// </summary>  
        /// <param name="strText">需要转换的字符串</param>  
        /// <returns>转换结果</returns>  
        public static string GetUpperChineseSpell(string strText)
        {
            return GetHeadSpell(strText).ToUpper();
        }

        #region 私有方法

        /// <summary>  
        /// 获取单个汉字的首拼音  
        /// </summary>  
        /// <param name="strText">需要转换的字符</param>  
        /// <returns>转换结果</returns>  
        private static string GetHeadSpell(string strText)
        {
            //获取第一个汉字
            strText.CheckNotNullOrEmpty("strText");
            string firstChar = strText.Substring(0, 1);

            byte[] arrCN = System.Text.Encoding.Default.GetBytes(firstChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return System.Text.Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "_";
            }
            else return firstChar;
        }

        #endregion
    }
}
