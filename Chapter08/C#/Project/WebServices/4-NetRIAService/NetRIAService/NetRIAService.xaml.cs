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
using NetRIAService.Web;
using System.Windows.Ria.Data;
using System.Windows.Media.Imaging;

namespace NetRIAService
{
    public partial class NetRIAService : UserControl
    {
        private ResourceContext _resourceContext = new ResourceContext();
        public NetRIAService()
        {
            InitializeComponent();
            LoadOperation<Resource> loadop = this._resourceContext.Load(this._resourceContext.GetResourceQuery());
            this.dataGrid.ItemsSource = loadop.Entities;
        }
        private void ResourceImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Image i = (Image)sender;
            i.Source = new BitmapImage(new Uri("images/error.png", UriKind.Relative));
            ToolTip t = new ToolTip();
            t.Content = new TextBlock()
            {
                FontFamily = new FontFamily("Arial"),
                FontSize = 12,
                Text = "Error retrieving Image",
                TextWrapping = TextWrapping.Wrap
            };
            ToolTipService.SetToolTip(i, t);
        }
    }
}
