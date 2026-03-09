using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shestorka_API.Models;

namespace Shestorka_API.Controllers
{
    public class CategoryController : ControllerBase
    {
        [HttpGet("category")]
        public ActionResult GetCategory()
        {
            return Ok(ShesterochkaContext.Context.CategoryEats.Select(obj => obj.Category));
        }
    }
}
