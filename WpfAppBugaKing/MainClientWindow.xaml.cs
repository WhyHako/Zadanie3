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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainClientWindow : Window
    {
        private static List<Menu> cartItems = new List<Menu>();
        public MainClientWindow()
        {
            InitializeComponent();
        }
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) //Внесение данных в DataGrid
        {
            if (Visibility == Visibility.Visible)
            {
                BugaKingEntities.getContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                Menu_Grider.ItemsSource = BugaKingEntities.getContext().Menu.ToList();
            }
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {
            CardWindow cardWindow = new CardWindow(cartItems);
            cardWindow.Show();
            this.Close();
        }
        private void AddCard_Click(object sender, RoutedEventArgs e)
        {
            Menu selectedDish = (Menu)Menu_Grider.SelectedItem;
            if (selectedDish != null)
            {
                cartItems.Add(selectedDish);
                MessageBox.Show("Блюдо добавлено в корзину.");
            }
        }

        private void MyOrders_Click(object sender, RoutedEventArgs e)
        {
            MyOrdersWindow MyOrdersWindow = new MyOrdersWindow();
            MyOrdersWindow.Show();
            this.Close();
        }
    }
}
