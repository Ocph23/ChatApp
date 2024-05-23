using ChatApp.Service;
using Microsoft.AspNetCore.Mvc;
using OcphApiAuth;
using Shared;
using Shared.Contracts;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase      
    {
        private readonly IMessageService messageService;

        public MessageController(IMessageService messageService)
        {
           this.messageService = messageService;
        }

        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<IEnumerable<MessageGroup>>> GetGroupMessage(int groupId)
        {
            try
            {
                var data = await messageService.GetGroupMessage(groupId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("private/{userid1}/{userid2}")]
        public async Task<ActionResult<IEnumerable<MessagePrivate>>> GetPrivateMessage(string? userid1, string userid2)
        {
            try
            {
                var data = await messageService.GetPrivateMessage(userid1,userid2);
                return Ok(data.OrderBy(x=>x.Tanggal));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("read/{temainId}/{mydId}")]
        public async Task<ActionResult> RaadMessage(string? temainId, string mydId)
        {
            try
            {
                await messageService.ReadMassage(temainId, mydId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("group")]
        public async Task<ActionResult<MessageGroup>> PostGroupMessage(MessageGroup mesage)
        {
            try
            {
                var data = await messageService.PostGroupMessage(mesage);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("private")]
        public async Task<ActionResult<MessagePrivate>> PostPrivateMessage(MessagePrivate message)
        {
            try
            {
                var data = await messageService.PostPrivateMessage(message);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("private/{id}")]
        public async Task<ActionResult<bool>> DeletePrivate(int id)
        {
            try
            {
                bool result= await messageService.DeletePrivate(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpDelete("group/{id}")]
        public async Task<ActionResult<bool>> DeleteGroup(int id)
        {
            try
            {
                bool result = await messageService.DeleteGroup(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
