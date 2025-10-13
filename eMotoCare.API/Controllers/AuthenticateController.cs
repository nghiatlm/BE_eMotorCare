
using eMotoCare.BLL.Services.AuthenticateService;
using eMotoCare.Common.Exceptions;
using eMotoCare.Common.Models.ApiResponse;
using eMotoCare.Common.Models.Requests;
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

        [HttpPost("verify-account")]
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
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = ex.Message
                });
            }
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

        [HttpPost("change-password/{id}")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, Guid id)
        {
            try
            {
                var result = await _authenticateService.ChangePassword(request.OldPassword, request.NewPassword, request.ConfirmPassword, id);
                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Change password successful",
                    Data = null
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Success = false,
                    Message = "An error occurred while changing the password."
                });

            }
        }

        [HttpPost("send-forgot-password-otp")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordRequest request)
        {
            var result = await _authenticateService.ForgotPasswordAsync(request.PhoneNumber);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassRequest request)
        {
            var result = await _authenticateService.ResetPasswordAsync(request.PhoneNumber, request.Otp, request.NewPassword, request.ConfirmPassword);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
