using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Auth
{
    public class LoginData
    {
        public required string Login { get; init; }
        public required string Password { get; init; }
    }
}
