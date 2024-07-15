using ChatAPI.BLL.Interfaces;
using ChatAPI.BLL.Services;
using ChatAPI.PL.DTO;
using ChatAPI.PL.Hubs;
using ChatAPI.PL.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatAPI.PL.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController(IChatsService chatsService, IHubContext<ChatHub> chatHub) : ControllerBase
    {
        private readonly ChatsMapper _mapper = new();
        
        [HttpPost]
        public async Task<IActionResult> Create(ChatCreateDto chatCreateDto)
        {
            if (chatCreateDto.Name is null || await chatsService.ExistsWithName(chatCreateDto.Name))
            {
                return BadRequest("Invalid name or name already exists");
            }
            
            var chatToCreate = _mapper.Map(chatCreateDto);
            
            var chat = await chatsService.CreateAsync(chatToCreate);
            
            return CreatedAtAction(nameof(GetById), new { id = chat.Id }, chat);
        }

        [HttpGet]
        public async Task<IActionResult> GetForUser(int userId)
        {
            var chats = await chatsService.GetForUserAsync(userId);
            
            return Ok(chats);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var chat = await chatsService.GetAsync(id);
            
            return Ok(chat);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, int userId)
        {
            await chatsService.RemoveAsync(id, userId);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ChatCreateDto chatCreateDto)
        {
            var chat = _mapper.Map(chatCreateDto);
            var updated = await chatsService.UpdateAsync(id, chat);

            return Ok(updated);
        }
    }
}