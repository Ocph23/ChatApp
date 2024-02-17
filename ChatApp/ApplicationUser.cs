using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser(string userName) : base(userName)
    {
    }

    public ApplicationUser() { }

    public string? Name { get; set; }

    public string? PrivateKey { get; set; }

    public string? PublicKey { get; set; }

    public string? Photo { get; set; }
}