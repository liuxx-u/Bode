using System;

namespace OSharp.Utility.Helper
{
    public class OrderNoHelper
    {
        private static readonly object Locker = new object();

        private static int _sn = 0;

        public static string NextBillNumber()
        {
            lock (Locker)
            {
                if (_sn == 99999)
                    _sn = 0;
                else
                    _sn++;
                return DateTime.Now.ToString("yyyyMMddHHmmss") + _sn.ToString().PadLeft(5, '0');
            }
        }
        // 防止创建类的实例
        private OrderNoHelper() { }
    }
}
