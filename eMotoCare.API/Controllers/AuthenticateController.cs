using eMotoCare.Application.ApiResponse;
using eMotoCare.Application.DTOs;
using eMotoCare.Application.Interfaces.IService;
using eMotoCare.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eMotoCare.API.Controllers
{
    [Route("api/v1/auths")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthenticateController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authenticateService.Register(request);
            return Ok(new ApiResponse
            {
                Code = StatusCodes.Status200OK,
                Success = true,
                Message = "Register successful",
                Data = null
            });
        }
    }
}
