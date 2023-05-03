using CustomerManagementSystem.Core;
using CustomerManagementSystem.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

namespace CustomerManagementSystem
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>

    public partial class MainWindow : Window
    {


        public MainWindow()

        {
            InitializeComponent();
            Controller.Pages = new PagesController(mainFrame);
            Controller.Pages.NewPage(new PageInventory());

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var win = new Window3();
            win.Show();
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btnWindowMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnWindowMaximized_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void btnWindowClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_ClickCreatePage(object sender, RoutedEventArgs e)
        {
            int indexButton = int.Parse(((RadioButton)sender).Tag.ToString());
            switch (indexButton)
            {
                case 1:
                    Controller.Pages.NewPage(new PageInventory());
                    break;
                case 2:
                    Controller.Pages.NewPage(new PageSale());
                    break;
                case 3:
                    Controller.Pages.NewPage(new PageOrder());
                    break;
                case 4:
                    Controller.Pages.NewPage(new PageEmployee());
                    break;
                case 5:
                    Controller.Pages.NewPage(new PageSuppliers());
                    break;
            }

        }

    }
}
