using ChatAPI.BLL.Interfaces;
using ChatAPI.PL.DTO;
using ChatAPI.PL.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.PL.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUsersService usersService) : ControllerBase
    {
        private readonly UsersMapper _mapper = new();
        
        [HttpPost]
        public async Task<IActionResult> Register(UserCreateDto userCreateDto)
        {
            var userToCreate = _mapper.Map(userCreateDto);
            
            var user = await usersService.RegisterAsync(userToCreate);
            
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await usersService.GetAsync(id);
            
            return Ok(user);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await usersService.RemoveAsync(id);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UserCreateDto userCreateDto)
        {
            var user = _mapper.Map(userCreateDto);
            var updated = await usersService.UpdateAsync(id, user);

            return Ok(updated);
        }
    }
}