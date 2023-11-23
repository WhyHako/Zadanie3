using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
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
    enum TableEnum
    {
        User,
        Restaurants,
        Menu,
        OrderDeatails,
        Orders,
    }
    /// <summary>
    /// Логика взаимодействия для MainAdminWindow.xaml
    /// </summary>
    public partial class MainAdminWindow : Window
    {
        private bool isUser;
        private bool isRestaurants;
        private bool isMenu;
        private bool isOrderDeatails;
        private bool isOrders;
        public MainAdminWindow()
        {
            InitializeComponent();
            Admin_Grider.AutoGeneratingColumn += Admin_Grider_AutoGeneratingColumn;
            Admin_Grider.PreparingCellForEdit += Admin_Grider_PreparingCellForEdit;
            IdentificationTable(TableEnum.User);
            Admin_Grider.ItemsSource = BugaKingEntities.getContext().User.ToList();
        }
        private void Admin_Grider_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) // Убираем возможность редактировать некоторые колонки в DataGrid
        {
            if (e.PropertyName == "Orders" || e.PropertyName == "OrderDetails" || e.PropertyName == "Password")
            {
                e.Cancel = true;
            }
            else if (e.PropertyName == "UserID")
            {
                e.Column.IsReadOnly = true;
            }
            else if (e.PropertyName == "Role")
            {
                var comboBoxColumn = new DataGridComboBoxColumn
                {
                    Header = "Role",
                    SelectedValueBinding = new Binding("Role"),
                    ItemsSource = new List<string> { "Manager", "User" }
                };

                e.Column = comboBoxColumn;
            }
        }
        private void Admin_Grider_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e) // Запрет на редактирование роли администратор
        {
            if (e.Column.Header.ToString() == "Role")
            {
                var user = e.Row.Item as User;

                if (user != null && user.Role == "Admin")
                {
                    e.EditingElement.IsEnabled = false;
                }
            }
        }
        private void Manager_Click(object sender, RoutedEventArgs e) //Переход к таблице пользователей
        {
            IdentificationTable(TableEnum.User);
            Admin_Grider.ItemsSource = BugaKingEntities.getContext().User.ToList();
        }

        private void Menu_Click(object sender, RoutedEventArgs e) //Переход к таблице меню
        {
            IdentificationTable(TableEnum.Menu);
            Admin_Grider.ItemsSource = BugaKingEntities.getContext().Menu.ToList();
        }
        private void IdentificationTable(TableEnum table)
        {
            if (table == TableEnum.User)
            {
                isUser = true;
                isRestaurants = false;
                isMenu = false;
                isOrderDeatails = false;
                isOrders = false;
            }
            if (table == TableEnum.Menu)
            {
                isUser = false;
                isRestaurants = false;
                isMenu = true;
                isOrderDeatails = false;
                isOrders = false;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить эту строку?", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (isMenu)
                {
                    Menu tmp = Admin_Grider.SelectedItem as Menu;
                    TryDeleteEntity(tmp, TableEnum.Menu);
                }

                if (isUser)
                {
                    User tmp = Admin_Grider.SelectedItem as User;
                    TryDeleteEntity(tmp, TableEnum.User);
                }
            }
        }
        private bool HasRelatedOrders(object entity, TableEnum table)
        {
            if (table == TableEnum.User)
            {
                User user = entity as User;
                if (user != null && user.Orders != null && user.Orders.Any())
                {
                    return true;
                }
            }
            else if (table == TableEnum.Menu)
            {
                Menu menu = entity as Menu;
                if (menu != null && menu.OrderDetails != null && menu.OrderDetails.Any())
                {
                    return true;
                }
            }
            return false;
        }
        private void TryDeleteEntity(object entity, TableEnum table) //Удаление и проверки на роль админа
        {
            if (entity != null)
            {
                // Проверка наличия связанных заказов
                if (!HasRelatedOrders(entity, table))
                {
                    // Проверка роли "admin"
                    if (!IsAdminRole(entity, table))
                    {
                        BugaKingEntities context = BugaKingEntities.getContext();
                        context.Entry(entity).State = EntityState.Deleted;
                        context.SaveChanges();
                        RefreshGrid(table);
                    }
                    else
                    {
                        MessageBox.Show("Невозможно удалить роль 'admin'.");
                    }
                }
                else
                {
                    MessageBox.Show("Невозможно удалить, так как есть связанные заказы.");
                }
            }
        }

        private bool IsAdminRole(object entity, TableEnum table)
        {
            if (table == TableEnum.User)
            {
                User user = entity as User;
                return user != null && user.Role == "Admin";
            }
            return false;
        }

        private void RefreshGrid(TableEnum table) 
        {
            switch (table)
            {
                case TableEnum.User:
                    Admin_Grider.ItemsSource = BugaKingEntities.getContext().User.ToList();
                    break;
                case TableEnum.Menu:
                    Admin_Grider.ItemsSource = BugaKingEntities.getContext().Menu.ToList();
                    break;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e) //Сохранение изменений к разным таблицам
        {
            if (isUser)
            {
                User tmp = Admin_Grider.SelectedItem as User;
                if (tmp != null)
                {
                    if (tmp.UserID == 0)
                    BugaKingEntities.getContext().User.Add(tmp);
                    BugaKingEntities.getContext().SaveChanges();
                }
            }
            if (isMenu)
            {
                Menu tmp = Admin_Grider.SelectedItem as Menu;
                if (tmp != null)
                {
                    if (tmp.DishID == 0)
                    BugaKingEntities.getContext().Menu.Add(tmp);
                    BugaKingEntities.getContext().SaveChanges();
                }
            }
        }  
    }
}