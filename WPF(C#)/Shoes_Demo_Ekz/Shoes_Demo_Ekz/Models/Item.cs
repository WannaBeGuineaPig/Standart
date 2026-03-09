using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace Shoes_Demo_Ekz.Models;

public partial class Item
{
    public string Articul { get; set; } = null!;

    public int IdType { get; set; }

    public string UnitOfMeasurement { get; set; } = null!;

    public double Price { get; set; }

    public int IdSupplier { get; set; }

    public int IdManafacturer { get; set; }

    public int IdCategory { get; set; }

    public int? DiscountPercent { get; set; }

    public int AmountInStorage { get; set; }

    public string Description { get; set; } = null!;

    public byte[]? Image { get; set; }
    public BitmapImage Photo
    {
        get
        {
            BitmapImage image = new BitmapImage();
            Uri baseUrl = new Uri("../Images/picture.png", UriKind.Relative);
            image.BeginInit();
            if (Image == null)
            {
                image.UriSource = baseUrl;
            }
            else
            {
                MemoryStream mem = new MemoryStream(Image);
                image.StreamSource = mem;
            }
            image.EndInit();
            return image;
        }
    }

    public string DiscountString
    {
        get
        {
            return $"{(DiscountPercent > 0 ? DiscountPercent : 0)}%";
        }
    }

    public string ColorAmountInStorage
    {
        get
        {
            if (AmountInStorage == 0)
            {
                return "#FF00FFFF";
            }
            return "Transparent";
        }
    }

    public string NewPrice
    {
        get
        {
            if (DiscountPercent == 0) return "";
            return $" {Math.Round((double) (Price - (Price / 100 * DiscountPercent)) * 100) / 100}";
        }
    }

    public string BigDiscount
    {
        get
        {
            if (DiscountPercent > 14) return "#2E8B57";
            return "Transparent";
        }
    }

    public string ColorPrice
    {
        get
        {
            if (DiscountPercent == 0) return "Black";
            return "Red";
        }
    }
    public string StylePrice
    {
        get
        {
            if (DiscountPercent == 0) return "None";
            return "Strikethrough";
        }
    }

    public virtual CategoryItem IdCategoryNavigation { get; set; } = null!;

    public virtual Manafacturer IdManafacturerNavigation { get; set; } = null!;

    public virtual Supplier IdSupplierNavigation { get; set; } = null!;

    public virtual TypeItem IdTypeNavigation { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
