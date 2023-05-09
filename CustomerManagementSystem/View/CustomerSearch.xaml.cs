using CustomerManagementSystem.Core;
using CustomerManagementSystem.Pages;
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

namespace CustomerManagementSystem.View
{
    /// <summary>
    /// Логика взаимодействия для ProductSearch.xaml
    /// </summary>
    public partial class CustomerSearch : Window
    {


        DataTable dataTableMain;
        DataTable dataTableCategory;
        DataTable newDataTable;



        public CustomerSearch()
        {
            InitializeComponent();
            var inquiry = @"select товары.идтовара, товары.название Название, категориитоваров.название Категория, товары.цена Цена, товары.колвонаскладе 'Кол-во на складе'  from товары, категориитоваров
where товары.идкатегории = категориитоваров.идкатегории and товары.колвонаскладе > 0";

            SQL.SQLConnect();
            SearchTextBox.Text = "";
            dataTableMain = SQL.Inquiry(inquiry);
            newDataTable = dataTableMain.Copy();
            SQL.Close();
            dataGridMain.ItemsSource = dataTableMain.AsDataView();
            //main_data_grid.Columns[0].Visibility = Visibility.Collapsed; // Скрываем первый столбец с ID
        }

        private void Search_Changed(object sender, TextChangedEventArgs e)
        {

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void dataGridMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var index = dataGridMain.SelectedIndex;

            AddEditSale.AddTovar(dataTableMain.Rows[index]);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridMain.Columns[0].Width = 100;
            dataGridMain.Columns[1].Width = 250;
        }
    }
}
