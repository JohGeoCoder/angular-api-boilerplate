using Models.Entities.DbModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces.Services
{
    public interface IUserService<TEntity> : IRepoService<TEntity> where TEntity : class, IBaseEntity
    {

    }
}
