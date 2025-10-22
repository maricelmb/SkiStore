using SkiStore.DTOs;
using SkiStore.Entities;

namespace SkiStore.Extensions
{
    public static class BasketExtensions
    {
        public static BasketDto ToDto(this Basket basket)
        {
            return new BasketDto
            {
                BasketId = basket.BasketId,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    PictureUrl = item.Product.PictureUrl,
                    Quantity = item.Quantity,
                    Brand = item.Product.Brand,
                    Type = item.Product.Type
                }).ToList()
            };
        }
        }
}
