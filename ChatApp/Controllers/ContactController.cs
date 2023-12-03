using ChatApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OcphApiAuth;
using Shared;
using Shared.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatApp
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService contactService;

        public ContactController(IContactService _contactService)
        {
            contactService = _contactService;
        }

        [HttpGet]
        public async Task<ActionResult<Contact>> Get()
        {
            try
            {
                var user = User.GetUserId();
                var result = await contactService.Get(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("addanggota/{groupid}/{userId}")]
        public async Task<ActionResult<bool>> AddAnggota(int groupid, string userId)
        {
            try
            {
                var result = await contactService.AddAnggota(groupid, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("addtemanbyuserName/{userName}")]
        public async Task<ActionResult<TemanDTO>> AddTemanByUserName(string userName)
        {
            try
            {
                var user = User.GetUserId();
                var result = await contactService.AddTemanByUserName(user, userName.ToLower());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("addteman/{temanId}")]
        public async Task<ActionResult<TemanDTO>> AddTeman(string temanId)
        {
            try
            {
                var user = User.GetUserId();
                var result = await contactService.AddTeman(user,temanId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("creategroup")]
        public async Task<ActionResult<GroupDTO>> CreateGroup(GroupDTO group)
        {
            try
            {
                var user = User.GetUserId();
                var result = await contactService.CreateGroup(user, group);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("teman/{temanId}")]
        public async Task<ActionResult<bool>> DeleteTeman(string userid, string temanId)
        {
            try
            {
                var user = User.GetUserId();
                var result = await contactService.DeleteTeman(user, temanId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<GroupDTO>> GetGroup(int groupId)
        {
            try
            {
                var user = User.GetUserId();
                var result = await contactService.GetGroup(groupId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
