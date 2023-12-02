namespace Client.OcphAuthClient.Models
{
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

}