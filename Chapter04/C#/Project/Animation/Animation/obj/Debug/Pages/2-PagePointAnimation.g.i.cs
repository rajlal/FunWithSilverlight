﻿#pragma checksum "D:\Silverlight 4\04-Final\062904\Project\Animation\Animation\Pages\2-PagePointAnimation.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2FC5C1CD567D4F286A7304CE8C8ABE47"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Animation {
    
    
    public partial class PagePointAnimation : System.Windows.Controls.UserControl {
        
        internal System.Windows.Media.Animation.Storyboard myBasicStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard myEclipseStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard mySunriseStoryboard;
        
        internal System.Windows.Controls.Canvas CanvasBasic;
        
        internal System.Windows.Media.EllipseGeometry ufobody;
        
        internal System.Windows.Media.EllipseGeometry uforing;
        
        internal System.Windows.Controls.Canvas CanvasEclipse;
        
        internal System.Windows.Media.EllipseGeometry SunEclipse;
        
        internal System.Windows.Controls.Image EclipseGlow;
        
        internal System.Windows.Media.EllipseGeometry MoonEclipse;
        
        internal System.Windows.Controls.Canvas CanvasSunrise;
        
        internal System.Windows.Media.RadialGradientBrush SunGlow;
        
        internal System.Windows.Media.EllipseGeometry Sun;
        
        internal System.Windows.Controls.Image btnPlay;
        
        internal System.Windows.Controls.Image btnPause;
        
        internal System.Windows.Controls.Image btnStop;
        
        internal System.Windows.Controls.Grid StatusGrid;
        
        internal System.Windows.Controls.TextBlock StatusBar;
        
        internal System.Windows.Controls.TextBlock StatusThickness;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Animation;component/Pages/2-PagePointAnimation.xaml", System.UriKind.Relative));
            this.myBasicStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("myBasicStoryboard")));
            this.myEclipseStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("myEclipseStoryboard")));
            this.mySunriseStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("mySunriseStoryboard")));
            this.CanvasBasic = ((System.Windows.Controls.Canvas)(this.FindName("CanvasBasic")));
            this.ufobody = ((System.Windows.Media.EllipseGeometry)(this.FindName("ufobody")));
            this.uforing = ((System.Windows.Media.EllipseGeometry)(this.FindName("uforing")));
            this.CanvasEclipse = ((System.Windows.Controls.Canvas)(this.FindName("CanvasEclipse")));
            this.SunEclipse = ((System.Windows.Media.EllipseGeometry)(this.FindName("SunEclipse")));
            this.EclipseGlow = ((System.Windows.Controls.Image)(this.FindName("EclipseGlow")));
            this.MoonEclipse = ((System.Windows.Media.EllipseGeometry)(this.FindName("MoonEclipse")));
            this.CanvasSunrise = ((System.Windows.Controls.Canvas)(this.FindName("CanvasSunrise")));
            this.SunGlow = ((System.Windows.Media.RadialGradientBrush)(this.FindName("SunGlow")));
            this.Sun = ((System.Windows.Media.EllipseGeometry)(this.FindName("Sun")));
            this.btnPlay = ((System.Windows.Controls.Image)(this.FindName("btnPlay")));
            this.btnPause = ((System.Windows.Controls.Image)(this.FindName("btnPause")));
            this.btnStop = ((System.Windows.Controls.Image)(this.FindName("btnStop")));
            this.StatusGrid = ((System.Windows.Controls.Grid)(this.FindName("StatusGrid")));
            this.StatusBar = ((System.Windows.Controls.TextBlock)(this.FindName("StatusBar")));
            this.StatusThickness = ((System.Windows.Controls.TextBlock)(this.FindName("StatusThickness")));
        }
    }
}

