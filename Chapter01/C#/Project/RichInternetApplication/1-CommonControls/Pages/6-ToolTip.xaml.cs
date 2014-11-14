using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace CommonControls
{
    public partial class PageToolTip : UserControl
    {
        public PageToolTip()
        {
            InitializeComponent();
            SetToolTip3();
        }
        private void SetToolTip3()
        {
           ToolTip tt = new ToolTip();
           tt.Template = (ControlTemplate) Resources["ToolTipTemplate"];
            tt.Content = new TextBlock() {
               FontFamily = new FontFamily("Arial"),
               FontSize = 12, Text = "Tooltip generated in the code.",
               TextWrapping = TextWrapping.Wrap };
           ToolTipService.SetToolTip(ToolTip3, tt);
        }
    }
}
