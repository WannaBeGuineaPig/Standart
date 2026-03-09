using Microsoft.EntityFrameworkCore;
using Shoes_Demo_Ekz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private User user;
        public OrderWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            SetData();
        }

        private void UpdateDataListView()
        {
            List<Order> orders = ShoesContext.Context.Orders.Include(obj => obj.IdPickupPointNavigation).OrderByDescending(obj => obj.Id).ToList();
            OrderBoxLV.ItemsSource = orders;
        }

        private void SetData() // Установка данных в момент открытия окна
        {
            RoleUserTBL.Text = user.Role;
            FIOUserTBL.Text = $"{user.LastName} {user.FirstName} {user.MidleName}";

            if (user.Role == "Администратор")
                addOrderBTN.Visibility = Visibility.Visible;

            UpdateDataListView();
        }

        private void ReturnBTN_Click(object sender, RoutedEventArgs e)
        {
            new CatalogWindow(user).Show();
            this.Close();
        }

        private void addOrderBTN_Click(object sender, RoutedEventArgs e)
        {
            if (user.Role == "Менеджер")
                return;

            if (new AddChangeDeleteOrderWindow(null).ShowDialog() == true)
                UpdateDataListView();
        }

        private void OrderBoxLV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (user.Role == "Менеджер")
                return;

            ListView listView = sender as ListView;
            Order order = (Order)listView.SelectedItem;
            if (order.Status == "Завершен")
            {
                return;
            }
            if (new AddChangeDeleteOrderWindow(order).ShowDialog() == true)
                UpdateDataListView();
        }
    }
}
