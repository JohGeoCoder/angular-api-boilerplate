using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities.DbModels.Interfaces
{
    public interface IBaseEntity
    {
        long Id { get; set; }
    }
}
