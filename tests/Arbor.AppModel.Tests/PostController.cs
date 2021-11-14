using Arbor.AppModel.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Arbor.AppModel.Tests
{
    public class PostController : Controller
    {
        [Route("~/")]
        [HttpPost]
        public IActionResult Index([FromBody] NoValidationAttributeModel _) => Ok();

        [Route("~/no-validation")]
        [HttpPost]
        public IActionResult NoValidation([NoValidation] [FromBody] NoValidationModel _) => Ok();

        [Route("~/validation")]
        [HttpPost]
        public IActionResult Validation([FromBody] NoValidationModel _) => Ok();
    }
}