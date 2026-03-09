using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shestorka_API.DTO;
using Shestorka_API.Models;
using Shestorka_API.Moduls;
using System.Text.RegularExpressions;

namespace Shestorka_API.Controllers
{
    [ApiController]
    public class EatController : ControllerBase
    {
        [HttpGet("eats")]
        public ActionResult GetEats(string? search = null, string sorting = "по возрастанию", string filtering = "Все поставщики")
        {
            var data = ShesterochkaContext.Context.Eats
                .Include(obj => obj.IdTypeNavigation)
                .Include(obj => obj.IdCategoryNavigation)
                .Include(obj => obj.IdManafacturerNavigation)
                .Include(obj => obj.IdSupplierNavigation)
                .ToList();

            if(search != null)
            {
                Regex regex = new Regex(search, RegexOptions.IgnoreCase);
                data = data.Where(obj => EatModule.CheckDataEat(obj, regex)).ToList();
            }

            if(filtering != "Все поставщики")
            {
                data = data.Where(obj => obj.IdSupplierNavigation.Supplier1 == filtering).ToList();
            }

            if(sorting == "по возрастанию")
            {
                data = data.OrderBy(obj => obj.AmountInStorage).ToList();
            }

            else
            {
                data = data.OrderByDescending(obj => obj.AmountInStorage).ToList();
            }

            var resultData = data.Select(obj => new
            {
                obj.Articul,
                obj.IdCategoryNavigation.Category,
                obj.IdTypeNavigation.Type,
                obj.Description,
                obj.IdManafacturerNavigation.Manafacturer1,
                obj.IdSupplierNavigation.Supplier1,
                obj.Price,
                obj.DiscountPercent,
                obj.AmountInStorage,
                obj.Image,
            });
            return Ok(resultData);
        }

        [HttpPost("eat-ordering")]
        public ActionResult GetEatOrdering(List<string> articulItemOrdering)
        {
            var data = ShesterochkaContext.Context.Eats
                .Include(obj => obj.IdTypeNavigation)
                .Where(obj => articulItemOrdering.Contains(obj.Articul))
                .Select(obj => new
                {
                    ArticulItem = obj.Articul,
                    TypeItem = obj.IdTypeNavigation.Type,
                    PriceItem = obj.Price,
                    DiscountItem = obj.DiscountPercent,
                    AmountInStorageItem = obj.AmountInStorage,
                    ImageItem = obj.Image,
                })
                .ToList();
            return Ok(data);
        }

        [HttpGet("item-on-articul")]
        public ActionResult GetItemOnArticul(string articul)
        {
            Eat? eat = ShesterochkaContext.Context.Eats
                .Include(obj => obj.IdTypeNavigation)
                .Include(obj => obj.IdCategoryNavigation)
                .Include(obj => obj.IdManafacturerNavigation)
                .Include(obj => obj.IdSupplierNavigation)
                .FirstOrDefault(obj => obj.Articul == articul);
            if (eat == null)
                return NotFound("Товар не найден!");
            return Ok(new {
                eat.Articul,
                eat.IdCategoryNavigation.Category,
                eat.IdTypeNavigation.Type,
                eat.Description,
                eat.IdManafacturerNavigation.Manafacturer1,
                eat.IdSupplierNavigation.Supplier1,
                eat.Price,
                eat.DiscountPercent,
                eat.AmountInStorage,
                eat.Image,
            });
        }

        [HttpPost("add-eat")]
        public ActionResult AddEats(EatClass newEat)
        {
            var checkEat = EatModule.CheckChangeEat(newEat);
            if (checkEat.Item1 == null)
                return BadRequest(checkEat.Item2);

            ShesterochkaContext.Context.Eats.Add(checkEat.Item1);
            ShesterochkaContext.Context.SaveChanges();
            return Ok("Товар успешно добавлен!");
        }

        [HttpPut("change-eat")]
        public ActionResult ChangeEats(EatClass newEat)
        {
            if (newEat.Articul == null)
                return BadRequest("Не ввёден артикул!");

            var checkEat = EatModule.CheckChangeEat(newEat);
            if (checkEat.Item1 == null)
                return BadRequest(checkEat.Item2);

            ShesterochkaContext.Context.SaveChanges();
            return Ok("Товар успешно изменён!");
        }

        [HttpDelete("delete-eat")]
        public ActionResult DeleteEats(string articul)
        {
            Eat? eat = ShesterochkaContext.Context.Eats.FirstOrDefault(obj => obj.Articul == articul);
            if (eat == null)
                return NotFound("Товар не найден!");

            int countOrderItem = ShesterochkaContext.Context.OrderItems.Where(obj => obj.ArticulItem == articul).Count();
            if (countOrderItem > 0)
                return BadRequest("Данный товар находится в заказе(-ах), удаление невозможно!");

            ShesterochkaContext.Context.Eats.Remove(eat);
            ShesterochkaContext.Context.SaveChanges();
            return Ok("Товар успешно удалён!");
        }
    }
}
