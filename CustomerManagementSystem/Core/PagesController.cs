using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CustomerManagementSystem.Core
{
    class PagesController
    {
        bool importantAction = false;

        public delegate void InvokeSave();
        public InvokeSave invokeSave;


        public Frame mainFrame;
        Page currentPage;

        public PagesController(Frame frame)
        {
            mainFrame = frame;
        }


        public void NewPage(Page page)
        {
            if (importantAction)
            {
                var result = MessageBox.Show("Данные были изменены. Сохранить изменения?", "Спектр Склад", MessageBoxButton.YesNoCancel);
                if (result is MessageBoxResult.Yes)
                {
                    //Save changes //////////////////////////////////////////////////////
                    invokeSave();
                    mainFrame.Navigate(page);
                    currentPage = page;
                }
                else if (result is MessageBoxResult.No) // Undo changes
                {
                    mainFrame.Navigate(page);
                    currentPage = page;
                }
                else if (result is MessageBoxResult.Cancel)
                {
                    return;
                }
                importantAction = false;
            }
            else
            {
                mainFrame.Navigate(page);
                currentPage = page;
            }
        }


    }
}
