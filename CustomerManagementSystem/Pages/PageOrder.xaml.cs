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
    /// Логика взаимодействия для PageOrder.xaml
    /// </summary>
    public partial class PageOrder : Page
    {
        public PageOrder()
        {
            InitializeComponent();
            UpdateTable();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        void UpdateTable()
        {
            var inquiry = @"select продажа.идпродажи, concat('Заказ #', продажа.идпродажи) 'Номер накладной', FORMAT( продажа.датапродажи, 'dd.MM.yyyy', 'zh-cn' )  'Дата заказа', FORMAT(sum(цена*количество), 'N', 'en-us') 'Сумма' from деталипродажа JOIN продажа ON продажа.идпродажи = деталипродажа.идпродажи group by продажа.идпродажи, датапродажи";

            SQL.SQLConnect();
            SearchTextBox.Text = "";
            DataTable dataTableMain = SQL.Inquiry(inquiry);
            SQL.Close();
            dataGridMain.ItemsSource = dataTableMain.AsDataView();

        }
    }
}
