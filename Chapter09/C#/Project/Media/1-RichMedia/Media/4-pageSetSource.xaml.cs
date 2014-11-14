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

namespace Media
{
    public partial class pageSetSource : UserControl
    {
        private Storyboard myStoryboard = new Storyboard();
        public pageSetSource()
        {
            InitializeComponent();
        }
        private void CollapseAll()
        {
           Media.Stop();
        }
        private void ShowMedia(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
            SetEmbeddedMEdia();
        }

        private void SetEmbeddedMEdia()
        {
            Media.Source = new Uri("Butterfly.wmv", UriKind.Relative);
            Media.Play();
        }
        private void SetLocalFile(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "All files (*.*)|*.*|Windows Media Video (*.wmv)|*.wmv|MPEG (*.mp4)|*.mp4";
            if (openDialog.ShowDialog().Value)
            {
                Media.SetSource(openDialog.File.OpenRead());
                Media.Play();
            }
        }
    }
}
