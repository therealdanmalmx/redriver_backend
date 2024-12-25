using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using redriver_backend.Models;
using redriver_test.Migrations;
using redriver_test.Models;

namespace redriver_test.Services.Auth
{
    public interface IAauthService
    {
        Task<ServiceResponse<Guid>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}