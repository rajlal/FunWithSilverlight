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
Imports System.IO
Imports System.Windows.Browser
Imports System.Windows.Media.Imaging

' Namespace LocalStorage

    Public Partial Class PageOpenFileDialog
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub '   New


        private Sub OpenFileDialog()

            lblFilename.Text = ""
            videoContainer.Stop()
            videoContainer.Visibility = Visibility.Collapsed
            textContainer.Visibility = Visibility.Collapsed
            imageContainer.Visibility = Visibility.Collapsed

            Dim dialog As OpenFileDialog  = New OpenFileDialog()

                dialog.Multiselect = false
                dialog.Filter = "All files (*.*)|*.*|Text files (txt)|*.txt|XAML files (xaml)|*.xaml|Image files (jpg, png)|*.jpg;*.png|Windows Media Video (wmv)|*.wmv"
                dialog.FilterIndex = 1

                If (dialog.ShowDialog().Value) Then
            Dim info As FileInfo = dialog.File
                    Me.lblFilename.Text = "Filename: " + info.Name.ToString()
                    Try

                        Select Case (dialog.File.Extension.ToLower() )
                    Case ".txt"
                        SetText(info.OpenText())
                    Case ".xaml"
                        SetText(info.OpenText())
                    Case ".jpg"
                        SetImage(info.OpenRead())
                    Case ".jpeg"
                        SetImage(info.OpenRead())
                    Case ".png"
                        SetImage(info.OpenRead())
                    Case ".wmv"
                        SetVideo(info.OpenRead())
                    Case ".wav"
                        SetVideo(info.OpenRead())
                    Case Else
                        SetText(info.OpenText())
                End Select '    dialog.File.Extension.ToLower()
            Catch ex As Exception
                HtmlPage.Window.Alert("Cancelled")
                    End Try
                End If
        End Sub '   OpenFileDialog

        private Sub SetText(txtS As StreamReader)

            Me.textContainer.Text = txtS.ReadToEnd()
            txtS.Close()
            Me.textContainer.Visibility = Visibility.Visible
        End Sub '   SetText

        private Sub SetImage(imgS As Stream)

            Dim image As BitmapImage  = New BitmapImage()

            image.SetSource(imgS)
            Me.imageContainer.Source = image
            imgS.Close()
            Me.imageContainer.Visibility = Visibility.Visible
        End Sub '   SetImage

        private Sub SetVideo(vidS As Stream)

            Me.videoContainer.SetSource(vidS)
            Me.videoContainer.Play()
            Me.videoContainer.Visibility = Visibility.Visible
        End Sub '   SetVideo

        private Sub Button_Click(sender As Object, e As RoutedEventArgs)

            OpenFileDialog()
        End Sub '   Button_Click
    End Class   '   PageOpenFileDialog
' End Namespace   '   LocalStorage
' ..\Project_05\ExtendBrowser\3-IsolatedStorage\Pages\1-PageOpenFileDialog.xaml.cs
