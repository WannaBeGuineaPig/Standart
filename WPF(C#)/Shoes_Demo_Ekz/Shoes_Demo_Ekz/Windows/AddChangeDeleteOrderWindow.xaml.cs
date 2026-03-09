using Microsoft.EntityFrameworkCore;
using Shoes_Demo_Ekz.Models;
using Shoes_Demo_Ekz.SupportClass;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AddChangeDeleteOrderWindow.xaml
    /// </summary>
    public partial class AddChangeDeleteOrderWindow : Window
    {
        private Order? order;
        private List<string> listStatusOrder = new List<string>() 
        {
            "Новый",
            "Завершен"
        };
        private List<string> listAddressOrder;
        public AddChangeDeleteOrderWindow(Order order)
        {
            InitializeComponent();
            this.order = order;
            SetData();
        }

        private void SetData()
        {
            this.Title += order == null ? "добавление заказа" : "редактирование заказа";
            addChangeBTN.Content = order == null ? "Добавить заказ" : "Изменить заказ";

            listAddressOrder = ShoesContext.Context.PickupPoints.Select(obj => obj.Address).ToList();

            StatusOrderCB.ItemsSource = listStatusOrder;
            AddressOrderCB.ItemsSource = listAddressOrder;

            List<string> lstUsers = ShoesContext.Context.Users.Select(obj => $"{obj.LastName} {obj.FirstName} {obj.MidleName}").ToList();

            UserOrderCB.ItemsSource = lstUsers;

            if (order == null)
            {
                DateOrderingTBL.Text += DateOnly.FromDateTime(DateTime.Now).ToString();
                DateDeliveryDP.SelectedDate = DateTime.Now;
                OrderItemsLV.ItemsSource = ShoesContext.Context.Items.Select(obj => new AddOrderClass()
                {
                    ArticulItem = obj.Articul,
                }).ToList();
                StatusOrderCB.IsEnabled = false;
                DateDeliveryDP.DisplayDateStart = DateTime.Now;
                DateDeliveryDP.DisplayDateEnd = DateTime.Now.AddYears(1);
                OrderItemsLV.Visibility = Visibility.Visible;
                return;
            }
            UserOrderCB.IsEnabled = false;
            User user = ShoesContext.Context.Users.First(obj => obj.Id == order.IdUser);
            UserOrderCB.SelectedIndex = lstUsers.IndexOf($"{user.LastName} {user.FirstName} {user.MidleName}");
            IdOrderTBL.Text = $"Номер заказа: {order.Id}";
            IdOrderTBL.Visibility = Visibility.Visible;
            IdCodePickUpTBL.Text = $"Код получения заказа: {order.CodePickup}";
            IdCodePickUpTBL.Visibility = Visibility.Visible;
            deleteBTN.Visibility = Visibility.Visible;
            StatusOrderCB.SelectedIndex = listStatusOrder.IndexOf(order.Status);
            AddressOrderCB.SelectedIndex = listAddressOrder.IndexOf(order.IdPickupPointNavigation.Address);
            DateOrderingTBL.Text += order.DateOrdering.ToString();
            DateDeliveryDP.SelectedDate = order.DateDelivery.ToDateTime(TimeOnly.MinValue);
            DateDeliveryDP.DisplayDateStart = order.DateDelivery.ToDateTime(TimeOnly.MinValue);
            DateDeliveryDP.DisplayDateEnd = order.DateDelivery.ToDateTime(TimeOnly.MinValue).AddYears(1);
            OrderItemsTBL.Visibility = Visibility.Visible;
            OrderItemsTBL.Text = "Состав заказа:";

            var data = ShoesContext.Context.OrderItems
                .Where(obj => obj.IdOrder == order.Id)
                .Select(obj => new { obj.ArticulItem, obj.AmountItem });

            foreach (var item in data)
            {
                OrderItemsTBL.Text += $"\nАртикул обуви: {item.ArticulItem}; Количество в заказе: {item.AmountItem};";
            }
        }
        private void exitActionItem(System.ComponentModel.CancelEventArgs e)
        {
            if (this.DialogResult == true)
            {
                string resultMessage = order == null ? "добавлен!" : order.Id == -1 ? "удалён!" : "изменён!";
                MessageBox.Show("Заказ успешно " + resultMessage, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string mess = $"Вы точно хотите выйти из окна " + (order == null ? "добавления заказа?" : "редактирования заказы?") + "\nПри выходе данные не будут сохранены.";
            MessageBoxResult boxResult = MessageBox.Show(mess, "Вопрос?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (boxResult == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            exitActionItem(e);
        }

        private void ExitAccountBTN_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void addChangeBTN_Click(object sender, RoutedEventArgs e)
        {
            AddChangeOrder();

        }
        private void AddChangeOrder()
        {
            if (order == null)
            {
                AddNewOrder();
            }

            else
                ChangeOrder();
        }

        private void AddNewOrder()
        {
            List<(string, int)> listItemsOrder = new List<(string, int)>();
            foreach (var item in OrderItemsLV.Items)
            {
                AddOrderClass itemClass = (AddOrderClass)item;
                if (itemClass.AmountItem.ToString().Length == 0 || itemClass.AmountItem == 0) continue;
                else if (Convert.ToInt32(itemClass.AmountItem) > ShoesContext.Context.Items.First(obj => obj.Articul == itemClass.ArticulItem).AmountInStorage)
                {
                    MessageBox.Show("Количество товаров в заказе не может превышать количество товаров на складе!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                ShoesContext.Context.Items.First(obj => obj.Articul == itemClass.ArticulItem).AmountInStorage -= Convert.ToInt32(itemClass.AmountItem);
                listItemsOrder.Add((itemClass.ArticulItem, itemClass.AmountItem));
            }
            if (listItemsOrder.Count == 0)
            {
                MessageBox.Show("Не выбранны товары для заказа!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Order order = new Order()
            {
                DateOrdering = DateOnly.FromDateTime(Convert.ToDateTime(DateOrderingTBL.Text.Split()[2])),
                DateDelivery = DateOnly.FromDateTime((DateTime)DateDeliveryDP.SelectedDate),
                IdPickupPoint = ShoesContext.Context.PickupPoints.First(obj => obj.Address == AddressOrderCB.Text).Id,
                IdUser = ShoesContext.Context.Users
                .First(obj => obj.LastName == UserOrderCB.Text.Split()[0] && obj.FirstName == UserOrderCB.Text.Split()[1] && obj.MidleName == UserOrderCB.Text.Split()[2]).Id,
                CodePickup = ShoesContext.Context.Orders.Max(x => x.CodePickup) + 1,
                Status = StatusOrderCB.Text,
            };
            ShoesContext.Context.Orders.Add(order);
            ShoesContext.Context.SaveChanges();
            ShoesContext.Context.OrderItems.AddRange(listItemsOrder.Select(obj => new OrderItem() { IdOrder = order.Id, ArticulItem = obj.Item1, AmountItem = obj.Item2 }));
            ShoesContext.Context.SaveChanges();
            this.DialogResult = true;
        }

        private void ChangeOrder()
        {
            order.Status = StatusOrderCB.Text;
            order.IdPickupPoint = ShoesContext.Context.PickupPoints.First(obj => obj.Address == AddressOrderCB.Text).Id;
            order.DateDelivery = DateOnly.FromDateTime((DateTime)DateDeliveryDP.SelectedDate);
            ShoesContext.Context.SaveChanges();
            this.DialogResult = true;
        }

        private void deleteBTN_Click(object sender, RoutedEventArgs e)
        {
            DeleteOrder();
        }

        private void DeleteOrder()
        {
            if (MessageBox.Show("Вы точно хотите удалить данный заказ?", "Вопрос?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            List<OrderItem> items = ShoesContext.Context.OrderItems.Where(obj => obj.IdOrder == order.Id).Include(obj => obj.ArticulItemNavigation).ToList();
            foreach (OrderItem itemClass in items)
            {
                itemClass.ArticulItemNavigation.AmountInStorage += itemClass.AmountItem;
            }

            ShoesContext.Context.OrderItems.RemoveRange(items);
            ShoesContext.Context.Orders.Remove(order);
            ShoesContext.Context.SaveChanges();
            order.Id = -1;
            this.DialogResult = true;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !new Regex(@"\d").IsMatch(e.Text);
        }

    }
}
