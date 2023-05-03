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
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        public Window4()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var inquiry = @"select деталипродажа.иддеталипродажа, товары.название 'Товар', категориитоваров.название 'Категория', деталипродажа.количество, деталипродажа.цена, деталипродажа.количество*деталипродажа.цена 'Сумма' from продажа, деталипродажа, товары, категориитоваров
where продажа.идпродажи = деталипродажа.идпродажи and деталипродажа.идтовара = товары.идтовара and товары.идкатегории = категориитоваров.идкатегории
and продажа.идпродажи = 1
";

            SQL.SQLConnect();
            var dt = SQL.Inquiry(inquiry);
            SQL.Close();
            main_data_grid.ItemsSource = dt.AsDataView();
            main_data_grid.Columns[0].Visibility = Visibility.Collapsed; // Скрываем первый столбец с ID
        }
    }
}
