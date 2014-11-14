'  <copyright file="ThumbnailImage.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the ThumbnailImage class</summary>
'  <author>Microsoft Expression Encoder Team</author>
Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Shapes
Imports Microsoft.VisualBasic

' Namespace ExpressionMediaPlayer

    ''' <summary>
    '''  Similar function as the Image control -- but handles UriFormatException cases by leaving image blank instead of thowing the exception higher.
    ''' </summary>
    Public Class ThumbnailImage
        Inherits ContentControl
        ''' <summary>
        ''' Using a DependencyProperty as the backing store for SourceUrl.  This enables animation, styling, binding, etc...
        ''' </summary>
        Public Property readonly() As
            Get
                return CType(GetValue(SourceProperty, String))
            End Get

            Set
                SetValue(SourceProperty, value)
            End Set
        End Property
        ''' <summary>
        ''' Load image
        ''' </summary>
        public Sub LoadImage()

            Try

                If ( Not string.IsNullOrEmpty(Me.Source)) Then

                    Dim image As Image  = New Image()
                    Dim sourceEscaped As String  = Uri.EscapeUriString(Me.Source)
                    Dim thumbnailUri As Uri  = New Uri(sourceEscaped, UriKind.RelativeOrAbsolute)

                    Debug.WriteLine("ThumbnailImage:LoadImage:" + thumbnailUri.ToString())

                    if (MediaPlayer.IsOffline)
                    {
                        IsoUri isoThumbnailUri = MediaPlayer.MakeOfflineIsoUri(thumbnailUri)
                        if (isoThumbnailUri.IsoFileExists)
                        {

                            Dim bi As BitmapImage  = New BitmapImage()

                            bi.SetSource(isoThumbnailUri.Stream)
                            image.Source = bi
                        }
                    }
                    else
                    {

                        Dim bi As BitmapImage  = New BitmapImage(thumbnailUri)

                        image.Source = bi
                    }
                    image.ImageFailed += Me.OnImageFailed
                    Me.Content = image
                else

                    Dim emptyRect As Rectangle  = New Rectangle()

                    emptyRect.Width = 32
                    emptyRect.Height = 24
                    emptyRect.Fill = New SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00))
                    Me.Content = emptyRect
                End If


            Catch ex As UriFormatException

                '  leave thumbnail blank if image can't be loaded
                Dim emptyRect As Rectangle  = New Rectangle()

                Me.Content = emptyRect
            End Try
        End Sub '   LoadImage
        ''' <summary>
        ''' Property handler for the source dependency property.
        ''' </summary>
        ''' <param name="imageObject">Source dependency object.</param>
        ''' <param name="eventArgs">Event args.</param>
        End Property


            Dim thumbnailImage As ThumbnailImage  = CType(imageObject,  ThumbnailImage)


            If (thumbnailImage  IsNot Nothing  AndAlso  eventArgs.NewValue  IsNot Nothing  AndAlso  eventArgs.NewValue is string) Then
                ThumbnailDownloader.Enqueue(thumbnailImage)
            End If
        End Sub '   SourcePropChanged
        ''' <summary>
        ''' Event handler for the Image failed event from the image.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnImageFailed(sender As Object, e As ExceptionRoutedEventArgs)

            Dim strErrorMessage As String  = string.Empty


            If (Me.Source  IsNot Nothing) Then
                strErrorMessage += Me.Source.ToString() + ChrW(13) & ChrW(10)
            End If

            strErrorMessage += e.ErrorException.ToString() + ChrW(13) & ChrW(10)
            Debug.WriteLine("ThumbnailImage:OnImageFailed:" + strErrorMessage)

            Dim textBlock As TextBlock  = New TextBlock()

            textBlock.Text = strErrorMessage
            textBlock.TextWrapping = TextWrapping.Wrap

            Content = textBlock
        End Sub '   OnImageFailed
    End Class   '   ThumbnailImage
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\ThumbnailImage.cs
