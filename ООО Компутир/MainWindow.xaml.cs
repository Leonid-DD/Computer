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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ООО_Компутир.Classes;

namespace ООО_Компутир
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Helper.DB = new Model.DBComputer();
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bGuest_Click(object sender, RoutedEventArgs e)
        {
            View.PCList PCList = new View.PCList();
            this.Hide();
            PCList.ShowDialog();
            this.Show();
        }

        private void bLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text;
            string password;
            if ((bool)cbSeePassword.IsChecked) password = tbPassword.Text;
            else password = pbPassword.Password;

            StringBuilder sb = new StringBuilder();

            //Обработка пустоты
            if (login == "")
            {
                sb.Append("Логин не введен.\n");
            }
            if (password == "")
            {
                sb.Append("Пароль не введен.\n");
            }
            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Поиск логина и пароля в БД
            List<Model.User> users = Helper.DB.User.ToList();

            //Авторизация через where
            Model.User user = users.Where(u => u.UserLogin.Equals(login)).FirstOrDefault();
            if (user != null)
            {
                //Проверка ввода пароля
                if (user.UserPassword.Equals(password))
                {
                    Helper.User = user;

                    //Переход на следующее окно
                    View.PCList PCList = new View.PCList();
                    this.Hide();
                    PCList.ShowDialog();
                    this.Show();

                    return;
                }
                else
                {
                    sb.Append("Введен неверный пароль.");
                }
            }
            else
            {
                sb.Append("Пользователь не найден.");
            }
            MessageBox.Show(sb.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void cbSeePassword_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)cbSeePassword.IsChecked)
            {
                pbPassword.Visibility = Visibility.Hidden;
                tbPassword.Visibility = Visibility.Visible;
                tbPassword.Text = pbPassword.Password;
            }
            else
            {
                pbPassword.Visibility = Visibility.Visible;
                tbPassword.Visibility = Visibility.Hidden;
                pbPassword.Password = tbPassword.Text;
            }
        }
    }
}
