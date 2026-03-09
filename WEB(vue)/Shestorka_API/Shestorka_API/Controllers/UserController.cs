using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shestorka_API.DTO;
using Shestorka_API.Models;
using System.Text.RegularExpressions;

namespace Shestorka_API.Controllers
{
    public class UserController : ControllerBase
    {
        [HttpGet("users")]
        public ActionResult GetUsers()
        {
            return Ok(ShesterochkaContext.Context.Users.ToList());
        }
        [HttpPost("authorization")]
        public ActionResult Authorization([FromBody] AuthorizationUser authUser)
        {
            if (authUser.Mail.Length == 0 || authUser.Password.Length == 0)
                return BadRequest("Не все поля введены!");

            if (!Regex.IsMatch(authUser.Mail, "^[a-z0-9]{3,}[@][a-z]{3,}[.][a-z]{3,}$", RegexOptions.IgnoreCase))
                return BadRequest("Не корректная почта!");

            if (!Regex.IsMatch(authUser.Password, "^[a-z]{5,}$", RegexOptions.IgnoreCase))
                return BadRequest("Не корректный пароль!");

            User? user = ShesterochkaContext.Context.Users.FirstOrDefault(obj => obj.Mail == authUser.Mail && obj.Password  == authUser.Password);

            if (user == null)
                return NotFound("Неверная почта или пароль, либо такого пользователя не существует!");
            
            return Ok(new
            {
                user.Id,
                user.Mail,
                user.Role,
                user.LastName,
                user.FirstName,
                user.MidleName,
            });
        }
    }
}
