using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Adapter.Push.Jpush.common.resp
{
   public class APIConnectionException:Exception
    {
        public APIConnectionException(String message):base(message)
        {
            
        }
    }
}
