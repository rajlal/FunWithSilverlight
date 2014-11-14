using System;
using System.Windows;
using System.Windows.Controls;
namespace CustomizeControl
{
    public partial class PageTemplateVSM: UserControl
    {
        public PageTemplateVSM()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VistaButton.IsEnabled = !VistaButton.IsEnabled;
            if (VistaButton.IsEnabled)
                Disable.Content = "Disable";
            else
                Disable.Content = "Enable";
        }
    }
}
