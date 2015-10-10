using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.Utility.Helper
{
    public static class DateTimeHelper
    {
        public static int GetAge(DateTime birthDay) 
        {
            DateTime now = DateTime.Today;

            int age = now.Year - birthDay.Year;

            if (birthDay > now.AddYears(-age)) age--;

            return age;
        }

        public static int GetAge(string birthDay) 
        {
            try 
            {
                var bday = DateTime.Parse(birthDay);
                return GetAge(bday);
            }
            catch 
            {
                return 0;
            }
        }
    }
}
