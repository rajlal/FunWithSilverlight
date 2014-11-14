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

namespace ReusableControl
{
    public partial class Slideshow : UserControl
    {
        public Slideshow()
        {
            InitializeComponent();
        }


    }

    public class SlideImages
    {
        public SlideImages() { }

        public List<SlideImage> SlideImageList { get; set; }
    }
    public class SlideImage
    {
        public string ImageUri { get; set; }
        public string Title { get; set; }
    }
}