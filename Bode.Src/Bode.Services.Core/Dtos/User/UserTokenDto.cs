using Bode.Services.Core.Models.User;

namespace Bode.Services.Core.Dtos.User
{
    public class UserTokenDto
    {
        public int Id { get; set; }

        public string NickName { get; set; }

        public string HeadPic { get; set; }

        public string Token { get; set; }

        public Sex Sex { get; set; }
    }
}
