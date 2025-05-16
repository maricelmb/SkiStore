using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SkiStore.Controllers
{
    [Route("api/[controller]")] //http://localhost:5001/api/products
    [ApiController]
    public class ProductsController : ControllerBase
    {
    }
}
