using CustomerManagementSystem.Core;
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
    public partial class PageImplementedServices : Page
    {
        DataTable dataTableMain;
        DataTable newDataTable;
        public PageImplementedServices()
        {
            InitializeComponent();
            
            UpdateTable();
        }


        async void UpdateTable()
        {
            dataGridMain.Visibility = Visibility.Hidden;
            var inquiry = @"select окусл.идуслуги, concat('Оказание услуги #', окусл.идуслуги) 'Номер накладной', FORMAT( окусл.датаоказания, 'yyyy.MM.dd', 'zh-cn' )  'Дата оказания услуги', клиенты.наименование 'Организация', сумма 'Оплата' from оказаниеуслуг окусл right join услуги усл on окусл.идуслуги = усл.идуслуги join клиенты on клиенты.идклиента = окусл.идклиента ORDER BY 'Дата оказания услуги' DESC";

            SQL.SQLConnect();
            SearchTextBox.Text = "";
            dataTableMain = SQL.Inquiry(inquiry);
            newDataTable = dataTableMain.Copy();
            SQL.Close();
            dataGridMain.ItemsSource = dataTableMain.AsDataView();
            await Task.Delay(100);
            dataGridMain.Columns[0].Visibility = Visibility.Collapsed; // Скрываем первый столбец с ID

            dataGridMain.Visibility = Visibility.Visible;
            dataGridMain.Columns[2].SortDirection = System.ComponentModel.ListSortDirection.Descending;
        }

        private void Search_Changed(object sender, TextChangedEventArgs e)
        {
            string textToFind1 = SearchTextBox.Text;

            newDataTable = SearcherDataTable.WordSearch(textToFind1, dataTableMain);

            dataGridMain.ItemsSource = newDataTable.AsDataView();
            dataGridMain.Columns[0].Visibility = Visibility.Collapsed; // Скрываем первый столбец с ID
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Controller.PagesController.NewPage(new AddEditSale());
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


            var resultWin = MessageBox.Show("Вы уверены что хотите удалить выбранную продажу!", "Удаление", MessageBoxButton.YesNo);

            if (resultWin == MessageBoxResult.Yes)
            {
                var inquiry = $"DELETE FROM оказаниеуслуг where идоказаниеуслуг = {index}";
                SQL.SQLConnect();
                SQL.Execute(inquiry);
                SQL.Close();
                UpdateTable();
            }
        }

        private void dataGridMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            if (dataGridMain.SelectedIndex == -1)
                return;
            DataRow dataRow = newDataTable.Rows[dataGridMain.SelectedIndex];

            DataRowView selectedRow = (DataRowView)dataGridMain.SelectedItem;

            if (selectedRow != null)
            {
                string value = selectedRow[0].ToString();
                Controller.PagesController.NewPage(new AddEditSale(selectedRow));
            }

        }

        private void ThisPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                UpdateTable();
            }
        }
    }
}
