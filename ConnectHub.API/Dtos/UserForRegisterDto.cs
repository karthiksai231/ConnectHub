using System.ComponentModel.DataAnnotations;

namespace ConnectHub.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Password Length must be between 4-8")]
        public string Password { get; set; }   
    }
} 