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

        DataTable dataTableMain;
        DataTable newDataTable;
        int idsale;
        string firstLongInquiry = "";
        ProductSearch winproductSearch;
        static DataGrid staticDataGrid;
        static List<SaleDetails> listSaleTovari = new List<SaleDetails>();

        public AddEditSale(int idsale =- 1)
        {
            InitializeComponent();
            Controller.WindowAddEditSale = this;
            staticDataGrid = dataGridMain;
            listSaleTovari = new List<SaleDetails>();
            winproductSearch = new ProductSearch();


            this.idsale = idsale;

            if (idsale != -1)
            {
                lblName.Content = $"Изменение продажи #{idsale}";
                btnSave.Content = "Сохранить";

                var inquiry = $@"select * from деталипродажа, товары where деталипродажа.идтовара=товары.идтовара and деталипродажа.идпродажи = {idsale}";
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
                MessageBox.Show("Товар добавлен!", "Ошибка!");
                staticDataGrid.Focus();
            }
            staticDataGrid.Items.Refresh();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string longInquiry = "";
            if (idsale != -1)
            {
                foreach (var item in listSaleTovari)
                {
                    longInquiry += $"Insert into деталипродажа (идпродажи, идтовара, количество, цена) values ('{idsale}', '{item.Tovar.IDTovara}', '{item.Qty}', '{item.Price}'); ";
                }

                SQL.SQLConnect();
                SQL.Inquiry(firstLongInquiry + longInquiry);
                SQL.Close();
                Controller.Pages.mainFrame.GoBack();
            }
            else
            {
                longInquiry += $"Insert into продажа (идсотрудника, датапродажи) values ('{Controller.IdAuthorizedEmployee}', '{DateTime.Now}'); DECLARE @lastid INT set @lastid = @@identity; ";
                foreach (var item in listSaleTovari)
                {
                    longInquiry += $"Insert into деталипродажа (идпродажи, идтовара, количество, цена) values (@lastid, '{item.Tovar.IDTovara}', '{item.Qty}', '{item.Price}'); ";
                }

                SQL.SQLConnect();
                SQL.Inquiry(firstLongInquiry + longInquiry);
                SQL.Close();
                Controller.Pages.mainFrame.GoBack();
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Controller.Pages.mainFrame.GoBack();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            winproductSearch.Closed += (sender2, e2) => {
                winproductSearch = new ProductSearch();
            };
            winproductSearch.Show();
            winproductSearch.Focus();
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            listSaleTovari.RemoveAt(dataGridMain.SelectedIndex);
            dataGridMain.Items.Refresh();
            lblSum.Content = SaleDetails.SumSaleDetail(listSaleTovari);
        }


        private async void dataGridMain_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            await Task.Delay(100);
            dataGridMain.IsReadOnly = true;
            dataGridMain.Items.Refresh();
            dataGridMain.IsReadOnly = false;
            lblSum.Content = SaleDetails.SumSaleDetail(listSaleTovari);

        }

    }
}
