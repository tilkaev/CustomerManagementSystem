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

namespace CustomerManagementSystem
{
    /// <summary>
    /// Логика взаимодействия для PageInventory.xaml
    /// </summary>
    public partial class PageInventory : Page
    {
        DataTable dataTableMain;
        DataTable dataTableCategory;
        DataTable newDataTable;
        public PageInventory()
        {
            InitializeComponent();

            UpdateTable();
        }
               
        
        private void Search_Changed(object sender, object e)
        {
            string textToFind1 = SearchTextBox.Text;
            string textToFind2 = "";

            if (comboBoxCategory.SelectedIndex != -1)
            {
                textToFind2 = comboBoxCategory.SelectedItem.ToString();
            }

            var searcherData = new SearcherDataTable()
            {
                search_words = new string[] { textToFind1, textToFind2 },
                search_columns = new int[] { 1, 2 },
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

            var win = new AddEditProduct(index);
            win.ShowDialog();

            if (win.result)
            {
                UpdateTable();
            }


        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddEditProduct();
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
                var inquiry = $"DELETE FROM товары where товары.идтовара = {index}";
                SQL.SQLConnect();
                SQL.Execute(inquiry);
                SQL.Close();
                UpdateTable();
            }

        }


        void UpdateTable()
        {
            var inquiry = @"select товары.идтовара, товары.название Название, категориитоваров.название Категория, товары.цена Цена, товары.колвонаскладе 'Кол-во на складе'  from товары, категориитоваров
where товары.идкатегории = категориитоваров.идкатегории";
            string inquiry2 = String.Format("select * from категориитоваров");

            SQL.SQLConnect();
            SearchTextBox.Text = "";
            dataTableMain = SQL.Inquiry(inquiry);
            dataTableCategory = SQL.Inquiry(inquiry2);
            newDataTable = dataTableMain.Copy();
            SQL.Close();
            dataGridMain.ItemsSource = dataTableMain.AsDataView();
            //main_data_grid.Columns[0].Visibility = Visibility.Collapsed; // Скрываем первый столбец с ID
            comboBoxCategory.Items.Clear();

            foreach (DataRow item in dataTableCategory.Rows)
            {
                comboBoxCategory.Items.Add(item[1].ToString()); // Заполнение КомбоБокса
            }
        }
    }
}
