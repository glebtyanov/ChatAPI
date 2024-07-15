using ChatAPI.BLL.Interfaces;
using ChatAPI.PL.DTO;
using ChatAPI.PL.Hubs;
using ChatAPI.PL.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController(IChatsService chatsService, ChatHub chatHub) : ControllerBase
    {
        private readonly ChatsMapper _mapper = new();

        [HttpPost]
        public async Task<IActionResult> Create(ChatCreateDto chatCreateDto)
        {
            var chatToCreate = _mapper.Map(chatCreateDto);

            var chat = await chatsService.CreateAsync(chatToCreate);

            return CreatedAtAction(nameof(GetById), new { id = chat.Id }, chat);
        }

        [HttpGet]
        public async Task<IActionResult> GetForUser(int userId)
        {
            var chats = await chatsService.GetWhereUserIsAdminAsync(userId);

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
            var chatToRemove = await chatsService.GetAsync(id);

            await chatHub.RemoveChatOnServer(chatToRemove);

            await chatsService.RemoveAsync(chatToRemove.Id, chatToRemove.AdminId);

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