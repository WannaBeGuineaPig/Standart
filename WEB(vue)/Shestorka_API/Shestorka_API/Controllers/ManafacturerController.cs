using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shestorka_API.Models;

namespace Shestorka_API.Controllers
{
    public class ManafacturerController : ControllerBase
    {
        [HttpGet("manafacturer")]
        public ActionResult GetManafacturer()
        {
            return Ok(ShesterochkaContext.Context.Manafacturers.Select(obj => obj.Manafacturer1));
        }
    }
}
