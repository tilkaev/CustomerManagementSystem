using CustomerManagementSystem.Core;
using CustomerManagementSystem.Models;
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
    /// Логика взаимодействия для AddEditSale.xaml
    /// </summary>
    public partial class AddEditOrder : Page
    {

        DataTable dataTableMain;
        DataTable newDataTable;
        int idorder = -1;
        DateTime dateorder;
        string firstLongInquiry = "";
        ProductSearch winproductSearch;
        static DataGrid staticDataGrid;
        static List<SaleDetails> listSaleTovari = new List<SaleDetails>();

        public AddEditOrder(DataRowView selectedRow = null)
        {
            InitializeComponent();
            Controller.WindowAddEditOrder = this;
            staticDataGrid = dataGridMain;
            listSaleTovari = new List<SaleDetails>();
            winproductSearch = new ProductSearch();
            datePickerDateSale.Text = DateTime.Now.Date.ToString();

            if (selectedRow != null) // Cостояние изменения продажи
            {
                idorder = (int)selectedRow[0];
                dateorder = DateTime.Parse(selectedRow[2].ToString());

                lblName.Content = $"Изменение заказа #{idorder}";
                btnSave.Content = "Сохранить";
                datePickerDateSale.Text = dateorder.ToString();

                var inquiry = $@"select * from деталипродажа, товары where деталипродажа.идтовара=товары.идтовара and деталипродажа.идпродажи = {idorder}";
                SQL.SQLConnect();
                dataTableMain = SQL.Inquiry(inquiry);
                newDataTable = dataTableMain.Copy();
                SQL.Close();

                foreach (DataRow item in dataTableMain.Rows)
                {
                    firstLongInquiry += $"DELETE FROM деталипродажа where иддеталипродажа = {item[0].ToString()}; ";

                    var tovar = new Tovar()
                    {
                        IDTovara = int.Parse(item[5].ToString()),
                        Name = item[7].ToString()
                    };

                    var ssa = item[4].ToString().Remove(item[4].ToString().Length - 5);
                    var zapis = new SaleDetails()
                    {
                        ID = int.Parse(item[0].ToString()),
                        Tovar = tovar,
                        Price = int.Parse(ssa),
                        Qty = int.Parse(item[3].ToString()),
                    };

                    listSaleTovari.Add(zapis);
                }
                lblSum.Content = SaleDetails.SumSaleDetail(listSaleTovari);
            }
            dataGridMain.ItemsSource = listSaleTovari;
        }

        public static void AddTovar(DataRow dtTovar)
        {
            DataRow item = dtTovar;
            var tovar = new Tovar()
            {
                IDTovara = int.Parse(item[0].ToString()),
                Name = item[1].ToString()
            };

            var zapis = new SaleDetails()
            {
                Tovar = tovar,
                Price = int.Parse(item[3].ToString()),
                Qty = 1,
            };

            var sd = listSaleTovari.Find(tov => tov.Tovar.IDTovara == tovar.IDTovara);
            if (sd == null)
            {
                listSaleTovari.Add(zapis);
            }
            else
            {
                sd.Qty++;
                //MessageBox.Show("Товар добавлен!", "Ошибка!");
                //staticDataGrid.Focus();
            }
            staticDataGrid.Items.Refresh();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string longInquiry = "";
            if (idorder != -1)   // Созданная продажа, изменить строки
            {
                if (listSaleTovari.Count == 0) // Если товары не добавлены
                {
                    MessageBox.Show("Не добавлены товары для сохранения продажи!", "Документ не может быть пустым!");
                    return;
                }

                foreach (var item in listSaleTovari)
                {
                    longInquiry += $"Insert into деталипродажа (идпродажи, идтовара, количество, цена) values ('{idorder}', '{item.Tovar.IDTovara}', '{item.Qty}', '{item.Price}'); ";
                }

                SQL.SQLConnect();
                SQL.Inquiry(firstLongInquiry + longInquiry);
                SQL.Close();
            }
            else                // Создать новую продажу
            {
                if (listSaleTovari.Count == 0) // Если товары не добавлены
                {
                    MessageBox.Show("Не добавлены товары для создания продажи!", "Ошибка!");
                    return;
                }

                longInquiry += $"Insert into продажа (идсотрудника, датапродажи) values ('{Controller.IdAuthorizedEmployee}', '{DateTime.Now}'); DECLARE @lastid INT set @lastid = @@identity; ";
                foreach (var item in listSaleTovari)
                {
                    longInquiry += $"Insert into деталипродажа (идпродажи, идтовара, количество, цена) values (@lastid, '{item.Tovar.IDTovara}', '{item.Qty}', '{item.Price}'); ";
                }

                SQL.SQLConnect();
                SQL.Inquiry(firstLongInquiry + longInquiry);
                SQL.Close();

            }
            Controller.PagesController.mainFrame.GoBack();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Controller.PagesController.mainFrame.GoBack();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            winproductSearch.Closed += (sender2, e2) =>
            {
                winproductSearch = new ProductSearch();
                lblSum.Content = SaleDetails.SumSaleDetail(listSaleTovari);
                if (listSaleTovari.Count != 0 & idorder != -1)
                {
                    btnDeleteSale.Visibility = Visibility.Hidden;
                }
            };
            winproductSearch.ShowDialog();
            winproductSearch.Focus();
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridMain.SelectedIndex == -1)
            {
                MessageBox.Show("Не выбран товар для удаления из продажи", "Ошибка");
                return;
            }
            listSaleTovari.RemoveAt(dataGridMain.SelectedIndex);
            dataGridMain.Items.Refresh();
            lblSum.Content = SaleDetails.SumSaleDetail(listSaleTovari);
            if (listSaleTovari.Count == 0 & idorder != -1)
            {
                btnDeleteSale.Visibility = Visibility.Visible;
            }
        }


        private async void dataGridMain_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            await Task.Delay(100);
            dataGridMain.IsReadOnly = true;
            dataGridMain.Items.Refresh();
            dataGridMain.IsReadOnly = false;
            lblSum.Content = SaleDetails.SumSaleDetail(listSaleTovari);

        }

        private void btnDeleteSale_Click(object sender, RoutedEventArgs e)
        {
            var resultWin = MessageBox.Show("Вы уверены что хотите удалить выбранную продажу!", "Удаление", MessageBoxButton.YesNo);

            if (resultWin == MessageBoxResult.Yes)
            {
                var inquiry = $"DELETE FROM продажа where идпродажи = {idorder}";
                SQL.SQLConnect();
                SQL.Execute(inquiry);
                SQL.Close();
            }
            Controller.PagesController.mainFrame.GoBack();
        }
    }
}
