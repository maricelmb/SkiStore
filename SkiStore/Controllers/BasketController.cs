using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiStore.Data;
using SkiStore.DTOs;
using SkiStore.Entities;
using SkiStore.Extensions;

namespace SkiStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController(StoreContext context) : BaseApiController
    {
        private const string BasketCookieName = "basketId";

        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetrieveBasket();

            if(basket == null) return NotFound();

            return basket.ToDto();
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
        {
            //get basket
            var basket = await RetrieveBasket();
            //create basket if null
            basket ??= CreateBasket();

            //get product
            var product = await context.Products.FindAsync(productId);
            if (product == null) return BadRequest("Problem adding item to basket");

            //add item to basket
            basket.AddItem(product, quantity);

            //save changes
            var result = await context.SaveChangesAsync() > 0;

            if (result) return CreatedAtAction(nameof(GetBasket), basket.ToDto());

            return BadRequest("Problem adding item to basket");
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem(int productId, int quantity)
        {
            // get basket
            var basket = await RetrieveBasket();
           
            // remove the item or reduce its quantity
            if (basket == null) return BadRequest("Unable to retrieve basket.");

            basket.RemoveItem(productId, quantity);
            
            // save changes
            var result = await context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest("Problem updating basket.");
        }

        private Basket? CreateBasket()
        {
            var basketId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                Expires = DateTime.UtcNow.AddDays(30)
            };

            Response.Cookies.Append(BasketCookieName, basketId, cookieOptions);
            var basket = new Basket
            {
                BasketId = basketId
            };
            context.Baskets.Add(basket); // tell ef to track the new basket; not saved to db yet
            return basket;
        }

        private async Task<Basket?> RetrieveBasket()
        {
            return await context.Baskets
               .Include(x => x.Items)
               .ThenInclude(x => x.Product)
               .FirstOrDefaultAsync(x => x.BasketId == Request.Cookies[BasketCookieName]);
        }
    }
}
