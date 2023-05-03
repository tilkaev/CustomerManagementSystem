using System;
using System.Collections.Generic;
using System.Data;
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

namespace CustomerManagementSystem
{
    /// <summary>
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var inquiry = @"select продажа.идпродажи, concat('Продажа #', продажа.идпродажи) 'Номер накладной', продажа.датапродажи 'Дата продажи', sum(цена) 'Сумма' from деталипродажа
JOIN продажа ON продажа.идпродажи = деталипродажа.идпродажи
group by продажа.идпродажи, датапродажи
";

            SQL.SQLConnect();
            var dt = SQL.Inquiry(inquiry);
            SQL.Close();
            main_data_grid.ItemsSource = dt.AsDataView();
            main_data_grid.Columns[0].Visibility = Visibility.Collapsed; // Скрываем первый столбец с ID
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var win = new Window4();
            win.Show();
        }
    }
}
