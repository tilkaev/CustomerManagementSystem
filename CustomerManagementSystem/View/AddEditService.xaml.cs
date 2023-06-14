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
    public partial class AddEditService : Window
    {
        DataTable dataTable;
        int idservice;
        public bool result = false;

        public AddEditService(int idservice = -1)
        {
            InitializeComponent();

            this.idservice = idservice;
            
            if (idservice != -1)
            {
                string sql2 = String.Format($"select * from услуги where идуслуги = { idservice}");
                SQL.SQLConnect();
                dataTable = SQL.Inquiry(sql2);
                SQL.Close();

                textBox1.Text = dataTable.Rows[0][1].ToString();
                textBox3.Text = dataTable.Rows[0][2].ToString();
                btnOk.Content = "Сохранить";
                labelTop.Content = "Услуга (изменение)";
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
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Введите название услуги!");
                return;
            }
            float price;
            if (!float.TryParse(textBox3.Text.Replace('.', ','), out price))
            {
                MessageBox.Show("Введите корректные данные в поле 'Стоимость'!");
                return;
            }
            if (price <= 0)
            {
                MessageBox.Show("Стоимость не можеть быть равна или меньше 0 !");
                return;
            }



            string sql = $"Insert into услуги (наименование, стоимость) values ('{textBox1.Text}', {price.ToString().Replace(',', '.')})";
            if (idservice != -1)
            {
                sql = $"UPDATE услуги SET наименование = '{textBox1.Text}', стоимость = {price.ToString().Replace(',', '.')} where идуслуги = {idservice}";
            }

            SQL.SQLConnect();

            SQL.Inquiry(sql);

            result = true;
            SQL.Close();
            this.Close();

        }
    }
}
