using Microsoft.AspNetCore.Mvc;

namespace PriceWatchApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceWatchController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            String data = "Test";
            return this.Ok(data);
        }
    }
}