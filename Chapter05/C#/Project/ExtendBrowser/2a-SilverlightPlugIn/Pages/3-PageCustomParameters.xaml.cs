using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightPlugIn
{
    public partial class PageCustomParameters : UserControl
    {
        public PageCustomParameters(StartupEventArgs e)
        {
            InitializeComponent();

           foreach (String key in e.InitParams.Keys)
                {
                    txtValues.Text += key + ": " + e.InitParams[key].ToString() + "\n";
                }   
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
