namespace Client.OcphAuthClient.Models
{
    public record RegisterRequest(string? Name, string? Email, string? PhoneNumber, string? Password, string? ConfirmPassword, string? Role, string? PrivateKey);
}