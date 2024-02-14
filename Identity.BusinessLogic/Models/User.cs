using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Identity.Core.Models
{
    public class User : IdentityUser
    {
        [Url]
        public string? ImageUrl { get; set; }
    }
}
