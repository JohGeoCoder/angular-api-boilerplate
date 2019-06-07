using Interfaces.Services;
using Models.Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class UserRoleService : BaseRepoService<UserRole, WebsiteApiDbContext>, IUserRoleService
    {
        public UserRoleService(WebsiteApiDbContext dbContext) : base(dbContext) { }
    }
}
