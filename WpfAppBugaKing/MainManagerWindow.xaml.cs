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
    /// Логика взаимодействия для MainManagerWindow.xaml
    /// </summary>
    public partial class MainManagerWindow : Window
    {
        public MainManagerWindow()
        {
            InitializeComponent();
            BugaKingEntities.getContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            Manager_Grider.ItemsSource = BugaKingEntities.getContext().Orders.ToList();
            Manager_Grider.AutoGeneratingColumn += Manager_Grider_AutoGeneratingColumn;
        }

        private void Manager_Grider_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "OrderDetails" || e.PropertyName == "Restaurants" || //скрыть
                e.PropertyName == "User" || e.PropertyName == "UserID" ||
                e.PropertyName == "RestaurantID")
            {
                e.Cancel = true;
            }
            else if (e.PropertyName == "OrderID" || e.PropertyName == "TotalAmount" || e.PropertyName == "OrderDateTime") //запрет на изменение
            {
                e.Column.IsReadOnly = true;
            }
            else if (e.PropertyName == "Status") //комбобокс статуса и выбора
            {
                var comboBoxColumn = new DataGridComboBoxColumn
                {
                    Header = "Status",
                    SelectedValueBinding = new Binding("Status"),
                    ItemsSource = new List<string> { "Зарегистрирован", "Готовим", "Собираем", "Готов к получению", "Заказ у покупателя" }
                };

                e.Column = comboBoxColumn;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e) //кнопка сохранения
        {
            Orders tmp = Manager_Grider.SelectedItem as Orders;
            if (tmp != null)
            {
                if (tmp.OrderID == 0)
                BugaKingEntities.getContext().Orders.Add(tmp);
                BugaKingEntities.getContext().SaveChanges();
            }
        }
    }
}
