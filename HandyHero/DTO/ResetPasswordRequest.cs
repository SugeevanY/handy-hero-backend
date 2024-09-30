using Microsoft.AspNetCore.Identity.Data;

namespace HandyHero.DTO
{
    public class ResetPasswordRequest
    {
        public string NewPassword { get; set; }
    }
}
