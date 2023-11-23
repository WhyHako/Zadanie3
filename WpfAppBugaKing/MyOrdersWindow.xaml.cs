using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppBugaKing
{
    public partial class MyOrdersWindow : Window
    {
        private User currentUser;

        public MyOrdersWindow()
        {
            InitializeComponent();
            currentUser = CurrentUserManager.CurrentUser;
            LoadOrders();
        }

        private void LoadOrders() //загрузка заказов к определенному пользователю который вошел в система
        {
            if (currentUser != null)
            {
                using (var context = new BugaKingEntities())
                {
                    var userOrders = context.Orders
                        .Where(order => order.UserID == currentUser.UserID)
                        .ToList();

                    MyOrders_Grider.ItemsSource = userOrders;
                }
            }
            else
            {
                MessageBox.Show("Ошибка получения текущего пользователя.");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainClientWindow mainClientWindow = new MainClientWindow();
            mainClientWindow.Show();
            this.Close();
        }
    }
}