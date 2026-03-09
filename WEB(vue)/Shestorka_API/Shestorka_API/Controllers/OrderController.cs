using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shestorka_API.DTO;
using Shestorka_API.Models;
using System.Linq;

namespace Shestorka_API.Controllers
{
    public class OrderController : ControllerBase
    {
        [HttpGet("order")]
        public ActionResult GetOrders(int idUser = -1)
        {
            var data = ShesterochkaContext.Context.Orders
            .Include(obj => obj.IdPickupPointNavigation)
            .ToList();

            if (idUser != -1)
            {
                if (ShesterochkaContext.Context.Users.FirstOrDefault(obj => obj.Id == idUser) == null) return NotFound("Пользователь не найден!");
                data = data.Where(obj => obj.IdUser == idUser).ToList();
            }

            var resultData = data.Select(obj => new
            {
                IdOrder = obj.Id,
                StatusOrder = obj.Status,
                AddressPickUpPoint = obj.IdPickupPointNavigation.Address,
                DateOrdering = (obj.DateOrdering.Day > 9 ? obj.DateOrdering.Day.ToString() : $"0{obj.DateOrdering.Day}") + "." + (obj.DateOrdering.Month > 9 ? obj.DateOrdering.Month.ToString() : $"0{obj.DateOrdering.Month}") +
            "." + obj.DateOrdering.Year.ToString(),
                DateDelivery = (obj.DateDelivery.Day > 9 ? obj.DateDelivery.Day.ToString() : $"0{obj.DateDelivery.Day}") + "." + (obj.DateDelivery.Month > 9 ? obj.DateDelivery.Month.ToString() : $"0{obj.DateDelivery.Month}") +
            "." + obj.DateDelivery.Year.ToString(),
                obj.CodePickup
            })
            .OrderByDescending(obj => obj.IdOrder)
            .ToList();
            return Ok(resultData);
        }

        [HttpPost("new-order")]
        public ActionResult AddNewOrder([FromBody] OrderClass orderClass)
        {
            var user = ShesterochkaContext.Context.Users.FirstOrDefault(obj => obj.Id == orderClass.IdUser);
            if (user == null)
            {
                return NotFound("Пользователь не найден!");
            }
            var pickUpPoint = ShesterochkaContext.Context.PickupPoints.FirstOrDefault(obj => obj.Address == orderClass.AddressPickUpPoint);
            if (pickUpPoint == null)
            {
                return NotFound("Пункт выдачи не найден!");
            }
            Order order = new Order()
            {
                Id = 0,
                IdUser = orderClass.IdUser,
                DateOrdering = orderClass.DateOrdering,
                DateDelivery = orderClass.DateDelivery,
                IdPickupPoint = pickUpPoint.Id,
                CodePickup = ShesterochkaContext.Context.Orders.Max(obj => obj.CodePickup) + 1,
                Status = orderClass.Status
            };
            ShesterochkaContext.Context.Orders.Add(order);
            ShesterochkaContext.Context.SaveChanges();

            foreach (var item in orderClass.ItemOrder)
            {
                Eat? eat = ShesterochkaContext.Context.Eats.FirstOrDefault(obj => obj.Articul == item.Key);
                if (eat == null)
                {
                    return NotFound("Еда в заказе не найдена!");
                }
                if(item.Value <= 0)
                {
                    return BadRequest("Количество товара в заказе должно быть больше 0!");
                }
                if (eat.AmountInStorage < item.Value)
                {
                    return BadRequest("Количество еды в заказе превышает на складе!");
                }
                eat.AmountInStorage -= item.Value;
                OrderItem orderItem = new OrderItem()
                {
                    IdOrder = order.Id,
                    ArticulItem = item.Key,
                    AmountItem = item.Value,
                };
                ShesterochkaContext.Context.OrderItems.Add(orderItem);
            }

            ShesterochkaContext.Context.SaveChanges();
            return NoContent();
        }
    }
}
