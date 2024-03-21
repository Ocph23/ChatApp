using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public AdminController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/<AdminController>
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = dbContext.Users.Where(x => x.UserName.ToLower() != "admin@gmail.com");
            return Ok(users);
        }

        [HttpGet("groups")]
        public IActionResult GetGroup()
        {
            var groups = from x in dbContext.Group.Include(x => x.Pembuat)
                         select new GroupDTO
                         {
                             Description = x.Deskripsi,
                             NameGroup = x.Nama,
                             Owner = x.Pembuat.Name,
                             Created = x.TanggalBuat,
                             Id = x.Id,
                             JumlahAnggota = x.Anggota.Count(),
                         };
            return Ok(groups);
        }

        [HttpGet("documents")]
        public IActionResult GetDocuments()
        {
            var groups = from x in dbContext.PesanPrivat.Where(x => x.MessageType == MessageType.File)
                         select new DocumentDTO
                         {
                             Id = x.Id,
                             UrlFile = x.UrlFile.Substring(1,x.UrlFile.Length),
                             Tanggal = x.Tanggal.Value.ToString(),
                         };
            return Ok(groups);
        }

        [HttpPut("changestatus/{id}")]
        public IActionResult Put(string id, ApplicationUser value)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                user.EmailConfirmed = !value.EmailConfirmed;
                dbContext.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }





    }
}
