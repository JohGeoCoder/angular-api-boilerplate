using Interfaces.Services;
using Models.Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class UserRoleService : BaseRepoService<UserRole, AngularAPIBoilerplateDbContext>, IUserRoleService
    {
        public UserRoleService(AngularAPIBoilerplateDbContext dbContext) : base(dbContext) { }
    }
}
