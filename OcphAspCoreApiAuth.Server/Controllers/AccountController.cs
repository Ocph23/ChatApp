//using Microsoft.AspNetCore.Mvc;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace OcphApiAuth
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AccountController : ControllerBase
//    {
//        private readonly IAccountService<ApplicationUser> accountService;

//        public AccountController(IAccountService _accountService )
//        {
//            accountService = _accountService;
//        }
//        // POST api/<AccountController>
//        [HttpPost("login")]
//        public async Task<ActionResult> Login([FromBody] LoginRequest value)
//        {
//            try
//            {
//                var response = await accountService.Login(value);
//                ArgumentNullException.ThrowIfNull(response);
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return Unauthorized(ex.Message);
//            }
//        }

//        [HttpPost("register")]
//        public async Task<ActionResult> Register([FromBody] RegisterRequest value)
//        {
//            try
//            {
//                var response = await accountService.Register(value);
//                ArgumentNullException.ThrowIfNull(response);
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }
//    }
//}
