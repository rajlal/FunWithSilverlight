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
using System.Windows.Media.Imaging;

namespace ImageManipulation
{
    public partial class PageImage : UserControl
    {
        public PageImage()
        {
            InitializeComponent();
        }

        private void ReadImage(object sender, MouseButtonEventArgs e)
        {

            Image Img = new Image();
            Img.Source = new BitmapImage(new Uri("image/silverlight.png", UriKind.Relative));
            Img.SetValue(Canvas.TopProperty, 18.0);
            Img.SetValue(Canvas.LeftProperty, 80.0);

            DynamicItemContainer.Children.Clear();
            DynamicItemContainer.Children.Add(Img);
            StatusBar.Text = ToolTipService.GetToolTip(ImageDefault).ToString();
        }
        private void StretchImage(object sender, MouseButtonEventArgs e)
        {
            DynamicItemContainer.Children.Clear();

            Grid ImageGrid = new Grid();

            ImageGrid.Width = 320;
            ImageGrid.Height = 200;

            ImageGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ImageGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ImageGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ImageGrid.ColumnDefinitions.Add(new ColumnDefinition());

            RowDefinition r1 = new RowDefinition();
            r1.Height = new GridLength(140);
            RowDefinition r2 = new RowDefinition();
            r2.Height = new GridLength(60);
            
            ImageGrid.RowDefinitions.Add(r1);
            ImageGrid.RowDefinitions.Add(r2);

            Image ImgNone = new Image();
            ImgNone.Source = new BitmapImage(new Uri("image/silverlight.png", UriKind.Relative));
            ImgNone.Stretch = Stretch.None;
            ImgNone.Margin = new Thickness(2);
          
            Image ImgFill = new Image();
            ImgFill.Source = new BitmapImage(new Uri("image/silverlight.png", UriKind.Relative));
            ImgFill.Stretch = Stretch.Fill;
            ImgFill.Margin = new Thickness(2);
  

            Image ImgUniform = new Image();
            ImgUniform.Source = new BitmapImage(new Uri("image/silverlight.png", UriKind.Relative));
            ImgUniform.Stretch = Stretch.Uniform;
            ImgUniform.Margin = new Thickness(2);


            Image ImgUniformToFill = new Image();
            ImgUniformToFill.Source = new BitmapImage(new Uri("image/silverlight.png", UriKind.Relative));
            ImgUniformToFill.Stretch = Stretch.UniformToFill;
            ImgUniformToFill.Margin = new Thickness(2);


            ImageGrid.Children.Add(ImgNone);
            ImageGrid.Children.Add(ImgFill);
            ImageGrid.Children.Add(ImgUniform);
            ImageGrid.Children.Add(ImgUniformToFill);

            TextBlock TextNone = new TextBlock();
            TextNone.Text = " Stretch:None\n Size: same\n Cropped";

            TextBlock TextFill = new TextBlock();
            TextFill.Text = " :Fill\n :Resize\n --";

            TextBlock TextUniform = new TextBlock();
            TextUniform.Text = " :Uniform\n :Resize\n --";
            
            TextBlock TextUniformFill = new TextBlock();
            TextUniformFill.Text = " :UniformFill\n :Resize\n Cropped";

            ImageGrid.Children.Add(TextNone);
            ImageGrid.Children.Add(TextFill);
            ImageGrid.Children.Add(TextUniform);
            ImageGrid.Children.Add(TextUniformFill);

            Grid.SetColumn(TextNone, 0);
            Grid.SetColumn(TextFill, 1);
            Grid.SetColumn(TextUniform, 2);
            Grid.SetColumn(TextUniformFill, 3);
            Grid.SetRow(TextNone, 1);
            Grid.SetRow(TextFill, 1);
            Grid.SetRow(TextUniform, 1);
            Grid.SetRow(TextUniformFill, 1);

            Grid.SetColumn(ImgNone, 0);
            Grid.SetColumn(ImgFill, 1);
            Grid.SetColumn(ImgUniform, 2);
            Grid.SetColumn(ImgUniformToFill, 3);
            

            DynamicItemContainer.Children.Add(ImageGrid);

            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();

    
       
        }
        private void ClipImage(object sender, MouseButtonEventArgs e)
        {
            DynamicItemContainer.Children.Clear();
            Grid ImageGrid = new Grid();
            ImageGrid.Width = 320;
            ImageGrid.Height = 200;

            ImageGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ImageGrid.ColumnDefinitions.Add(new ColumnDefinition());
     
            RowDefinition r1 = new RowDefinition();
            r1.Height = new GridLength(160);
            RowDefinition r2 = new RowDefinition();
            r2.Height = new GridLength(40);

            ImageGrid.RowDefinitions.Add(r1);
            ImageGrid.RowDefinitions.Add(r2);

            Image ImgClip = new Image();
            ImgClip.Source = new BitmapImage(new Uri("image/silverlight.png", UriKind.Relative));
            ImgClip.Margin = new Thickness(2);
            EllipseGeometry eg = new EllipseGeometry();
            eg.RadiusX = 78;
            eg.RadiusY = 72;
            eg.Center = new Point(78, 78);
            ImgClip.Clip = eg;


            Image ImgClip2 = new Image();
            ImgClip2.Source = new BitmapImage(new Uri("image/silverlight.png", UriKind.Relative));
            ImgClip2.Margin = new Thickness(2);

            RectangleGeometry rg = new RectangleGeometry();
            rg.Rect = new Rect(10,10,130,140);
            rg.RadiusX=25;
            rg.RadiusY = 25;
            
            ImgClip2.Clip = rg;

            ImageGrid.Children.Add(ImgClip);
            ImageGrid.Children.Add(ImgClip2);
        
            TextBlock TextE = new TextBlock();
            TextE.Text = " Ellipse\n Geometry";

            TextBlock TextR = new TextBlock();
            TextR.Text = " Rectangle\n Geometry";
           
            ImageGrid.Children.Add(TextE);
            ImageGrid.Children.Add(TextR);
           
            Grid.SetColumn(TextE, 0);
            Grid.SetRow(TextE, 1);
            Grid.SetColumn(TextR, 1);
            Grid.SetRow(TextR, 1);

            Grid.SetColumn(ImgClip, 0);
            Grid.SetColumn(ImgClip2, 1);
         

            DynamicItemContainer.Children.Add(ImageGrid);
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();

        }
        private void OpacityImage(object sender, MouseButtonEventArgs e)
        {
            DynamicItemContainer.Children.Clear();

            Image ImgOpac = new Image();
            ImgOpac.Source = new BitmapImage(new Uri("image/silverlight.png", UriKind.Relative));
            ImgOpac.SetValue(Canvas.TopProperty, 18.0);
            ImgOpac.SetValue(Canvas.LeftProperty, 80.0);

            RadialGradientBrush rgb = new RadialGradientBrush();
            rgb.GradientOrigin = new Point(.5,.5);
            rgb.Center = new Point(.5,.5);
            rgb.RadiusX=.5;
            rgb.RadiusY=.5;

            GradientStop gs1 =new GradientStop();
            gs1.Color = getColorFromHex("ffffffff");
            gs1.Offset= .5;

            GradientStop gs2 =new GradientStop();
            gs2.Color = getColorFromHex("00ffffff");
            gs2.Offset= 1;

            rgb.GradientStops.Add(gs1);
            rgb.GradientStops.Add(gs2);

            ImgOpac.OpacityMask = rgb;

            DynamicItemContainer.Children.Add(ImgOpac);
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();

        }
        public Color getColorFromHex(string s)
        {
            byte a = System.Convert.ToByte(s.Substring(0, 2), 16);
            byte r = System.Convert.ToByte(s.Substring(2, 2), 16);
            byte g = System.Convert.ToByte(s.Substring(4, 2), 16);
            byte b = System.Convert.ToByte(s.Substring(6, 2), 16);
            return Color.FromArgb(a, r, g, b);
        }
        private void ShadowImage(object sender, MouseButtonEventArgs e)
        {

            DynamicItemContainer.Children.Clear();
            
            Image ImgShadow = new Image();
            ImgShadow.Source = new BitmapImage(new Uri("image/silverlight.png", UriKind.Relative));
            ImgShadow.SetValue(Canvas.TopProperty, 18.0);
            ImgShadow.SetValue(Canvas.LeftProperty, 80.0);

            Canvas cShadow = new Canvas();
            cShadow.Background = new SolidColorBrush(Colors.DarkGray);
            cShadow.Width = 160;
            cShadow.Height = 160;
            cShadow.SetValue(Canvas.TopProperty, 18.0);
            cShadow.SetValue(Canvas.LeftProperty, 80.0);

            TranslateTransform tt =new TranslateTransform();
            tt.X = 4;
            tt.Y = 4;
            cShadow.RenderTransform = tt;

           
            DynamicItemContainer.Children.Add(cShadow);
            DynamicItemContainer.Children.Add(ImgShadow);


            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();

        }
        private void GlowImage(object sender, MouseButtonEventArgs e)
        {

            DynamicItemContainer.Children.Clear();

            Image ImgGlow = new Image();
            ImgGlow.Source = new BitmapImage(new Uri("image/silverlight.png", UriKind.Relative));
            ImgGlow.SetValue(Canvas.TopProperty, 18.0);
            ImgGlow.SetValue(Canvas.LeftProperty, 80.0);

            Border bGlow = new Border();

            bGlow.Width = 168;
            bGlow.Height = 168;
            bGlow.SetValue(Canvas.TopProperty, 14.0);
            bGlow.SetValue(Canvas.LeftProperty, 76.0);
            bGlow.BorderThickness = new Thickness(4);

            
            LinearGradientBrush lgb = new LinearGradientBrush();
            lgb.StartPoint = new Point(0, 0);
            lgb.EndPoint = new Point(1, 1);

            GradientStop gs1 = new GradientStop();
            gs1.Color = Colors.Yellow;
            gs1.Offset = 0.0;

            GradientStop gs2 = new GradientStop();
            gs2.Color = Colors.Orange;
            gs2.Offset = 0.25;

            GradientStop gs3 = new GradientStop();
            gs3.Color = Colors.Orange;
            gs3.Offset = 0.75;

            GradientStop gs4 = new GradientStop();
            gs4.Color = Colors.Yellow;
            gs4.Offset = 1.0;

            lgb.GradientStops.Add(gs1);
            lgb.GradientStops.Add(gs2);
            lgb.GradientStops.Add(gs3);
            lgb.GradientStops.Add(gs4);

            bGlow.BorderBrush = lgb;
            bGlow.Child = ImgGlow;

            DynamicItemContainer.Children.Add(bGlow);

            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();

        }
        private void ReflectImage(object sender, MouseButtonEventArgs e)
        {
            DynamicItemContainer.Children.Clear();

            Image ImgMain = new Image();
            ImgMain.Source = new BitmapImage(new Uri("image/silverlight.png", UriKind.Relative));
            ImgMain.SetValue(Canvas.TopProperty, 20.0);
            ImgMain.SetValue(Canvas.LeftProperty, 60.0);
            ImgMain.Width =80;
            ImgMain.Height = 80;

            Image ImgReflection = new Image();
            ImgReflection.Source = new BitmapImage(new Uri("image/silverlight.png", UriKind.Relative));
            ImgReflection.SetValue(Canvas.TopProperty, 180.0);
            ImgReflection.SetValue(Canvas.LeftProperty, 60.0);
            ImgReflection.Width = 80;
            ImgReflection.Height = 80;

            Image ImgMain2 = new Image();
            ImgMain2.Source = new BitmapImage(new Uri("image/silverlight.png", UriKind.Relative));
            ImgMain2.SetValue(Canvas.TopProperty, 20.0);
            ImgMain2.SetValue(Canvas.LeftProperty, 180.0);
            ImgMain2.Width = 80;
            ImgMain2.Height = 80;

            Image ImgReflection2 = new Image();
            ImgReflection2.Source = new BitmapImage(new Uri("image/silverlight.png", UriKind.Relative));
            ImgReflection2.SetValue(Canvas.TopProperty, 180.0);
            ImgReflection2.SetValue(Canvas.LeftProperty, 180.0);
            ImgReflection2.Width = 80;
            ImgReflection2.Height = 80;
            
            
            ScaleTransform st = new ScaleTransform();
            st.ScaleY  = -1;
            ImgReflection.RenderTransform = st;

            ScaleTransform st2 = new ScaleTransform();
            st2.ScaleY = -1;
            ImgReflection2.RenderTransform = st;


            LinearGradientBrush lgb = new LinearGradientBrush();
            lgb.StartPoint = new Point(.5, 0);
            lgb.EndPoint = new Point(.5, 2);

            GradientStop gs1 = new GradientStop();
            gs1.Color = getColorFromHex("00000000");
            gs1.Offset = 0.0;

            GradientStop gs2 = new GradientStop();
            gs2.Color = getColorFromHex("FFFFFFFF");
            gs2.Offset = 1.0;

            lgb.GradientStops.Add(gs1);
            lgb.GradientStops.Add(gs2);

            LinearGradientBrush lgb2 = new LinearGradientBrush();
            lgb2.StartPoint = new Point(.5, 2);
            lgb2.EndPoint = new Point(.5, 0);

            GradientStop gs21 = new GradientStop();
            gs21.Color = getColorFromHex("ffffffff");
            gs21.Offset = .5;

            GradientStop gs22 = new GradientStop();
            gs22.Color = getColorFromHex("00000000");
            gs22.Offset = 1;


            lgb2.GradientStops.Add(gs21);
            lgb2.GradientStops.Add(gs22);

            ImgReflection.OpacityMask = lgb;
            ImgReflection2.OpacityMask = lgb2;
          

            DynamicItemContainer.Children.Add(ImgMain);
            DynamicItemContainer.Children.Add(ImgReflection);

            DynamicItemContainer.Children.Add(ImgMain2);
            DynamicItemContainer.Children.Add(ImgReflection2);

            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();

        }

    }
}
