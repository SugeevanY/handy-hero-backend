using HandyHero.Common;
using HandyHero.DTO;
using HandyHero.Services.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HandyHero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IPasswordService _passwordService;

        public PasswordController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest fprequest)
        {
            try
            {
                await _passwordService.SendVerificationCodeAsync(fprequest.Email);



                return Ok(new { message = "Code_Sent" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

       
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {

            var result = await _passwordService.VerifyOtpAsync(request.VerificationCode);
            if (result)
            {
                return Ok(new { message = "OTP_Verified" });
            }

            return BadRequest(new { message = "Invalid OTP or user not found" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {

            var result = await _passwordService.ResetPasswordAsync(request.NewPassword);
            if (result)
            {
                return Ok(new { message = "Password_Updated" });
            }

            return BadRequest(new { message = "User not found or password reset failed" });
        }

    }

}

        
    