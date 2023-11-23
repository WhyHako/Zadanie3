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
using System.Collections.Generic;

namespace WpfAppBugaKing
{
    /// <summary>
    /// Логика взаимодействия для CardWindow.xaml
    /// </summary>
    public partial class CardWindow : Window
    {
        private List<Menu> cartItems;

        public CardWindow(List<Menu> cartItems)
        {
            InitializeComponent();
            this.cartItems = cartItems;
            Card_Grider.ItemsSource = cartItems;
        }
        private void Delete_Click(object sender, RoutedEventArgs e) // Кнопка удаления из корзины
        {
            Menu selectedDish = (Menu)Card_Grider.SelectedItem;
            if (selectedDish != null)
            {
                cartItems.Remove(selectedDish);
                Card_Grider.ItemsSource = null;
                Card_Grider.ItemsSource = cartItems;
                MessageBox.Show("Блюдо удалено из корзины.");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) //Кнопка возврата на страницу пользователя
        {
            MainClientWindow MainClientWindow = new MainClientWindow();
            MainClientWindow.Show();
            this.Close();
        }

        private void Done_Click(object sender, RoutedEventArgs e) 
        {
            User currentUser = CurrentUserManager.CurrentUser; //Сохранение

            if (currentUser != null)
            {
                if (cartItems.Count != 0)
                { 
                int? totalAmount = cartItems.Sum(dish => dish.Price);
                Orders order = new Orders
                {
                    Status = "Зарегистрирован",
                    OrderDateTime = DateTime.Now,
                    UserID = currentUser.UserID,
                    RestaurantID = 1,
                    TotalAmount = totalAmount
                };

                foreach (Menu dish in cartItems)
                {
                    OrderDetails orderDetail = new OrderDetails
                    {
                        Quantity = 1,
                        DishID = dish.DishID,
                    };

                    order.OrderDetails.Add(orderDetail);
                }

                using (var context = new BugaKingEntities())
                {
                    context.Orders.Add(order);
                    context.SaveChanges();
                }

                    MessageBox.Show("Заказ успешно оформлен.");

                    // Очистка корзины после оформления заказа
                    cartItems.Clear();
                    Card_Grider.ItemsSource = null;
                    Card_Grider.ItemsSource = cartItems;
                    MainClientWindow MainClientWindow = new MainClientWindow();
                    MainClientWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Корзина пуста. Добавьте блюда перед оформлением заказа.");
                    MainClientWindow MainClientWindow = new MainClientWindow();
                    MainClientWindow.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Ошибка получения текущего пользователя.");
            }
        }
    }
}
