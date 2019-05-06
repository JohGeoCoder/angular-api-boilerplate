using System;
using System.Collections.Generic;
using System.Text;

namespace Exceptions.Registrations
{
    public class ExceptionUsernameAlreadyExists : Exception
    {
        public ExceptionUsernameAlreadyExists() : base("The username you selected already exists") { }
    }
}
