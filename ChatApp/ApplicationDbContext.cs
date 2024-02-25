using ChatApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Pertemanan> Pertemanan { get; set; }
    public DbSet<Group> Group { get; set; }
    public DbSet<AnggotaGroup> AnggotaGroup { get; set; }
    public DbSet<MessagePrivate> PesanPrivat { get; set; }
    public DbSet<MessageGroup> PesanGroup { get; set; }
}
