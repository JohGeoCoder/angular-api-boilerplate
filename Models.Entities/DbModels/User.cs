using System;
using System.Collections.Generic;

namespace Models.Entities.DbModels
{
    public partial class User
    {
        public User()
        {
            UserRole = new HashSet<UserRole>();
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
