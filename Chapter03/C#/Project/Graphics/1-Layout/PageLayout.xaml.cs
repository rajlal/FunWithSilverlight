using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Layout
{
    public partial class PageLayout : UserControl
    {
        private string SelectedItem = "Border";
        private string SelectedCanvas = "Border";
        private Storyboard myStoryboard = new Storyboard();
        public PageLayout()
        {
            InitializeComponent();
        }
        private void showMargin(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CommonItems.Visibility = Visibility.Visible;
            CanvasMargin.Visibility = Visibility.Visible;
            SelectedCanvas = "Margin";
            rectFixed.Fill = new SolidColorBrush(Colors.Green);
            rectBorderFixed.Fill = new SolidColorBrush(Colors.Green);
            layoutRectangleFixed.BorderThickness = new Thickness(1.0);
            layoutBorderFixed.BorderThickness = new Thickness(1.0);

            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void showBorder(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CommonItems.Visibility = Visibility.Visible;
            CanvasBorder.Visibility = Visibility.Visible;
            SelectedCanvas = "Border";

            rectFixed.Fill = new SolidColorBrush(getColorFromHex("FF1E90FF"));
            rectBorderFixed.Fill = new SolidColorBrush(getColorFromHex("FF1E90FF"));

            layoutRectangleFixed.BorderThickness = new Thickness(1.0);
            layoutBorderFixed.BorderThickness = new Thickness(1.0);
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void showPadding(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasPadding.Visibility = Visibility.Visible;
            SelectedCanvas = "Padding";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
     
        private void CollapseAll()
        {
            CommonItems.Visibility = Visibility.Collapsed;
            CanvasMargin.Visibility = Visibility.Collapsed;
            CanvasBorder.Visibility = Visibility.Collapsed;
            CanvasPadding.Visibility = Visibility.Collapsed;
        }

        private void sliderBorder_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SelectedItem =="Border")
            layoutBorderFixed.BorderThickness = new Thickness(sliderBorder.Value);
            else
                layoutRectangleFixed.BorderThickness = new Thickness(sliderBorder.Value);

            bThick.Text = string.Format("{0:0#}", sliderBorder.Value) + " px";
        }

        private void ChangeOption(object sender, RoutedEventArgs e)
        {
            try
            {
                sliderBorder.Value=1.0;
                sliderMargin.Value = 1.0;
                RadioButton rb = sender as RadioButton;
                if (rb.Name == "FixedBorder")
                {
                    SelectedItem = "Border";
                    layoutRectangleFixed.Visibility = Visibility.Collapsed;
                    layoutBorderFixed.Visibility = Visibility.Visible;
                    
                    bWidth.Text = "150 px";
                    bHeight.Text = "100 px";
                    rWidth.Text = "n/a";
                    rHeight.Text = "n/a";

                }
                else
                {
                    SelectedItem = "Rectangle";
                    layoutBorderFixed.Visibility = Visibility.Collapsed;
                    layoutRectangleFixed.Visibility = Visibility.Visible;
                    bWidth.Text = "n/a";
                    bHeight.Text = "n/a";
                    rWidth.Text = "150 px";
                    rHeight.Text = "100 px";
                }
            }
            catch (Exception)
            { }

        }
        public Color getColorFromHex(string s)
        {
            byte a = System.Convert.ToByte(s.Substring(0, 2), 16);
            byte r = System.Convert.ToByte(s.Substring(2, 2), 16);
            byte g = System.Convert.ToByte(s.Substring(4, 2), 16);
            byte b = System.Convert.ToByte(s.Substring(6, 2), 16);
            return Color.FromArgb(a, r, g, b);
        }

        private void sliderMargin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {

                if (SelectedItem == "Border")
                    rectBorderFixed.Margin = new Thickness(sliderMargin.Value);
                else
                    rectFixed.Margin = new Thickness(sliderMargin.Value);

                bMargin.Text = string.Format("{0:0#}", sliderMargin.Value) + " px";

            }
            catch (Exception)
            { }
        }

        private void sliderFont_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                txtPaddingFont.Text = "Fontsize: " + string.Format("{0:0#}", sliderFont.Value);
                txtPadding.FontSize = sliderFont.Value;
                txtMargin.FontSize = sliderFont.Value;
            }
            catch (Exception)
            { }
        }

        private void sliderValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
            txtPadding.Padding = new Thickness(sliderValue.Value);
            txtMargin.Margin = new Thickness(sliderValue.Value);

            string pad = string.Format("{0:0#}", sliderValue.Value);

            txtPaddingValue.Text = "Padding: \"" + pad + "," + pad + "," + pad + "," + pad + "\"";
            txtMarginValue.Text = "Padding: \"" + pad + "," + pad + "," + pad + "," + pad + "\"";

             }
            catch (Exception)
            { }
        }
     

        

      


    }
}
