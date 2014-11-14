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
using System.Windows.Interop;

namespace SilverlightPlugIn
{
    public partial class PageAccessPlugin : UserControl
    {
        public PageAccessPlugin()
        {
            InitializeComponent();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            SilverlightHost host = Application.Current.Host;

            // Read-only properties of the Host object.
            Color pluginBackground = host.Background;
            Uri source = host.Source;

            txtValues.Text = "Background: " + pluginBackground.ToString() + "\n";
            txtValues.Text += "Source: " + source.ToString().Substring(source.ToString().LastIndexOf('/')+1) + "\n";

           
            // The Settings object, which represents Web browser settings.
            Settings settings = host.Settings;

            txtValues.Text += "EnableFrameRateCounter: " + settings.EnableFrameRateCounter.ToString() + "\n";
            txtValues.Text += "EnableRedrawRegions: " + settings.EnableRedrawRegions.ToString() + "\n";
            txtValues.Text += "MaxFrameRate: " + settings.MaxFrameRate.ToString() + "\n";
            
            // Read-only properties of the Settings object.
            bool windowless = settings.Windowless;
            bool htmlAccessEnabled = settings.EnableHTMLAccess;
            txtValues.Text += "Windowless Property: " + windowless.ToString() + "\n";
            txtValues.Text += "HtmlAccessEnabled Property: " + htmlAccessEnabled.ToString() + "\n";

            // The Content object, which represents the plug-in display area.
            Content content = host.Content;

            // The read/write IsFullScreen property of the Content object.
            // See also the Content.FullScreenChanged event.
            bool isFullScreen = content.IsFullScreen;
            txtValues.Text += "IsFullScreen Property: " + isFullScreen.ToString() + "\n";
        }
    }
}
