using Shoes_Demo_Ekz.Models;
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
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void AuthBTN_Click(object sender, RoutedEventArgs e)
        {
            Authorization();
        }

        private void GuestEnterBTN_Click(object sender, RoutedEventArgs e)
        {
            GuestEnter();
        }

        private void GuestEnter()
        {
            new CatalogWindow(null).Show();
            this.Close();
        }

        private void Authorization()
        {
            string mail = MailTB.Text;
            string password = PasswordTB.Text;
            if (mail.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Не все поля введены!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Regex.IsMatch(mail, "^[a-z0-9]{4,15}[@][a-z]{4,10}[.]com$", RegexOptions.IgnoreCase))
            {
                MessageBox.Show("Некорректная почта!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Regex.IsMatch(password, "^[a-z0-9]{5,30}$", RegexOptions.IgnoreCase))
            {
                MessageBox.Show("Некорректный пароль!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            User? user = ShoesContext.Context.Users.FirstOrDefault(obj => obj.Mail == mail && obj.Password == password);
            if (user == null)
            {
                MessageBox.Show("Неверная почта или пароль, либо такого пользователя не существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Успешной вход!", "Поздравляем!", MessageBoxButton.OK, MessageBoxImage.Information);
            new CatalogWindow(user).Show();
            this.Close();
        }
    }
}
