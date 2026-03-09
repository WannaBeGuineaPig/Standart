using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shestorka_API.Models;

namespace Shestorka_API.Controllers
{
    public class TypeController : ControllerBase
    {
        [HttpGet("type")]
        public ActionResult GetTypes()
        {
            return Ok(ShesterochkaContext.Context.TypeEats.Select(obj => obj.Type));
        }
    }
}
