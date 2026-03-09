using Shestorka_API.DTO;
using Shestorka_API.Models;
using System.Text.RegularExpressions;

namespace Shestorka_API.Moduls
{
    public static class EatModule
    {
        private static string letters = new String(Enumerable.Range('A', 26).Select(n => (char)n).ToArray());
        public static string CreateArticul()
        {
            Random rand = new Random();
            string articul = $"{letters[rand.Next(0, letters.Length)]}{rand.Next(0, 10)}{rand.Next(0, 10)}{rand.Next(0, 10)}{letters[rand.Next(0, letters.Length)]}{rand.Next(0, 10)}";

            while (ShesterochkaContext.Context.Eats.FirstOrDefault(obj => obj.Articul == articul) != null)
            {
                articul = $"{letters[rand.Next(0, letters.Length)]}{rand.Next(0, 10)}{rand.Next(0, 10)}{rand.Next(0, 10)}{letters[rand.Next(0, letters.Length)]}{rand.Next(0, 10)}";
            }

            return articul;
        }

        public static (Eat?, string?) CheckChangeEat(EatClass newEat)
        {
            TypeEat? type = ShesterochkaContext.Context.TypeEats.FirstOrDefault(obj => obj.Type == newEat.Type);
            if (type == null)
                return (null, "Не корректный тип товара!" );

            string? unitOfMeasurement = new List<string> { "шт." }.FirstOrDefault(obj => obj == newEat.UnitOfMeasurement);
            if (unitOfMeasurement == null)
                return (null, "Не корректная единица товара!");

            Supplier? supplier = ShesterochkaContext.Context.Suppliers.FirstOrDefault(obj => obj.Supplier1 == newEat.Supplier);
            if (supplier == null)
                return (null, "Не корректный поставщик товара!");

            Manafacturer? manafacturer = ShesterochkaContext.Context.Manafacturers.FirstOrDefault(obj => obj.Manafacturer1 == newEat.Manafacturer);
            if (manafacturer == null)
                return (null, "Не корректный производитель товара!");

            CategoryEat? category = ShesterochkaContext.Context.CategoryEats.FirstOrDefault(obj => obj.Category == newEat.Category);

            if (category == null)
                return (null, "Не корректная категория товара!");

            if (newEat.Price <= 0)
                return (null, "Не корректная цена товара!");

            if (newEat.AmountInStorage < 0)
                return (null, "Не корректное количество товара на складе!");

            if (newEat.DiscountPercent < 0 || newEat.DiscountPercent > 99)
                return (null, "Не корректная скидка товара!");

            Eat eat; 
            if (newEat.Articul == null)
            {
                eat = new Eat();
                eat.Articul = CreateArticul();
            }
            else
            {
                eat = ShesterochkaContext.Context.Eats.First(obj => obj.Articul == newEat.Articul);
            }
            eat.IdType = type.Id;
            eat.UnitOfMeasurement = unitOfMeasurement;
            eat.IdSupplier = supplier.Id;
            eat.IdManafacturer = manafacturer.Id;
            eat.IdCategory = category.Id;
            eat.Price = newEat.Price;
            eat.AmountInStorage = newEat.AmountInStorage;
            eat.DiscountPercent = newEat.DiscountPercent;
            eat.Description = newEat.Description;
            if (newEat.Image != null)
                eat.Image = newEat.Image;
            return (eat, null);
        }

        public static bool CheckDataEat(Eat eat, Regex regex)
        {
            return regex.IsMatch(eat.Articul) ||
                regex.IsMatch(eat.IdTypeNavigation.Type) ||
                regex.IsMatch(eat.UnitOfMeasurement) ||
                regex.IsMatch(eat.IdSupplierNavigation.Supplier1) ||
                regex.IsMatch(eat.IdManafacturerNavigation.Manafacturer1) ||
                regex.IsMatch(eat.IdCategoryNavigation.Category) ||
                regex.IsMatch(eat.Description);
        }

    }
}
