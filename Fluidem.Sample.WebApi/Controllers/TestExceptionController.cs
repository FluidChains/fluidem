using Microsoft.AspNetCore.Mvc;

namespace Fluidem.Sample.WebApi.Controllers
{
    public class TestExceptionController : Controller
    {
        // GET
        [HttpGet("api/testException")]
        public IActionResult Index()
        {
            int.Parse("PRUEBA");
            return Ok("OK");
        }
    }
}