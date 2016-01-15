using System;
using Bode.Services.Core.Models.User;
using OSharp.Core.Data;

namespace Bode.Services.Core.Dtos.User
{
    public class UserInfoEditDto : IEditDto<int>
    {
        public int Id { get; set; }

        public string HeadPic { get; set; }

        public Sex Sex { get; set; }

        public string NickName { get; set; }
    }
}
