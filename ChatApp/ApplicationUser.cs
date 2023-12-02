using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser(string userName) : base(userName)
    {
    }

    public ApplicationUser()
    {

    }

    public string? Name { get; set; }
}