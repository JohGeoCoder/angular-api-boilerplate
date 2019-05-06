using Interfaces.Services;
using Models.Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class UserService : BaseRepoService<User, AngularAPIBoilerplateDbContext>, IUserService<User>
    {
        public UserService(AngularAPIBoilerplateDbContext dbContext) : base(dbContext) { }
    }
}
