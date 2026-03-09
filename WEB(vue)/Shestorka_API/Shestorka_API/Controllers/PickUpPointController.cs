using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shestorka_API.Models;

namespace Shestorka_API.Controllers
{
    public class PickUpPointController : ControllerBase
    {
        [HttpGet("address-pick-up-point")]
        public ActionResult GetAddress()
        {
            var data = ShesterochkaContext.Context.PickupPoints.Select(x => x.Address).ToList();
            return Ok(data);
        }
    }
}
