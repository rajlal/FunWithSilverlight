Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports System.Windows.Media.Imaging

' Namespace ImageManipulation

    Public Partial class PageImage
        Inherits UserControl

        Public Sub New()

            InitializeComponent()
        End Sub '   New


    Private Sub ReadImage(sender As Object, e As MouseButtonEventArgs)


        Dim Img As Image = New Image()
        Img.Source = New BitmapImage(New Uri("image/silverlight.png", UriKind.Relative))
        Img.SetValue(Canvas.TopProperty, 18.0)
        Img.SetValue(Canvas.LeftProperty, 80.0)

        DynamicItemContainer.Children.Clear()
        DynamicItemContainer.Children.Add(Img)
        StatusBar.Text = ToolTipService.GetToolTip(ImageDefault).ToString()
    End Sub '   ReadImage

    Private Sub StretchImage(sender As Object, e As MouseButtonEventArgs)

        DynamicItemContainer.Children.Clear()

        Dim ImageGrid As Grid = New Grid()

        ImageGrid.Width = 320
        ImageGrid.Height = 200

        ImageGrid.ColumnDefinitions.Add(New ColumnDefinition())
        ImageGrid.ColumnDefinitions.Add(New ColumnDefinition())
        ImageGrid.ColumnDefinitions.Add(New ColumnDefinition())
        ImageGrid.ColumnDefinitions.Add(New ColumnDefinition())

        Dim r1 As RowDefinition = New RowDefinition()
        r1.Height = New GridLength(140)
        Dim r2 As RowDefinition = New RowDefinition()
        r2.Height = New GridLength(60)

        ImageGrid.RowDefinitions.Add(r1)
        ImageGrid.RowDefinitions.Add(r2)

        Dim ImgNone As Image = New Image()
        ImgNone.Source = New BitmapImage(New Uri("image/silverlight.png", UriKind.Relative))
        ImgNone.Stretch = Stretch.None
        ImgNone.Margin = New Thickness(2)

        Dim ImgFill As Image = New Image()
        ImgFill.Source = New BitmapImage(New Uri("image/silverlight.png", UriKind.Relative))
        ImgFill.Stretch = Stretch.Fill
        ImgFill.Margin = New Thickness(2)


        Dim ImgUniform As Image = New Image()
        ImgUniform.Source = New BitmapImage(New Uri("image/silverlight.png", UriKind.Relative))
        ImgUniform.Stretch = Stretch.Uniform
        ImgUniform.Margin = New Thickness(2)


        Dim ImgUniformToFill As Image = New Image()
        ImgUniformToFill.Source = New BitmapImage(New Uri("image/silverlight.png", UriKind.Relative))
        ImgUniformToFill.Stretch = Stretch.UniformToFill
        ImgUniformToFill.Margin = New Thickness(2)


        ImageGrid.Children.Add(ImgNone)
        ImageGrid.Children.Add(ImgFill)
        ImageGrid.Children.Add(ImgUniform)
        ImageGrid.Children.Add(ImgUniformToFill)

        Dim TextNone As TextBlock = New TextBlock()
        TextNone.Text = " Stretch:None\n Size: same\n Cropped"

        Dim TextFill As TextBlock = New TextBlock()
        TextFill.Text = " :Fill\n :Resize\n --"

        Dim TextUniform As TextBlock = New TextBlock()
        TextUniform.Text = " :Uniform\n :Resize\n --"

        Dim TextUniformFill As TextBlock = New TextBlock()
        TextUniformFill.Text = " :UniformFill\n :Resize\n Cropped"

        ImageGrid.Children.Add(TextNone)
        ImageGrid.Children.Add(TextFill)
        ImageGrid.Children.Add(TextUniform)
        ImageGrid.Children.Add(TextUniformFill)

        Grid.SetColumn(TextNone, 0)
        Grid.SetColumn(TextFill, 1)
        Grid.SetColumn(TextUniform, 2)
        Grid.SetColumn(TextUniformFill, 3)
        Grid.SetRow(TextNone, 1)
        Grid.SetRow(TextFill, 1)
        Grid.SetRow(TextUniform, 1)
        Grid.SetRow(TextUniformFill, 1)

        Grid.SetColumn(ImgNone, 0)
        Grid.SetColumn(ImgFill, 1)
        Grid.SetColumn(ImgUniform, 2)
        Grid.SetColumn(ImgUniformToFill, 3)


        DynamicItemContainer.Children.Add(ImageGrid)

        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()



    End Sub '   StretchImage

    Private Sub ClipImage(sender As Object, e As MouseButtonEventArgs)

        DynamicItemContainer.Children.Clear()
        Dim ImageGrid As Grid = New Grid()
        ImageGrid.Width = 320
        ImageGrid.Height = 200

        ImageGrid.ColumnDefinitions.Add(New ColumnDefinition())
        ImageGrid.ColumnDefinitions.Add(New ColumnDefinition())

        Dim r1 As RowDefinition = New RowDefinition()
        r1.Height = New GridLength(160)
        Dim r2 As RowDefinition = New RowDefinition()
        r2.Height = New GridLength(40)

        ImageGrid.RowDefinitions.Add(r1)
        ImageGrid.RowDefinitions.Add(r2)

        Dim ImgClip As Image = New Image()
        ImgClip.Source = New BitmapImage(New Uri("image/silverlight.png", UriKind.Relative))
        ImgClip.Margin = New Thickness(2)
        Dim eg As EllipseGeometry = New EllipseGeometry()
        eg.RadiusX = 78
        eg.RadiusY = 72
        eg.Center = New Point(78, 78)
        ImgClip.Clip = eg


        Dim ImgClip2 As Image = New Image()
        ImgClip2.Source = New BitmapImage(New Uri("image/silverlight.png", UriKind.Relative))
        ImgClip2.Margin = New Thickness(2)

        Dim rg As RectangleGeometry = New RectangleGeometry()
        rg.Rect = New Rect(10, 10, 130, 140)
        rg.RadiusX = 25
        rg.RadiusY = 25

        ImgClip2.Clip = rg

        ImageGrid.Children.Add(ImgClip)
        ImageGrid.Children.Add(ImgClip2)

        Dim TextE As TextBlock = New TextBlock()
        TextE.Text = " Ellipse\n Geometry"

        Dim TextR As TextBlock = New TextBlock()
        TextR.Text = " Rectangle\n Geometry"

        ImageGrid.Children.Add(TextE)
        ImageGrid.Children.Add(TextR)

        Grid.SetColumn(TextE, 0)
        Grid.SetRow(TextE, 1)
        Grid.SetColumn(TextR, 1)
        Grid.SetRow(TextR, 1)

        Grid.SetColumn(ImgClip, 0)
        Grid.SetColumn(ImgClip2, 1)


        DynamicItemContainer.Children.Add(ImageGrid)
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()

    End Sub '   ClipImage

    Private Sub OpacityImage(sender As Object, e As MouseButtonEventArgs)

        DynamicItemContainer.Children.Clear()

        Dim ImgOpac As Image = New Image()
        ImgOpac.Source = New BitmapImage(New Uri("image/silverlight.png", UriKind.Relative))
        ImgOpac.SetValue(Canvas.TopProperty, 18.0)
        ImgOpac.SetValue(Canvas.LeftProperty, 80.0)

        Dim rgb As RadialGradientBrush = New RadialGradientBrush()
        rgb.GradientOrigin = New Point(0.5, 0.5)
        rgb.Center = New Point(0.5, 0.5)
        rgb.RadiusX = 0.5
        rgb.RadiusY = 0.5

        Dim gs1 As GradientStop = New GradientStop()
        gs1.Color = getColorFromHex("ffffffff")
        gs1.Offset = 0.5

        Dim gs2 As GradientStop = New GradientStop()
        gs2.Color = getColorFromHex("00ffffff")
        gs2.Offset = 1

        rgb.GradientStops.Add(gs1)
        rgb.GradientStops.Add(gs2)

        ImgOpac.OpacityMask = rgb

        DynamicItemContainer.Children.Add(ImgOpac)
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()

    End Sub '   OpacityImage

    Public Function getColorFromHex(s As String) As Color

        Dim a As Byte = System.Convert.ToByte(s.Substring(0, 2), 16)
        Dim r As Byte = System.Convert.ToByte(s.Substring(2, 2), 16)
        Dim g As Byte = System.Convert.ToByte(s.Substring(4, 2), 16)
        Dim b As Byte = System.Convert.ToByte(s.Substring(6, 2), 16)
        Return Color.FromArgb(a, r, g, b)
    End Function '   ShadowImage

    Private Sub ShadowImage(sender As Object, e As MouseButtonEventArgs)


        DynamicItemContainer.Children.Clear()

        Dim ImgShadow As Image = New Image()
        ImgShadow.Source = New BitmapImage(New Uri("image/silverlight.png", UriKind.Relative))
        ImgShadow.SetValue(Canvas.TopProperty, 18.0)
        ImgShadow.SetValue(Canvas.LeftProperty, 80.0)

        Dim cShadow As Canvas = New Canvas()
        cShadow.Background = New SolidColorBrush(Colors.DarkGray)
        cShadow.Width = 160
        cShadow.Height = 160
        cShadow.SetValue(Canvas.TopProperty, 18.0)
        cShadow.SetValue(Canvas.LeftProperty, 80.0)

        Dim tt As TranslateTransform = New TranslateTransform()
        tt.X = 4
        tt.Y = 4
        cShadow.RenderTransform = tt


        DynamicItemContainer.Children.Add(cShadow)
        DynamicItemContainer.Children.Add(ImgShadow)


        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()

    End Sub '   GlowImage

    Private Sub GlowImage(sender As Object, e As MouseButtonEventArgs)


        DynamicItemContainer.Children.Clear()

        Dim ImgGlow As Image = New Image()
        ImgGlow.Source = New BitmapImage(New Uri("image/silverlight.png", UriKind.Relative))
        ImgGlow.SetValue(Canvas.TopProperty, 18.0)
        ImgGlow.SetValue(Canvas.LeftProperty, 80.0)

        Dim bGlow As Border = New Border()

        bGlow.Width = 168
        bGlow.Height = 168
        bGlow.SetValue(Canvas.TopProperty, 14.0)
        bGlow.SetValue(Canvas.LeftProperty, 76.0)
        bGlow.BorderThickness = New Thickness(4)


        Dim lgb As LinearGradientBrush = New LinearGradientBrush()
        lgb.StartPoint = New Point(0, 0)
        lgb.EndPoint = New Point(1, 1)

        Dim gs1 As GradientStop = New GradientStop()
        gs1.Color = Colors.Yellow
        gs1.Offset = 0.0

        Dim gs2 As GradientStop = New GradientStop()
        gs2.Color = Colors.Orange
        gs2.Offset = 0.25

        Dim gs3 As GradientStop = New GradientStop()
        gs3.Color = Colors.Orange
        gs3.Offset = 0.75

        Dim gs4 As GradientStop = New GradientStop()
        gs4.Color = Colors.Yellow
        gs4.Offset = 1.0

        lgb.GradientStops.Add(gs1)
        lgb.GradientStops.Add(gs2)
        lgb.GradientStops.Add(gs3)
        lgb.GradientStops.Add(gs4)

        bGlow.BorderBrush = lgb
        bGlow.Child = ImgGlow

        DynamicItemContainer.Children.Add(bGlow)

        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()

    End Sub '   ReflectImage

    Private Sub ReflectImage(sender As Object, e As MouseButtonEventArgs)

        DynamicItemContainer.Children.Clear()

        Dim ImgMain As Image = New Image()
        ImgMain.Source = New BitmapImage(New Uri("image/silverlight.png", UriKind.Relative))
        ImgMain.SetValue(Canvas.TopProperty, 20.0)
        ImgMain.SetValue(Canvas.LeftProperty, 60.0)
        ImgMain.Width = 80
        ImgMain.Height = 80

        Dim ImgReflection As Image = New Image()
        ImgReflection.Source = New BitmapImage(New Uri("image/silverlight.png", UriKind.Relative))
        ImgReflection.SetValue(Canvas.TopProperty, 180.0)
        ImgReflection.SetValue(Canvas.LeftProperty, 60.0)
        ImgReflection.Width = 80
        ImgReflection.Height = 80

        Dim ImgMain2 As Image = New Image()
        ImgMain2.Source = New BitmapImage(New Uri("image/silverlight.png", UriKind.Relative))
        ImgMain2.SetValue(Canvas.TopProperty, 20.0)
        ImgMain2.SetValue(Canvas.LeftProperty, 180.0)
        ImgMain2.Width = 80
        ImgMain2.Height = 80

        Dim ImgReflection2 As Image = New Image()
        ImgReflection2.Source = New BitmapImage(New Uri("image/silverlight.png", UriKind.Relative))
        ImgReflection2.SetValue(Canvas.TopProperty, 180.0)
        ImgReflection2.SetValue(Canvas.LeftProperty, 180.0)
        ImgReflection2.Width = 80
        ImgReflection2.Height = 80


        Dim st As ScaleTransform = New ScaleTransform()
        st.ScaleY = -1
        ImgReflection.RenderTransform = st

        Dim st2 As ScaleTransform = New ScaleTransform()
        st2.ScaleY = -1
        ImgReflection2.RenderTransform = st


        Dim lgb As LinearGradientBrush = New LinearGradientBrush()
        lgb.StartPoint = New Point(0.5, 0)
        lgb.EndPoint = New Point(0.5, 2)

        Dim gs1 As GradientStop = New GradientStop()
        gs1.Color = getColorFromHex("00000000")
        gs1.Offset = 0.0

        Dim gs2 As GradientStop = New GradientStop()
        gs2.Color = getColorFromHex("FFFFFFFF")
        gs2.Offset = 1.0

        lgb.GradientStops.Add(gs1)
        lgb.GradientStops.Add(gs2)

        Dim lgb2 As LinearGradientBrush = New LinearGradientBrush()
        lgb2.StartPoint = New Point(0.5, 2)
        lgb2.EndPoint = New Point(0.5, 0)

        Dim gs21 As GradientStop = New GradientStop()
        gs21.Color = getColorFromHex("ffffffff")
        gs21.Offset = 0.5

        Dim gs22 As GradientStop = New GradientStop()
        gs22.Color = getColorFromHex("00000000")
        gs22.Offset = 1


        lgb2.GradientStops.Add(gs21)
        lgb2.GradientStops.Add(gs22)

        ImgReflection.OpacityMask = lgb
        ImgReflection2.OpacityMask = lgb2


        DynamicItemContainer.Children.Add(ImgMain)
        DynamicItemContainer.Children.Add(ImgReflection)

        DynamicItemContainer.Children.Add(ImgMain2)
        DynamicItemContainer.Children.Add(ImgReflection2)

        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()

    End Sub  '   getColorFromHex


    End Class   '   PageImage

' End Namespace 
' ..\Graphics\G\Graphics\Graphics\5-ImageManipulation\PageImage.xaml.cs
