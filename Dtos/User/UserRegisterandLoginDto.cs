using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace redriver_test.Dtos.User
{
    public class UserRegisterandLoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}