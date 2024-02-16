using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Identity.DataAccess.Models
{
    public class User : IdentityUser
    {
        [Url]
        public string? ImageUrl { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
    }
}
