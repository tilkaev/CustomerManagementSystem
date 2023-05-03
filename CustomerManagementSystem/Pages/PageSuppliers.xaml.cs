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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomerManagementSystem.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageSuppliers.xaml
    /// </summary>
    public partial class PageSuppliers : Page
    {
        public PageSuppliers()
        {
            InitializeComponent();
            UpdateTable();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        void UpdateTable()
        {
            var inquiry = @"select * from поставщики";

            SQL.SQLConnect();
            SearchTextBox.Text = "";
            DataTable dataTableMain = SQL.Inquiry(inquiry);
            SQL.Close();
            dataGridMain.ItemsSource = dataTableMain.AsDataView();

        }
    }
}
