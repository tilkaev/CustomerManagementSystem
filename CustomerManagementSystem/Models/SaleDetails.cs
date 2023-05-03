using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomerManagementSystem.Models
{

    class Tovar
    {
        public int IDTovara { get; set; }

        public string Name { get; set; }
    }


    class SaleDetails
    {
        public int ID { get; set; }

        public Tovar Tovar { get; set; }

        int qty = 1;
        public int Qty {
            set
            {
                if (value <= 0)
                {
                    MessageBox.Show("Количество не может быть меньше или равно 0!", "Ошибка");
                }
                else
                {
                    qty = value;
                    this.Sum = 1;
                }
            }
            get { return qty; }
        }

        public int Price { get; set; }

        int sum = 0;
        public int Sum
        {
            set
            {
                sum = this.Qty*this.Price;
            }
            get { return sum; }
        }


        public static int SumSaleDetail(List<SaleDetails> saleDetails)
        {
            int sum = 0;
            foreach (var item in saleDetails)
            {
                sum += item.Qty * item.Price;
            }
            return sum;
        }
    }
}
