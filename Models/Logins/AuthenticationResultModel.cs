using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Logins
{
    public class AuthenticationResultModel
    {
        public long UserId { get; set; }
        public string Token { get; set; }
    }
}
