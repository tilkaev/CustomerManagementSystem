using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace CustomerManagementSystem
{
    class SQL
    {
        //public static SqlConnection connect { get; set; }

        static SQL()
        {
            connect = new SqlConnection(@"data source=TIMUR-HOME\SQLEXPRESS;initial catalog=SkladSpektr;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework");
        }


        public static SqlConnection connect;


        public static void SQLConnect()
        {
            try
            {
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
                Close();
            }
        }


        public static DataTable Inquiry(string sql) //Возвращаем результат запроса
        {
            DataTable inv = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, connect))
                {
                    SqlDataReader result = cmd.ExecuteReader();
                    inv.Load(result);
                    return inv;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
                Close();
                return null;
            }

        }

        public static bool Execute(string sql)
        {
            try
            {

                using (SqlCommand cmd = new SqlCommand(sql, connect))
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
                Close();
                return false;
            }
        }

        public static void Close()
        {
            try
            {
                connect.Close();
            }
            catch (Exception)
            {
                return;
            }
        }

    }
}
