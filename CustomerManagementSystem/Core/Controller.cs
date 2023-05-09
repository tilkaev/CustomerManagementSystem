﻿using CustomerManagementSystem.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomerManagementSystem.Core
{
    class Controller
    {
        public static int IdAuthorizedEmployee { get; set; }

        public static PagesController PagesController { get; set; }

        public static AddEditSale WindowAddEditSale { get; set; }

        public static AddEditOrder WindowAddEditOrder { get; set; }

    }
}
