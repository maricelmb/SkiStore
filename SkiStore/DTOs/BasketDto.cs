using SkiStore.Entities;

namespace SkiStore.DTOs
{
    public class BasketDto
    {    
        //stored in cookie/local storage on client
        public required string BasketId { get; set; }

        public List<BasketItemDto> Items { get; set; } = [];
    }
}
