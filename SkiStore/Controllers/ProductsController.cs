using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkiStore.Data;
using SkiStore.Entities;

namespace SkiStore.Controllers
{
    [Route("api/[controller]")] //http://localhost:5001/api/products
    [ApiController]
    public class ProductsController(StoreContext context) : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Product>> GetProducts()
        { 
            return context.Products.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id) 
        {
            var product = context.Products.Find(id);

            if (product == null) return NotFound(); 

            return product;
        }
    }
}
