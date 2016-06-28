using System;
using System.Data.Entity;
using System.Linq;
using Bode.Services.Core.Models.Identity;
using OSharp.Data.Entity.Default;

namespace Bode.Services.Implement.SeedActions
{
    public class RoleSeedAction : IDefaultSeedAction
    {
        public void Action(DbContext context)
        {
            if (!context.Set<SysRole>().Any(p => p.Name == "admin"))
            {
                context.Set<SysRole>()
                    .Add(new SysRole()
                    {
                        Name = "admin",
                        Remark = "超级管理员角色，拥有系统最高权限",
                        IsAdmin = true,
                        IsSystem = true,
                        IsLocked = false,
                        CreatedTime = DateTime.Now
                    });
            }
        }

        public int Order
        {
            get
            {
                return 1;
            }
        }
    }
}
