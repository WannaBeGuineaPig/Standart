using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Shoes_Demo_Ekz.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Shoes_Demo_Ekz.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddChangeWindow.xaml
    /// </summary>
    public partial class AddChangeWindow : Window
    {
        private Item? item;
        private byte[] newImageItem;
        private string letters = new String(Enumerable.Range('A', 26).Select(n => (char)n).ToArray());
        private List<string> types;
        private List<string> categories;
        private List<string> manafacturer;
        private List<string> suppliers;
        private List<string> unitOfMeasures = new List<string>() { "шт." };
        public AddChangeWindow(Item? item = null)
        {
            InitializeComponent();
            this.item = item;
            SetData();
        }

        private void SetData()
        {
            this.Title += item == null ? "добавление обуви" : "редактирование обуви";
            addChangeBTN.Content = item == null ? "Добавить обувь" : "Изменить обувь";
            
            types = ShoesContext.Context.TypeItems.Select(x => x.Type).ToList();
            categories = ShoesContext.Context.CategoryItems.Select(x => x.Category).ToList();
            manafacturer = ShoesContext.Context.Manafacturers.Select(x => x.Manafacturer1).ToList();
            suppliers = ShoesContext.Context.Suppliers.Select(x => x.Supplier1).ToList();

            typeItemCB.ItemsSource = types;
            categoryItemCB.ItemsSource = categories;
            manafacturerItemCB.ItemsSource = manafacturer;
            supplierItemCB.ItemsSource = suppliers;
            unitOfMeasureItemCB.ItemsSource = unitOfMeasures;

            if (item == null)
                return;

            imageItem.Source = item.Photo;
            articulItemTBL.Visibility = Visibility.Visible;
            deleteBTN.Visibility = Visibility.Visible;
            articulItemTBL.Text += item.Articul;
            typeItemCB.SelectedIndex = types.IndexOf(item.IdTypeNavigation.Type);
            categoryItemCB.SelectedIndex = categories.IndexOf(item.IdCategoryNavigation.Category);
            descriptionItemTB.Text = item.Description;
            manafacturerItemCB.SelectedIndex = manafacturer.IndexOf(item.IdManafacturerNavigation.Manafacturer1);
            supplierItemCB.SelectedIndex = suppliers.IndexOf(item.IdSupplierNavigation.Supplier1);
            priceItemTB.Text = item.Price.ToString();
            unitOfMeasureItemCB.SelectedIndex = unitOfMeasures.IndexOf(item.UnitOfMeasurement);
            amountInStorageItemTB.Text = item.AmountInStorage.ToString();
            discountItemTB.Text = item.DiscountPercent.ToString();
        }

        private void ExitAccountBTN_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void exitActionItem(System.ComponentModel.CancelEventArgs e)
        {
            if (this.DialogResult == true)
            {
                return;
            }
            string mess = $"Вы точно хотите выйти из окна " + (item == null ? "добавления товара?" : "редактирования товара?");
            MessageBoxResult boxResult = MessageBox.Show(mess, "Вопрос?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (boxResult == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }
        }

        private void addChangeBTN_Click(object sender, RoutedEventArgs e)
        {
            AddChangeItem();
        }

        private string? CheckError() // Функция для проверок на ошибки
        {
            if (descriptionItemTB.Text.Length == 0)
                return $"Не введёно описание товара!";
            
            if (priceItemTB.Text.Length == 0)
                return $"Не введена цена товара!";

            if (amountInStorageItemTB.Text.Length == 0)
                return $"Не введено количество товаров на складе!";

            if (discountItemTB.Text.Length == 0)
                return $"Не введена скидка товара!";

            int discountInt;
            int amointInStorageInt; 
            double priceDouble; 

            if (!Regex.IsMatch(priceItemTB.Text, @"^\d{1,6}([,.]\d{1,2})*$") || !double.TryParse(priceItemTB.Text, out priceDouble))
                return $"Не корректная цена!";

            if (!int.TryParse(amountInStorageItemTB.Text, out amointInStorageInt))
                return $"Не корректное количество товара на складе!";

            if (!int.TryParse(discountItemTB.Text, out discountInt))
                return $"Не корректная скидка товара!";

            if (0 > discountInt || discountInt > 99)
                return $"Скидка товара должна быть от 0 до 99!";

            if (0 > priceDouble)
                return $"Цена должна быть положительной!";

            if (0 > amointInStorageInt)
                return $"Количество на складе должно быть положительным!";

            return null;
        }
        
        private void AddChangeItem() // Функция для определения метода добавления или редактирования товара
        {
            string? error = CheckError();

            if (error != null)
            {
                MessageBox.Show(error, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (item == null)
                AddItem();
            else
                ChangeItem();

            this.DialogResult = true;
        }

        private void AddItem() // Функция для добавления товара
        {
            Item item = new Item();
            item.Articul = CreateArticul();
            if (newImageItem != null)
                item.Image = newImageItem;
            item.IdType = ShoesContext.Context.TypeItems.First(obj => obj.Type == typeItemCB.Text).Id;
            item.IdCategory = ShoesContext.Context.CategoryItems.First(obj => obj.Category == categoryItemCB.Text).Id;
            item.Description = descriptionItemTB.Text;
            item.IdManafacturer = ShoesContext.Context.Manafacturers.First(obj => obj.Manafacturer1 == manafacturerItemCB.Text).Id; 
            item.IdSupplier = ShoesContext.Context.Suppliers.First(obj => obj.Supplier1 == supplierItemCB.Text).Id; 
            item.Price = Convert.ToDouble(priceItemTB.Text); 
            item.UnitOfMeasurement = unitOfMeasureItemCB.Text; 
            item.AmountInStorage = Convert.ToInt32(amountInStorageItemTB.Text); 
            item.DiscountPercent = Convert.ToInt32(discountItemTB.Text);
            ShoesContext.Context.Items.Add(item);
            ShoesContext.Context.SaveChanges();
        }

        private void ChangeItem() // Функция для редактирования товара
        {
            Item item = ShoesContext.Context.Items.First(obj => obj.Articul == this.item.Articul);
            if (newImageItem != null)
                item.Image = newImageItem;
            item.IdType = ShoesContext.Context.TypeItems.First(obj => obj.Type == typeItemCB.Text).Id;
            item.IdCategory = ShoesContext.Context.CategoryItems.First(obj => obj.Category == categoryItemCB.Text).Id;
            item.Description = descriptionItemTB.Text;
            item.IdManafacturer = ShoesContext.Context.Manafacturers.First(obj => obj.Manafacturer1 == manafacturerItemCB.Text).Id;
            item.IdSupplier = ShoesContext.Context.Suppliers.First(obj => obj.Supplier1 == supplierItemCB.Text).Id;
            item.Price = Convert.ToDouble(priceItemTB.Text);
            item.UnitOfMeasurement = unitOfMeasureItemCB.Text;
            item.AmountInStorage = Convert.ToInt32(amountInStorageItemTB.Text);
            item.DiscountPercent = Convert.ToInt32(discountItemTB.Text);
            ShoesContext.Context.SaveChanges();
        }

        private string CreateArticul() // Функция для создания артикула
        {
            Random rand = new Random();
            string articul = $"{letters[rand.Next(0, letters.Length)]}{rand.Next(0, 10)}{rand.Next(0, 10)}{rand.Next(0, 10)}{letters[rand.Next(0, letters.Length)]}{rand.Next(0, 10)}";
            
            while (ShoesContext.Context.Items.FirstOrDefault(obj => obj.Articul == articul) != null)
                articul = $"{letters[rand.Next(0, letters.Length)]}{rand.Next(0, 10)}{rand.Next(0, 10)}{rand.Next(0, 10)}{letters[rand.Next(0, letters.Length)]}{rand.Next(0, 10)}";
            
            return articul;
        }

        private void changeImageBTN_Click(object sender, RoutedEventArgs e)
        {
            SetNewImage();
        }

        private void SetNewImage()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Title = "Выбор изображения товара";
            fileDialog.Filter = "Image Files|*.png;";
            
            if (fileDialog.ShowDialog() == false)
            {
                MessageBox.Show("Фотография не изменена.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            System.Drawing.Image image = System.Drawing.Image.FromFile(fileDialog.FileName);
            if (image.Width > 300 || image.Height > 200)
            {
                MessageBox.Show("Фотография должна иметь размер максимально 300x200!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                newImageItem = memoryStream.ToArray();
            }

            MessageBox.Show("Успешное добавление фотографии!", "Поздравляем!", MessageBoxButton.OK, MessageBoxImage.Information);
            BitmapImage imageView = new BitmapImage();
            imageView.BeginInit();
            imageView.StreamSource = new MemoryStream(newImageItem);
            imageView.EndInit();
            imageItem.Source = imageView;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            exitActionItem(e);
        }

        private void deleteBTN_Click(object sender, RoutedEventArgs e)
        {
            DeleteBtn();
        }

        private void DeleteBtn()
        {
            List<OrderItem> orderItems = ShoesContext.Context.OrderItems.Where(obj => obj.ArticulItem == item.Articul).ToList();
            if(orderItems.Count > 0)
            {
                MessageBox.Show("Данный товар находится в заказе(-ах), удаление невозможно!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ShoesContext.Context.Items.Remove(item);
            ShoesContext.Context.SaveChanges();
            this.DialogResult = true;
        }
    }
}
