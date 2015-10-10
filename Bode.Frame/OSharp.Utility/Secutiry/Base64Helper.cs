using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.Utility.Secutiry
{
    public class Base64Helper
    {
        public static String GetBase64Encode(String str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            
            return Convert.ToBase64String(bytes);
        }
    }
}
