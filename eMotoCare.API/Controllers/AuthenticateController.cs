using eMotoCare.Application.ApiResponse;
using eMotoCare.Application.DTOs;
using eMotoCare.Application.Exceptions;
using eMotoCare.Application.Interfaces.IService;
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

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            try
            {
                var isValid = await _authenticateService.VerifyOtpAsync(request.PhoneNumber, request.Otp);
                if (!isValid)
                    return BadRequest(new ApiResponse
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Success = false,
                        Message = "Invalid or expired OTP."
                    });

                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Phone number verified successfully."
                });
            } catch (AppException ex)
            { return BadRequest(new ApiResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = ex.Message
                });}
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authenticateService.Login(request.Phone, request.Password);
            return Ok(new ApiResponse
            {
                Code = StatusCodes.Status200OK,
                Success = true,
                Message = "Login successful",
                Data = result
            });
        }


    }
}
