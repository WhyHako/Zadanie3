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

namespace WpfAppBugaKing
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow LoginWindow = new LoginWindow();
            LoginWindow.Show();
            this.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e) //добавление записи в таблицу бд
        {
            if (LNTextBox.Text != null && FNTextBox.Text != null && MNTextBox.Text != null &&
            AddressTextBox.Text != null && usernameTextBox.Text != null && EmailTextBox.Text != null && passwordBox.Password != null)
            {
                try
                {
                    User User = new User()
                    {
                        FirstName = FNTextBox.Text,
                        MiddleName = MNTextBox.Text,
                        LastName = LNTextBox.Text,
                        Address = AddressTextBox.Text,
                        Phone = usernameTextBox.Text,
                        Email = EmailTextBox.Text,
                        Password = passwordBox.Password,
                        Role = "User"
                    };

                    BugaKingEntities.getContext().User.Add(User);
                }
                catch (Exception ex) { }
                finally
                {
                    BugaKingEntities.getContext().SaveChanges();

                    LoginWindow lw = new LoginWindow();
                    lw.Show();
                    this.Close();
                }
            }
            else
                MessageBox.Show("Ошибка, неправильно введены данные!");
        }
    }
}
