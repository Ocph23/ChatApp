using ChatApp.SignalRApp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OcphAspCoreApiAuth.Server;
using ChatApp.Service;
using OcphApiAuth;
using Microsoft.OpenApi.Models;
using Shared.Contracts;
using Amazon.S3;
using Amazon.S3.Util;
using Microsoft.AspNetCore.HttpOverrides;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;

var builder = WebApplication.CreateBuilder(args);


if (builder.Environment.IsProduction())
{
    builder.WebHost.UseKestrel(serverOptions =>
    {
        serverOptions.ListenLocalhost(5028);
    });
}

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "all",
                      policy =>
                      {
                          policy.WithOrigins("*");
                      });
});



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication()
.AddIdentityServerJwt();
builder.Services.TryAddEnumerable(
    ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>,
        ConfigureJwtBearerOptions>());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});
builder.Services.AddSignalR();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
          new[] { "application/octet-stream" });
});

builder.Services.AddScoped<IAccountService<ApplicationUser>, AccountService<ApplicationUser, ApplicationDbContext>>();

builder.Services.AddOcphAuthServe(builder.Configuration);


builder.Services.AddSingleton<IChatUserManager, ChatUserManager>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IMessageService, MessageService>();
AWSOptions awsOptions = new AWSOptions
{
    Credentials = new BasicAWSCredentials("AKIA5FTY64XH7B4H755C", "gGjAvZC/cJHEUMPGxtVSsiV5ZbXvNX9GRaJCajB6"),
    Region = Amazon.RegionEndpoint.APSoutheast2
};
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonS3>(awsOptions);
var app = builder.Build();

//database seed


using (var scope = app.Services.CreateScope())
{
    var dbcontext = scope.ServiceProvider.GetService<ApplicationDbContext>();
    dbcontext.Database.EnsureCreated();

    var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

    if (!dbcontext.Roles.Any())
    {
        await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        await roleManager.CreateAsync(new IdentityRole { Name = "User" });
    }

    if (!dbcontext.Users.Any())
    {
        var user = new ApplicationUser("admin@gmail.com") { Name = "Admin", Email = "admin@gmail.com", EmailConfirmed = true };
        var result = await userManager.CreateAsync(user, "Password@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
        else
        {
            await userManager.DeleteAsync(user);
        }
    }
}

app.UseResponseCompression();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.UseBlazorFrameworkFiles();
app.MapFallbackToFile("index.html");
app.MapControllers();
app.MapHub<ChatHub>("/chathub");

app.Run();
