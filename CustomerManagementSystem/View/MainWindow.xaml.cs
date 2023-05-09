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
    public partial class MainWindow : Window
    {
        public MainWindow()

        {
            InitializeComponent();
            Controller.PagesController = new PagesController(mainFrame);
            Controller.PagesController.NewPage(new PageProducts());

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
            var indexButton = ((RadioButton)sender).Tag.ToString();
            switch (indexButton)
            {
                case "Products":
                    if (!(Controller.PagesController.mainFrame.Content is PageProducts))
                        Controller.PagesController.NewPage(new PageProducts());
                    break;
                case "Sales":
                    if (!(Controller.PagesController.mainFrame.Content is PageSale))
                        Controller.PagesController.NewPage(new PageSale());
                    break;
                case "Customers":
                    // Страница с клиентами
                    break;
                case "Orders":
                    if (!(Controller.PagesController.mainFrame.Content is PageOrder))
                        Controller.PagesController.NewPage(new PageOrder());
                    break;
                case "Employees":
                    if (!(Controller.PagesController.mainFrame.Content is PageEmployee))
                        Controller.PagesController.NewPage(new PageEmployee());
                    break;
                case "Suppliers":
                    if (!(Controller.PagesController.mainFrame.Content is PageSuppliers))
                        Controller.PagesController.NewPage(new PageSuppliers());
                    break;
                case "Reports":
                    // Страница с отчетами
                    break;
            }

        }

    }
}
