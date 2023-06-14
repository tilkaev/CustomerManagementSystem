using CustomerManagementSystem.Core;
using CustomerManagementSystem.View;
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
    /// Логика взаимодействия для PageSale.xaml
    /// </summary>
    public partial class PageServices : Page
    {
        DataTable dataTableMain;
        DataTable dataTableCategory;
        DataTable newDataTable;
        public PageServices()
        {
            InitializeComponent();

            UpdateTable();
        }


        private void Search_Changed(object sender, object e)
        {
            string textToFind1 = SearchTextBox.Text;
            

            var searcherData = new SearcherDataTable()
            {
                search_words = new string[] { textToFind1 },
                search_columns = new int[] { 1 },
                dataTable = dataTableMain
            };
            newDataTable = searcherData.MultiSearch();
            dataGridMain.ItemsSource = newDataTable.AsDataView();



        }

        private void main_data_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGridMain.SelectedIndex == -1)
                return;
            var index = (int)newDataTable.Rows[dataGridMain.SelectedIndex][0];

            var win = new AddEditService(index);
            win.ShowDialog();

            if (win.result)
            {
                UpdateTable();
            }


        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddEditService();
            win.ShowDialog();

            if (win.result)
            {
                UpdateTable();
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            var index = dataGridMain.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("Выберите запись для удаления!");
                return;
            }
            index = (int)newDataTable.Rows[index][0];


            var resultWin = MessageBox.Show("Вы уверены что хотите удалить выбранную запись!", "Удаление", MessageBoxButton.YesNo);

            if (resultWin == MessageBoxResult.Yes)
            {
                var inquiry = $"DELETE FROM услуги where услуги.идуслуги = {index}";
                SQL.SQLConnect();
                SQL.Execute(inquiry);
                SQL.Close();
                UpdateTable();
            }

        }


        void UpdateTable()
        {
            var inquiry = @"select идуслуги, наименование Наименование, стоимость Стоимость from услуги";
            string inquiry2 = String.Format("select * from категориитоваров");

            SQL.SQLConnect();
            SearchTextBox.Text = "";
            dataTableMain = SQL.Inquiry(inquiry);
            dataTableCategory = SQL.Inquiry(inquiry2);
            newDataTable = dataTableMain.Copy();
            SQL.Close();
            dataGridMain.ItemsSource = dataTableMain.AsDataView();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
