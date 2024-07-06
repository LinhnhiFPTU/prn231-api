using System.ComponentModel.DataAnnotations;

namespace PRN231.API.Payload.Request.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    [MaxLength(50, ErrorMessage = "Username's max length is 50 characters")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [MaxLength(50, ErrorMessage = "Password's max length is 50 characters")]
    public string Password { get; set; } = null!;
}