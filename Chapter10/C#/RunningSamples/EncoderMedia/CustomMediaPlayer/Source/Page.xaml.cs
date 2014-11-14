// <copyright file="Page.xaml.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Describes the built module</summary>
// <author>Microsoft Expression Encoder Team</author>
namespace MediaPlayerTemplate
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Windows;
    using System.Windows.Browser;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using ExpressionMediaPlayer;

    public partial class Page : UserControl
    {

        public Page(object sender, StartupEventArgs e)
        {
            InitializeComponent();
            myPlayer.OnStartup(sender, e);
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://silverlightfun.com"),"_blank");
        }
    }
}