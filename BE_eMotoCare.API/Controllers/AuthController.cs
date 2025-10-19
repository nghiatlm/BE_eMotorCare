
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMototCare.BLL.Services.AuthServices;
using eMototCare.BLL.Services.FirebaseServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/auths")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IFirebaseService _firebase;

        public AuthController(IAuthService service, IFirebaseService firebase)
        {
            _service = service;
            _firebase = firebase;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _service.Login(request);
            return Ok(ApiResponse<AuthResponse>.SuccessResponse(response, "Đăng nhập thành công"));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _service.Register(request);
            return Ok(ApiResponse<string>.SuccessResponse("Đăng ký thành công"));
        }

        [HttpPost("verify-sms-otp")]
        public async Task<IActionResult> VerifySmsOtp([FromBody] VerifyOtpRequest request)
        {
            var decodedToken = await _firebase.VerifyIdTokenAsync(request.IdToken);
            var phone = decodedToken.Claims["phone_number"].ToString();
            return Ok(ApiResponse<AuthResponse>.SuccessResponse(null, "Xác thực OTP thành công"));
        }
    }
}