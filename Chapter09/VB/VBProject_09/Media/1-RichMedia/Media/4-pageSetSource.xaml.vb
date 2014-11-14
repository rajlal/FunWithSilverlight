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

    Public Partial Class pageSetSource
        Inherits UserControl

    Private myStoryboard As Storyboard = New Storyboard()

        Public Sub New()
            InitializeComponent()
        End Sub '   New

        private Sub CollapseAll()

           Media.Stop()
        End Sub '   CollapseAll

        private Sub ShowMedia(sender As Object, e As MouseButtonEventArgs)

            CollapseAll()
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
            SetEmbeddedMEdia()
        End Sub '   ShowMedia


        private Sub SetEmbeddedMEdia()

            Media.Source = New Uri("Butterfly.wmv", UriKind.Relative)
            Media.Play()
        End Sub '   SetEmbeddedMEdia

        private Sub SetLocalFile(sender As Object, e As MouseButtonEventArgs)

        Dim openDialog As OpenFileDialog = New OpenFileDialog()
            openDialog.Filter = "All files (*.*)|*.*|Windows Media Video (*.wmv)|*.wmv|MPEG (*.mp4)|*.mp4"

            If (openDialog.ShowDialog().Value) Then
                Media.SetSource(openDialog.File.OpenRead())
                Media.Play()
            End If
        End Sub '   SetLocalFile
    End Class   '   pageSetSource
' End Namespace   '   Media
' ..\Project_09\Media\1-RichMedia\Media\4-pageSetSource.xaml.cs
