using Microsoft.AspNetCore.Mvc;
using OcphApiAuth;
using Shared;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService<ApplicationUser> accountService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment env;

        public AccountController(IAccountService<ApplicationUser> _accountService, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            accountService = _accountService;
            this.env = env;
        }
        // POST api/<AccountController>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest value)
        {
            try
            {
                var response = await accountService.Login(value.UserName, value.Password);
                ArgumentNullException.ThrowIfNull(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest value)
        {
            try
            {
                var user = new ApplicationUser(value.Email) { Email = value.Email, EmailConfirmed = true, Name = value.Name };
                using (ECDiffieHellmanCng alice = new ECDiffieHellmanCng())
                {

                    alice.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                    alice.HashAlgorithm = CngAlgorithm.Sha256;
                    user.PublicKey = Convert.ToBase64String(alice.PublicKey.ToByteArray());
                }
                var response = await accountService.Register(user, value.Role, value.Password);
                ArgumentNullException.ThrowIfNull(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("profile")]
        public async Task<ActionResult> GetProfile()
        {
            try
            {
                var userId = User.GetUserId();
                var user = await accountService.FindUserById(userId);
                ArgumentNullException.ThrowIfNull(user);
                return Ok(new UserDTO { Id = user.Id, Email = user.Email, Nama = user.Name, Telepon = user.PhoneNumber, Photo = user.Photo, Active = user.EmailConfirmed });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("publickey/{id}")]
        public async Task<ActionResult> GetPublicKey(string id)
        {
            try
            {
                var user = await accountService.FindUserById(id);
                ArgumentNullException.ThrowIfNull(user);
                return Ok(user.PublicKey);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("profile/{id}")]
        public async Task<ActionResult> PutProfile(string id, UserDTO userdto)
        {
            try
            {
                var user = await accountService.FindUserById(id);
                ArgumentNullException.ThrowIfNull(user);
                user.EmailConfirmed = userdto.Active;
                user.Name = userdto.Nama;
                user.PhoneNumber = userdto.Telepon;
                user.Email = userdto.Email;
                var result = await accountService.UpdateUser(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("photo/{id}")]
        public async Task<IActionResult> Upload(string id, IList<IFormFile> files)
        {
            var user = await accountService.FindUserById(id);
            ArgumentNullException.ThrowIfNull(user, "user tidak ditemukan");
            var httprequest = HttpContext.Request.Form.Files;
            string filex = string.Empty;
            foreach (var file in httprequest)
            {
                var fileType = Path.GetExtension(file.FileName);
                //var ext = file.;
                if (fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" || fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg")
                {
                    var filePath = Path.Combine(env.ContentRootPath, "wwwroot/profile");
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    if (file != null && file.Length > 0)
                    {
                        var DocUrl = Path.Combine(filePath, id.ToString() + fileType);
                        var image = System.Drawing.Image.FromStream(file.OpenReadStream());
                        var resized = new Bitmap(image, new Size(50, 50));
                        using var imageStream = new MemoryStream();
                        resized.Save(imageStream, ImageFormat.Png);
                        var imageBytes = imageStream.ToArray();
                        using (var stream = new FileStream(DocUrl, FileMode.Create, FileAccess.Write, FileShare.Write, imageBytes.Length))
                        {
                            stream.Write(imageBytes, 0, imageBytes.Length);
                        }

                        user.Photo = id.ToString() + fileType;
                        bool saved = await accountService.UpdateUser(user);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            return Ok(filex);
        }

    }
}
