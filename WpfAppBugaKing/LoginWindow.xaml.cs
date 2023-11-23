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

namespace WpfAppBugaKing
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public User GetCurrentUser()
        {
            BugaKingEntities DbBd = new BugaKingEntities(); //Сам логин
            return (from ord in DbBd.User
                    where ord.Phone == usernameTextBox.Text && ord.Password == passwordBox.Password
                    select ord).FirstOrDefault();
        }

        private void Login_Click(object sender, RoutedEventArgs e) //Переход на окно в зависимости от роли
        {
            User currentUser = GetCurrentUser();

            if (currentUser != null)
            {
                CurrentUserManager.CurrentUser = currentUser;

                if (currentUser.Role == "User")
                {
                    MainClientWindow MainWindow = new MainClientWindow();
                    MainWindow.Show();
                    this.Close();
                }
                else if (currentUser.Role == "Manager")
                {
                    MainManagerWindow MainWindow = new MainManagerWindow();
                    MainWindow.Show();
                    this.Close();
                }
                else if (currentUser.Role == "Admin")
                {
                    MainAdminWindow MainWindow = new MainAdminWindow();
                    MainWindow.Show();
                    this.Close();
                }
                else
                {
                    LogError.Visibility = Visibility.Visible;
                    LogError.Text = "У вас нет доступа.";
                }
            }
            else
            {
                LogError.Visibility = Visibility.Visible;
                LogError.Text = "Введены неверные данные";
            }
        }

    private void Register_Click(object sender, RoutedEventArgs e) //переход на окно регистрации
        {
            RegisterWindow RegisterWindow = new RegisterWindow();
            RegisterWindow.Show();
            this.Close();
        }
    }
}
