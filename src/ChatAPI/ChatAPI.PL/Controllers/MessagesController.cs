using ChatAPI.BLL.Interfaces;
using ChatAPI.PL.DTO;
using ChatAPI.PL.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.PL.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController(IMessagesService messagesService) : ControllerBase
    {
        private readonly MessagesMapper _mapper = new();
        
        [HttpPost]
        public async Task<IActionResult> Create(MessageCreateDto messageCreateDto)
        {
            var messageToCreate = _mapper.Map(messageCreateDto);
            
            var message = await messagesService.CreateAsync(messageToCreate);
            
            return CreatedAtAction(nameof(GetById), new { id = message.Id }, message);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var message = await messagesService.GetAsync(id);
            
            return Ok(message);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, int userId)
        {
            await messagesService.RemoveAsync(id, userId);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, MessageCreateDto messageCreateDto)
        {
            var message = _mapper.Map(messageCreateDto);
            var updated = await messagesService.UpdateAsync(id, message);

            return Ok(updated);
        }
    }
}