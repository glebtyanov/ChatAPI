using ChatAPI.DAL.Models;
using ChatAPI.PL.DTO;
using Riok.Mapperly.Abstractions;

namespace ChatAPI.PL.Mappers
{
    [Mapper]
    public partial class MessagesMapper
    {
        public partial Message Map(MessageCreateDto messageCreateDto);
    }
}