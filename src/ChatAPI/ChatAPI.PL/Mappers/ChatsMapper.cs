using ChatAPI.DAL.Models;
using ChatAPI.PL.DTO;
using Riok.Mapperly.Abstractions;

namespace ChatAPI.PL.Mappers
{
    [Mapper]
    public partial class ChatsMapper
    {
        public partial Chat Map(ChatCreateDto chatCreateDto);
    }
}