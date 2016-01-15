using AutoMapper;
using Bode.Services.Core.Models.Identity;
using OSharp.Core.Security;
using Bode.Services.Core.Dtos.Identity;
using Bode.Services.Core.Dtos.Security;
using Bode.Services.Core.Dtos.User;
using Bode.Services.Core.Models.User;

namespace Bode.Services.Core.Dtos
{
    public partial class DtoMappers
    {
        static partial void MapperRegisterCustom()
        {
            //Identity
            Mapper.CreateMap<SysOrganizationDto, SysOrganization>();
            Mapper.CreateMap<SysUserDto, SysUser>();
            Mapper.CreateMap<SysRoleDto, SysRole>();

            //Security
            Mapper.CreateMap<FunctionDto, Function>();
            Mapper.CreateMap<EntityInfoDto, EntityInfo>();

            //UserInfo
            Mapper.CreateMap<UserInfoRegistDto, UserInfo>();
            Mapper.CreateMap<UserInfoEditDto, UserInfo>();
        }
    }
}
