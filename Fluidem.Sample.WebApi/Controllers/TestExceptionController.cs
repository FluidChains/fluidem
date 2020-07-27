using Microsoft.AspNetCore.Mvc;

namespace Fluidem.Sample.WebApi.Controllers
{
    public class TestExceptionController : Controller
    {
        // GET
        [HttpGet("api/exception-test")]
        public IActionResult Index()
        {
            int.Parse("parse exception");
            
            return Ok("OK");
        }
    }
}