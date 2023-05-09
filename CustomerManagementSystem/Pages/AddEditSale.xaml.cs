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
    public partial class AddEditSale : Page
    {

        DataTable dataTableSaleDetails;
        DataTable newDataTableSaleDetails;
        DataTable dataTableCustomer;
        DataTable dataTableSale;
        ProductSearch winproductSearch;
        static int idsale = -1;
        static DateTime datesale;
        static DataGrid staticDataGrid;
        static List<SaleDetails> listSaleTovari;

        public AddEditSale(DataRowView selectedRow = null)
        {
            InitializeComponent();
            Controller.WindowAddEditSale = this;
            staticDataGrid = dataGridMain;
            listSaleTovari = new List<SaleDetails>();
            winproductSearch = new ProductSearch();
            datesale = DateTime.Now.Date;
            datePickerDateSale.Text = datesale.ToString();


            string queryCustomer = String.Format("select идклиента, наименование from клиенты");
            SQL.SQLConnect();
            dataTableCustomer = SQL.Inquiry(queryCustomer);
            comboBoxCustomer.Items.Clear();
            foreach (DataRow item in dataTableCustomer.Rows)
            {
                comboBoxCustomer.Items.Add(item[1].ToString()); // Заполнение КомбоБокса
            }


            if (selectedRow != null) // Cостояние изменения продажи
            {
                idsale = (int)selectedRow[0];
                datesale = DateTime.Parse(selectedRow[2].ToString());

                var querySale = $@"select датапродажи, идклиента from продажа where идпродажи = {idsale}";
                var querySaleDetails = $@"select * from деталипродажа, товары where деталипродажа.идтовара=товары.идтовара and деталипродажа.идпродажи = {idsale}";

                dataTableSale = SQL.Inquiry(querySale);
                dataTableSaleDetails = SQL.Inquiry(querySaleDetails);
                newDataTableSaleDetails = dataTableSaleDetails.Copy();

                lblName.Content = $"Изменение продажи #{idsale}";
                btnSave.Content = "Сохранить";

                datePickerDateSale.Text = dataTableSale.Rows[0][0].ToString();

                DataRow[] foundRows = dataTableCustomer.Select($"[идклиента] = '{dataTableSale.Rows[0][1]}'");
                if (foundRows.Length > 0)
                {
                    int index = dataTableCustomer.Rows.IndexOf(foundRows[0]);
                    comboBoxCustomer.SelectedIndex = index;
                }

                foreach (DataRow item in dataTableSaleDetails.Rows)
                {
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
            SQL.Close();
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
            }
            staticDataGrid.Items.Refresh();
        }


        private void btnSaveSale_Click(object sender, RoutedEventArgs e)
        {

            string date = datePickerDateSale.Text;
            if (date == null)
                date = DateTime.Now.Date.ToString();


            int customer_index = comboBoxCustomer.SelectedIndex;
            if (customer_index == -1)
            {
                MessageBox.Show("Выберите клиента!", "Ошибка!");
                return;
            }

            if (listSaleTovari.Count == 0) // Если товары не добавлены
            {
                MessageBox.Show("Не добавлены товары в документе продажа!", "Документ не может быть пустым!");
                return;
            }


            string longQuery = "";
            if (idsale == -1) // Создание продажи
            {
                var idcustomer = int.Parse(dataTableCustomer.Rows[1][comboBoxCustomer.SelectedIndex].ToString());
                longQuery += $"Insert into продажа (идсотрудника, идклиента, датапродажи) values ('{Controller.IdAuthorizedEmployee}', '{idcustomer}', '{date}'); DECLARE @lastid INT set @lastid = @@identity; ";
                foreach (var item in listSaleTovari)
                {
                    longQuery += $"Insert into деталипродажа (идпродажи, идтовара, количество, цена) values (@lastid, '{item.Tovar.IDTovara}', '{item.Qty}', '{item.Price}'); ";
                }

                SQL.SQLConnect();
                SQL.Inquiry(longQuery);
                SQL.Close();
            }

            else // Изменение продажи
            {
                var idcustomer = int.Parse(dataTableCustomer.Rows[comboBoxCustomer.SelectedIndex][0].ToString());
                datesale = DateTime.Parse(datePickerDateSale.Text);
                longQuery += $"UPDATE продажа SET идклиента = {idcustomer}, датапродажи = '{datesale}' WHERE идпродажи = {idsale}; ";
                foreach (var item in listSaleTovari)
                {
                    longQuery += $"Insert into деталипродажа (идпродажи, идтовара, количество, цена) values ('{idsale}', '{item.Tovar.IDTovara}', '{item.Qty}', '{item.Price}'); ";
                }

                var preInquiry = $"DELETE FROM деталипродажа where идпродажи = {idsale};";

                SQL.SQLConnect();
                SQL.Inquiry(preInquiry + longQuery);
                SQL.Close();
            }
            Controller.PagesController.mainFrame.GoBack();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Controller.PagesController.mainFrame.GoBack();
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            lblName.Content = $"Изменение продажи #{idsale} *";
            winproductSearch.Closed += (sender2, e2) =>
            {
                winproductSearch = new ProductSearch();
                lblSum.Content = SaleDetails.SumSaleDetail(listSaleTovari);
                if (listSaleTovari.Count != 0 & idsale != -1)
                {
                    btnDeleteSale.Visibility = Visibility.Hidden;
                }
            };
            winproductSearch.ShowDialog();
            winproductSearch.Focus();
        }

        private void btnDelProduct_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridMain.SelectedIndex == -1)
            {
                MessageBox.Show("Не выбран товар для удаления из продажи", "Ошибка");
                return;
            }

            lblName.Content = $"Изменение продажи #{idsale} *";


            listSaleTovari.RemoveAt(dataGridMain.SelectedIndex);
            dataGridMain.Items.Refresh();
            lblSum.Content = SaleDetails.SumSaleDetail(listSaleTovari);
            if (listSaleTovari.Count == 0 & idsale != -1)
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
                var inquiry = $"DELETE FROM продажа where идпродажи = {idsale}";
                SQL.SQLConnect();
                SQL.Execute(inquiry);
                SQL.Close();
                Controller.PagesController.mainFrame.GoBack();
            }
        }


    }
}
