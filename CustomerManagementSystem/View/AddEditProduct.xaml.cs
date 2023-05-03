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
    public partial class AddEditProduct : Window
    {
        DataTable dataTable;
        DataTable dataTable2;
        int idtovar;
        public bool result = false;

        public AddEditProduct(int idtovar = -1)
        {
            InitializeComponent();

            this.idtovar = idtovar;
            string sql = String.Format("select * from категориитоваров");
            string sql2 = String.Format($"select * from товары where идтовара = {idtovar}");
            SQL.SQLConnect();
            dataTable = SQL.Inquiry(sql);
            dataTable2 = SQL.Inquiry(sql2);
            SQL.Close();

            int index = 0;
            foreach (DataRow item in dataTable.Rows)
            {
                if (idtovar != -1)
                {
                    if (item[0].ToString() == dataTable2.Rows[0][1].ToString())
                    {
                        comboBox2.SelectedIndex = index;
                        textBox1.Text = dataTable2.Rows[0][2].ToString();
                        textBox3.Text = dataTable2.Rows[0][3].ToString();
                        btnOk.Content = "Сохранить";
                        labelTop.Content = "Товар (изменение)";
                    }
                }
                comboBox2.Items.Add(item[1].ToString()); // Заполнение КомбоБокса
                index++;
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
                MessageBox.Show("Введите название товара!");
                return;
            }
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите категорию товара!");
                return;
            }
            float price;
            if (!float.TryParse(textBox3.Text.Replace('.', ','), out price))
            {
                MessageBox.Show("Введите корректные данные в поле 'Цена'!");
                return;
            }
            if (price <= 0)
            {
                MessageBox.Show("Цена не можеть быть равна или меньше 0 !");
                return;
            }



            string sql = $"Insert into товары (название, идкатегории, цена) values ('{textBox1.Text}', '{dataTable.Rows[comboBox2.SelectedIndex][0]}', {price.ToString().Replace(',', '.')})";
            if (idtovar != -1)
            {
                sql = $"UPDATE товары SET название = '{textBox1.Text}', идкатегории = {dataTable.Rows[comboBox2.SelectedIndex][0]}, цена = {price.ToString().Replace(',', '.')} where идтовара = {idtovar}";
            }

            SQL.SQLConnect();

            SQL.Inquiry(sql);

            result = true;
            SQL.Close();
            this.Close();

        }
    }
}
