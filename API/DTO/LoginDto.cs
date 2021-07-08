using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class LoginDto
    {
        [Required]
        public string username { get; set; }
        
       [Required]
        public string Password { get; set; }
    }
}