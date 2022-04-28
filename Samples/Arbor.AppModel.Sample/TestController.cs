using Microsoft.AspNetCore.Mvc;

namespace Arbor.AppModel.Sample;

public class TestController : Controller
{
    [Route("~/")]
    public IActionResult Index()
    {
        return Ok();
    }
}