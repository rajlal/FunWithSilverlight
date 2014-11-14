using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Windows.Browser;
using System.Windows.Markup;
namespace HelloSilverlightWorld
{
    public partial class Page : UserControl
    {
        bool ResetFlag;
        public Page()
        {
            InitializeComponent();
        }
        private void LayoutRoot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UIElement s = (UIElement)e.OriginalSource;
            Point MousePosition = e.GetPosition(LayoutRoot);
            AddSmiley(MousePosition.X, MousePosition.Y);
        }
        private void Smiley_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
                if (HtmlPage.Window.Confirm("Delete Smiley?"))
                {
                    RemoveSmiley(sender);
                }
        }
        private void Reset_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RemoveAllSmiley();
        }      
        private void AddSmiley(double x, double y)
        {
            XmlReader xmlReader ;
                
                if ((int)x % 2==0) 
                xmlReader = XmlReader.Create("files/smiley.xaml");
                else
                xmlReader = XmlReader.Create("/HelloSilverlight;component/files/smileypink.xaml");

                xmlReader.MoveToContent();
                UIElement MySmiley = (UIElement)XamlReader.Load(xmlReader.ReadOuterXml());
                MySmiley.SetValue(Canvas.TopProperty, y);
                MySmiley.SetValue(Canvas.LeftProperty, x);
                MySmiley.MouseLeftButtonDown += new MouseButtonEventHandler(Smiley_MouseLeftButtonUp);
                LayoutRoot.Children.Add(MySmiley);
                if (!ResetFlag) ResetText();
        }
        private void ResetText()
        {
            ResetFlag = true;
            Reset.Text = "(Click To Reset)";
            Reset.MouseLeftButtonDown += new MouseButtonEventHandler(Reset_MouseLeftButtonUp);
                
        }
        private void RemoveSmiley(Object sender)
        {
            LayoutRoot.Children.Remove((UIElement)sender);
        }
        private void RemoveAllSmiley()
        {
            if (HtmlPage.Window.Confirm("Delete All Smiley?"))
            {
                int TotalCount = LayoutRoot.Children.Count;
                for (int i = TotalCount - 1; i > 1; i--)
                {
                    LayoutRoot.Children.RemoveAt(i);
                }
                ResetFlag = false;
                Reset.Text = "(Click Anywhere)";
                Reset.MouseLeftButtonDown -= new MouseButtonEventHandler(Reset_MouseLeftButtonUp);
          
            }
            
        }
    }
}
