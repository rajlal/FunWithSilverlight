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

' Namespace ReusableControl

    Public Partial class Slideshow
        Inherits UserControl

        Public Sub New()

            InitializeComponent()
        End Sub '   New
    End Class   '   Slideshow

    Public class SlideImages

        Public Sub New() 
        End Sub '   New

        public List<SlideImage> SlideImageList { get; set; }
    End Class   '   SlideImages
    Public class SlideImage

    Public Property ImageUri As String
    Public Property Title As String
    End Class   '   SlideImage
' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\2-ReusableControl\Slideshowold.xaml.cs
