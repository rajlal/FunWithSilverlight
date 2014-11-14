'  <copyright file="App.xaml.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Describes the built module</summary>
'  <author>Microsoft Expression Encoder Team</author>
' Namespace MediaPlayerTemplate

Imports    using System
    using System.Collections.Generic
    using System.Collections.ObjectModel
    using System.IO
    using System.Net
    using System.Windows
    using System.Windows.Browser
    using System.Windows.Controls
    using System.Windows.Documents
    using System.Windows.Input
    using System.Windows.Media
    using System.Windows.Media.Animation
    using System.Windows.Media.Imaging
    using System.Windows.Shapes

    Public Partial Class App
        Inherits Application
        Public Sub New()

            InitializeComponent()
        End Sub '   New


        private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup

            Me.RootVisual = New Page(sender, e)
        End Sub '   Application_Startup
    End Class   '   App
' End Namespace   '   MediaPlayerTemplate
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\CustomMediaPlayer\Source\App.xaml.cs
