'  <copyright file="Page.xaml.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Describes the built module</summary>
'  <author>Microsoft Expression Encoder Team</author>
' Namespace MediaPlayerTemplate

Imports    using System
    using System.Collections.Generic
    using System.Net
    using System.Windows
    using System.Windows.Browser
    using System.Windows.Controls
    using System.Windows.Documents
    using System.Windows.Input
    using System.Windows.Media
    using System.Windows.Media.Animation
    using System.Windows.Shapes
    using ExpressionMediaPlayer

    Public Partial Class Page
        Inherits UserControl

        Public Sub New(sender As Object, e As StartupEventArgs)
            InitializeComponent()
            myPlayer.OnStartup(sender, e)
        End Sub '   New


        private Sub Image_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://silverlightfun.com"),"_blank")
        End Sub '   Image_MouseLeftButtonUp
    End Class   '   Page
' End Namespace   '   MediaPlayerTemplate
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\CustomMediaPlayer\Source\Page.xaml.cs
