using Microsoft.AspNetCore.Mvc;

namespace Arbor.AppModel.Sample;

public class TestController : Controller
{
    public TestController()
    {

    }

    [HttpGet]
    [Route("~/")]
    public IActionResult Index() => Ok(new {Message="Hello world"});
}