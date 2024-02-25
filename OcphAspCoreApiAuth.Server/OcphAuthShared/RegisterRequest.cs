namespace OcphApiAuth
{
    public class RegisterRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string PrivateKey { get; set; } = string.Empty;
    }

}