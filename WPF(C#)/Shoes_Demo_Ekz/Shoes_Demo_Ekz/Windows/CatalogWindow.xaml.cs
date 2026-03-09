using Microsoft.EntityFrameworkCore;
using Shoes_Demo_Ekz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для CatalogWindow.xaml
    /// </summary>
    public partial class CatalogWindow : Window
    {
        private User? user;
        private List<string> sortingList = new List<string>()
        {
            "по возрастанию", "по убыванию"
        };
        private List<string> listSupplier;
        public CatalogWindow(User? user)
        {
            InitializeComponent();
            this.user = user;
            SetDataList();
            SetDataStartWindow();
        }
        private void SetDataStartWindow() // Установка данных в момент открытия окна
        {
            if (user == null)
            {
                RoleUserTBL.Text = "Гость";
                return;
            }
                RoleUserTBL.Text = user.Role;
                FIOUserTBL.Text = $"{user.LastName} {user.FirstName} {user.MidleName}";
            
            if (user.Role != "Авторизированный клиент")
            {
                changeOutDataSP.Visibility = Visibility.Visible;
                OrdersBTN.Visibility = Visibility.Visible;
            }


            if (user.Role == "Администратор")
                addShoesBTN.Visibility = Visibility.Visible;

            sortingShoesCB.ItemsSource = sortingList;

            listSupplier = ShoesContext.Context.Suppliers.Select(obj => obj.Supplier1).ToList();
            listSupplier.Insert(0, "Все поставщики");
            filteringShoesCB.ItemsSource = listSupplier;
        }

        private void SetDataList(string search = "", string sorting = "по возрастанию", string filtering = "Все поставщики")
        {
            List<Item> items = ShoesContext.Context.Items
                .Include(obj => obj.IdCategoryNavigation)
                .Include(obj => obj.IdTypeNavigation)
                .Include(obj => obj.IdManafacturerNavigation)
                .Include(obj => obj.IdSupplierNavigation)
                .ToList();

            if (search.Length > 0) // Поиск по всем атрибутам
            {
                Regex regex = new Regex(search, RegexOptions.IgnoreCase);
                items = items.Where(obj => regex.IsMatch(obj.Articul) 
                || regex.IsMatch(obj.IdTypeNavigation.Type) 
                || regex.IsMatch(obj.UnitOfMeasurement)
                || regex.IsMatch(obj.IdSupplierNavigation.Supplier1) 
                || regex.IsMatch(obj.IdManafacturerNavigation.Manafacturer1)
                || regex.IsMatch(obj.IdCategoryNavigation.Category) 
                || regex.IsMatch(obj.Description))
                .ToList();
            }

            if (filtering != "Все поставщики") // Фильтрация
            {
                items = items.Where(obj => obj.IdSupplierNavigation.Supplier1 == filtering).ToList();
            }

            if (sorting == "по возрастанию") // Сортировка
            {
                items = items.OrderBy(obj => obj.AmountInStorage).ToList();
            }
            else // Сортировка
            {
                items = items.OrderByDescending(obj => obj.AmountInStorage).ToList();
            }
            if (items.Count == 0)
            {
                NotFoundTB.Visibility = Visibility.Visible;
                CatalogBoxLV.Visibility = Visibility.Collapsed;
            }
            else
            {
                NotFoundTB.Visibility = Visibility.Collapsed; 
                CatalogBoxLV.Visibility = Visibility.Visible;
                CatalogBoxLV.ItemsSource = items;
            }
        }

        private void ExitAccountBTN_Click(object sender, RoutedEventArgs e)
        {
            ExitAccountBtn();
        }

        private void ExitAccountBtn()
        {
            if (MessageBox.Show("Вы точно хотите выйти?", "Вопрос", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;
            new AuthorizationWindow().Show();
            this.Close();
        }

        private void searchShoesTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sortingList == null)
                return;
            TextBox textBox = sender as TextBox;
            SetDataList(textBox!.Text, sortingShoesCB.Text, filteringShoesCB.Text);
        }

        private void sortingShoesCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sortingList == null)
                return;
            ComboBox comboBox = sender as ComboBox;
            SetDataList(searchShoesTB.Text, sortingList[comboBox.SelectedIndex], filteringShoesCB.Text);
        }

        private void filteringShoesCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sortingList == null)
                return;
            ComboBox comboBox = sender as ComboBox;
            SetDataList(searchShoesTB.Text, sortingShoesCB.Text, listSupplier[comboBox.SelectedIndex]);
        }

        private void addShoesBTN_Click(object sender, RoutedEventArgs e)
        {
            addItem();
        }

        private void addItem()
        {
            if (user == null || user.Role != "Администратор")
                return;

            if (new AddChangeWindow().ShowDialog() == true)
            {
                MessageBox.Show("Товар успешно добавлен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                SetDataList(searchShoesTB.Text, sortingShoesCB.Text, filteringShoesCB.Text);
            }
            else
            {
                MessageBox.Show("Товар не был добавлен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CatalogBoxLV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            changeItem(sender);
        }

        private void changeItem(object sender)
        {
            if (user == null || user.Role != "Администратор")
                return;

            ListView listView = sender as ListView;
            Item item = (Item)listView.SelectedItem;
            AddChangeWindow result = new AddChangeWindow(item);
            if (result.ShowDialog() == true)
            {
                string message = ShoesContext.Context.Items.FirstOrDefault(obj => obj.Articul == item.Articul) == null ? "Товар успешно удалён!" : "Товар успешно изменён!";
                MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                SetDataList(searchShoesTB.Text, sortingShoesCB.Text, filteringShoesCB.Text);
            }
            else
            {
                MessageBox.Show("Товар не был изменён!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OrdersBTN_Click(object sender, RoutedEventArgs e)
        {
            new OrderWindow(user).Show();
            this.Close();
        }
    }
}
