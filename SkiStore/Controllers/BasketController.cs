using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiStore.Data;
using SkiStore.Entities;

namespace SkiStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController(StoreContext context) : BaseApiController
    {
        private const string BasketCookieName = "basketId";

        [HttpGet]
        public async Task<ActionResult<Basket>> GetBasket()
        {
            var basket = await RetrieveBasket();

            if(basket == null) return NotFound();

            return basket;
        }

        [HttpPost]
        public async Task<ActionResult> AddItemToBasket(int productId, int quantity)
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

            if (result) return CreatedAtAction(nameof(GetBasket), basket);

            return BadRequest("Problem adding item to basket");
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
