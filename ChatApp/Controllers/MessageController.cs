﻿using ChatApp.Service;
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
                return Ok(data);
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
       
    }
}
