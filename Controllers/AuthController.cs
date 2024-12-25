using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using redriver_backend.Models;
using redriver_test.Dtos.User;
using redriver_test.Models;
using redriver_test.Services.Auth;

namespace redriver_test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAauthService _authService;

        public AuthController(IAauthService authService )
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<Guid>>> Register(UserRegisterandLoginDto request)
        {
            var response = await _authService.Register(
                new User { UserName = request.Username }, request.Password
            );

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<Guid>>> Login(UserRegisterandLoginDto request)
        {
            var response = await _authService.Login(request.Username, request.Password);

            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}