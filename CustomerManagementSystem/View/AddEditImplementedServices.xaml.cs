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
using System.Windows.Shapes;

namespace CustomerManagementSystem.View
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddEditImplementedServices : Window
    {
        DataTable dataTableCustomer;
        DataTable dataTableServices;
        int idimplementedservice;
        public bool result = false;

        public AddEditImplementedServices(int idimplementedservice = -1)
        {
            InitializeComponent();
            this.idimplementedservice = idimplementedservice;

            datePickerDateImplemented.Text = DateTime.Today.ToString();

            SQL.SQLConnect();
            string queryCustomer = String.Format("select идклиента, наименование from клиенты");
            dataTableCustomer = SQL.Inquiry(queryCustomer);
            comboBoxCustomer.Items.Clear();
            foreach (DataRow item in dataTableCustomer.Rows)
            {
                comboBoxCustomer.Items.Add(item[1].ToString()); // Заполнение КомбоБокса
            }

            string queryServices = String.Format("select идуслуги, concat(наименование, ' - ', стоимость, 'р'), стоимость  from услуги");
            dataTableServices = SQL.Inquiry(queryServices);
            comboBoxServices.Items.Clear();
            foreach (DataRow item in dataTableServices.Rows)
            {
                comboBoxServices.Items.Add(item[1].ToString()); // Заполнение КомбоБокса
            }
            SQL.Close();

            if (idimplementedservice != -1)
            {
                btnOk.Content = "Сохранить";
                labelTop.Content = $"Оказание услуги №{idimplementedservice} (изменение)";
                borderLabelTop.Width = 265;

                string queryimplementedservicer = String.Format($"select * from оказаниеуслуг where идоказаниеуслуг = {idimplementedservice}");
                SQL.SQLConnect();
                var tmpDataTable = SQL.Inquiry(queryimplementedservicer);
                SQL.Close();

                datePickerDateImplemented.Text = tmpDataTable.Rows[0][2].ToString();

                int index = 0;
                foreach (DataRow item in dataTableCustomer.Rows)
                {
                    if (item[0].ToString() == tmpDataTable.Rows[0][3].ToString())
                    {
                        comboBoxCustomer.SelectedIndex = index;
                        break;
                    }
                    index++;
                }


                index = 0;
                foreach (DataRow item in dataTableServices.Rows)
                {
                    if (item[0].ToString() == tmpDataTable.Rows[0][1].ToString())
                    {
                        comboBoxServices.SelectedIndex = index;
                        break;
                    }
                    index++;
                }

                textBoxFinalCost.Text = tmpDataTable.Rows[0][5].ToString();



            }

        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
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

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {

            if (datePickerDateImplemented.Text == "")
            {
                MessageBox.Show("Дата не может быть пустой!");
                return;
            }

            if (comboBoxCustomer.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите клиента!");
                return;
            }

            if (comboBoxServices.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите услугу!");
                return;
            }

            float price;
            if (!float.TryParse(textBoxFinalCost.Text.Replace('.', ','), out price))
            {
                MessageBox.Show("Введите корректные данные в поле 'Цена'!");
                return;
            }
            if (price <= 0)
            {
                MessageBox.Show("Цена не можеть быть равна или меньше 0 !");
                return;
            }


            
            string sql = $@"Insert into оказаниеуслуг (идуслуги, датаоказания, идклиента, идсотрудника, сумма) values ('{dataTableServices.Rows[comboBoxServices.SelectedIndex][0]}', '{datePickerDateImplemented.Text}', {dataTableCustomer.Rows[comboBoxCustomer.SelectedIndex][0]}, {Controller.IdAuthorizedEmployee}, {price.ToString().Replace(',', '.')})";
            if (idimplementedservice != -1)
            {
                sql = $@"UPDATE оказаниеуслуг SET идуслуги = '{dataTableServices.Rows[comboBoxServices.SelectedIndex][0]}', датаоказания = '{datePickerDateImplemented.Text}', идклиента = {dataTableCustomer.Rows[comboBoxCustomer.SelectedIndex][0]}, идсотрудника = {Controller.IdAuthorizedEmployee}, сумма = {price.ToString().Replace(',', '.')} where идоказаниеуслуг = {idimplementedservice}";
            }

            SQL.SQLConnect();

            SQL.Inquiry(sql);

            result = true;
            SQL.Close();
            this.Close();

        }


        private void comboBoxServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var finalcost = dataTableServices.Rows[comboBoxServices.SelectedIndex][2].ToString();
            textBoxFinalCost.Text = finalcost;

        }
    }
}
