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
        [HttpGet]
        public async Task<ActionResult<Basket>> GetBasket()
        {
           var basket = await context.Baskets
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.BasketId == Request.Cookies["basketId"]);

            if(basket == null) return NotFound();

            return basket;
        }
    }
}
