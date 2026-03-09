using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shestorka_API.Models;

namespace Shestorka_API.Controllers
{
    public class SupplierController : ControllerBase
    {
        [HttpGet("supplier")]
        public ActionResult GetSupplier()
        {
            return Ok(ShesterochkaContext.Context.Suppliers.Select(obj => obj.Supplier1));
        }
    }
}
