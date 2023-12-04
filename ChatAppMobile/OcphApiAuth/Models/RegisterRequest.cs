namespace Client.OcphAuthClient.Models
{
    public record RegisterRequest(string? Email, string? PhoneNumber, string? Password, string? ConfirmPassword, string? Role);
}