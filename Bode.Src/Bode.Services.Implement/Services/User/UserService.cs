using System.Threading.Tasks;
using Bode.Services.Core.Dtos.User;
using Bode.Services.Core.Models.Identity;
using Bode.Services.Implement.Permissions.Identity;
using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;

namespace Bode.Services.Implement.Services
{
    public partial class UserService
    {
        public IRepository<SysUser, int> SysUserRepo { protected get; set; }

        /// <summary>
        /// 获取或设置 用户管理器
        /// </summary>
        public UserManager UserManager { get; set; }

        /// <summary>
        /// 获取或设置 用户存储器
        /// </summary>
        public UserStore UserStore { get; set; }

        /// <summary>
        /// 编辑UserInfo信息
        /// </summary>
        /// <param name="dtos">要更新的UserInfoEditDto信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> EditUserInfos(params UserInfoEditDto[] dtos)
        {
            dtos.CheckNotNull("dtos");

            OperationResult result = await Task.Run(() => UserInfoRepo.Update(dtos));
            return result;
        }
    }
}
