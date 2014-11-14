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

' Namespace Media

    Public Partial Class pageVideoBrush
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
            CollapseAll()
            FillRectangle()
        End Sub '   New

        private Sub CollapseAll()

            media.Stop()
            mediaReflect.Stop()
            mediaText.Stop()
            mediaRotate.Stop()
            mediaRotateBG.Stop()
            canvasTextRotate.Visibility = Visibility.Collapsed
            canvasTextBlock.Visibility = Visibility.Collapsed
            canvasMedia.Visibility = Visibility.Collapsed
                 canvasReflection.Visibility = Visibility.Collapsed
        End Sub '   CollapseAll

        private Sub ShowMedia(sender As Object, e As MouseButtonEventArgs)

            CollapseAll()
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
            FillRectangle()
        End Sub '   ShowMedia


        private Sub FillRectangle()

            canvasMedia.Visibility = Visibility.Visible

            Dim vb As VideoBrush  = New VideoBrush()

            vb.SourceName = "media"
            vb.Stretch = Stretch.UniformToFill
            myRectangle.Fill = vb
            media.Play()
        End Sub '   FillRectangle

        private Sub ShowReflection(sender As Object, e As MouseButtonEventArgs)

            CollapseAll()
            canvasReflection.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
            mediaReflect.Play()
        End Sub '   ShowReflection


        private Sub ShowText(sender As Object, e As MouseButtonEventArgs)

            CollapseAll()
            canvasTextBlock.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
            mediaText.Play()
        End Sub '   ShowText


        private Sub ShowTextRotate(sender As Object, e As MouseButtonEventArgs)

            CollapseAll()
            canvasTextRotate.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
            mediaRotate.Play()
            mediaRotateBG.Play()
        End Sub '   ShowTextRotate
    End Class   '   pageVideoBrush
' End Namespace   '   Media
' ..\Project_09\Media\1-RichMedia\Media\3-pageVideoBrush.xaml.cs
