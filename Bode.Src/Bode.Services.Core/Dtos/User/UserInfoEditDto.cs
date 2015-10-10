using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSharp.Core.Data;

namespace Bode.Services.Core.Dtos.User
{
    public class UserInfoEditDto : IEditDto<int>
    {

        public int Id { get; set; }
    }
}
