using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace ReusableControl
{
    public partial class Thumbnail : UserControl
    {
        private bool ToolTipFlag;
        public Thumbnail()
        {
            InitializeComponent();
        }
        private void ThumbImage_MouseEnter(object sender, MouseEventArgs e)
        {
                ImageSetToolTip();
          
        }
        private void ImageSetToolTip()
        {
            if (!ToolTipFlag)
            {

                ToolTip tt = new ToolTip();
                tt.Template = (ControlTemplate)Resources["ToolTipTemplate"];
                Border brdrMain = new Border();
                brdrMain.BorderThickness = new Thickness(1);
                brdrMain.BorderBrush = new SolidColorBrush(Colors.Gray);
                
                StackPanel ImageStackPanel = new StackPanel();

                Border brdr = new Border();
                brdr.BorderThickness = new Thickness(1);
                brdr.Background = new SolidColorBrush(Colors.LightGray);
                

                TextBlock k = new TextBlock();
                k.Text = ThumbnailText.Text;
                k.Foreground = new SolidColorBrush(Colors.Black);
                k.TextAlignment = TextAlignment.Center;
                brdr.Child = k;

                Image mainImage = new Image();
                mainImage.Source = ThumbImage.Source; 
                mainImage.Width = 200;
                ImageStackPanel.Children.Add(mainImage);
                ImageStackPanel.Children.Add(brdr);
           
                brdrMain.Child = ImageStackPanel;
                tt.Content = brdrMain;
                tt.Cursor = Cursors.Hand;
                ToolTipService.SetToolTip(ThumbImage, tt);
                tt.IsOpen = true;
                ToolTipFlag = true;
            }
            
            
        }
        private void ThumbImage_MouseLeave(object sender, MouseEventArgs e)
        {
            ToolTipService.SetToolTip(ThumbImage,null);
            ToolTipFlag = false;
        }
        private void ThumbImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            this.DataContext = new ThumbnailData {ImageUri = "files/silverlight.jpg", Title = "Image Error" };
        }
        public Double ThumbWidth
        {
            get { return ThumbImage.Width; }
            set 
            {
                ThumbImage.Width = value;
                ThumbShadow.Width = value + 2;
                ThumbGrid.Width = value ;
                ThumbBorder.Width = value;
            }

        }
        public Double ThumbHeight
        {
            get { return ThumbImage.Height; }
            set
            {
                ThumbImage.Height = value;
                ThumbShadow.Height = value + 22;
                ThumbGrid.Height = value + 20;
                ThumbBorder.Height = value + 20;
            }

        }
    }
    public class ThumbnailData
    {
        public string ImageUri { get; set; }
        public string Title { get; set; }
    }
}
