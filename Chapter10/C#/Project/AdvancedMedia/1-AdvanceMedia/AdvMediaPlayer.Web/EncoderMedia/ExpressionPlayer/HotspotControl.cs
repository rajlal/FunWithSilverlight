/// <copyright file="HotspotControl.xaml.cs" company="Microsoft">
///     Copyright © Microsoft Corporation. All rights reserved.
/// </copyright>
/// <summary>Implements the Hotspot class which listens to various mouse 
/// events and fires associated animations</summary>
/// <author>Microsoft Expression Encoder Team</author>
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Diagnostics;
using System.Windows.Media;

namespace ExpressionMediaPlayer
{
    public partial class HotspotControl : Grid
    {
        /// <summary>
        /// hotspot has focus
        /// </summary>
        private bool m_hotspotHasFocus;
        /// <summary>
        /// timer for delay for exit animation
        /// </summary>
        private DispatcherTimer m_timerExitAnim;        
        /// <summary>
        /// Hotspot control, fires named animations on various mouse events
        /// </summary>
        public HotspotControl()
        {
            this.MouseEnter += new MouseEventHandler(OnMouseEnter);
            this.MouseLeave += new MouseEventHandler(OnMouseLeave);

            SetEvents(this);           
            
            m_timerExitAnim = new DispatcherTimer();
            m_timerExitAnim.Tick += new EventHandler(ExitAnimationTimer);
            Delay = 1.0;
        }
        /// <summary>
        /// when exit animation timer times out
        /// </summary>
        private void ExitAnimationTimer(object sender, EventArgs e)
        {
            m_timerExitAnim.Stop();
            if (this.MouseLeaveAnimation != null)
                this.MouseLeaveAnimation.Begin();            
        }
        /// <summary>
        /// timed delay before showing exit animation
        /// </summary>
        public double Delay { get; set; }
        /// <summary>
        /// Animation to fire when mouse enters hotspot
        /// </summary>
        public Storyboard MouseEnterAnimation { get; set; }
        /// <summary>
        /// Animation to fire when mouse leaves hotspot
        /// </summary>
        public Storyboard MouseLeaveAnimation { get; set; }
        /// <summary>
        /// handle mouse entering hotspot
        /// </summary>
        protected void OnMouseEnter(object sender,MouseEventArgs e)
        {
            FireEnterAnimation();            
        }
        /// <summary>
        /// handle mouse leaving hotspot
        /// </summary>        
        protected void OnMouseLeave(object sender, MouseEventArgs e)
        {
            FireLeaveAnimation();            
        }
        /// <summary>
        /// is this control a child of the hotspot area
        /// </summary>
        /// <param name="dependancyObject"></param>
        /// <returns></returns>
        private bool IsHotSpotOrChild(object dependancyObjectTest)
        {
            if (dependancyObjectTest!=null && dependancyObjectTest is DependencyObject)
            {
                DependencyObject dependancyObject = dependancyObjectTest as DependencyObject;                
                do
                {                    
                    if (dependancyObject == (DependencyObject)this)
                        return true;
                    dependancyObject = VisualTreeHelper.GetParent(dependancyObject);
                } while (dependancyObject != null);
            }
            return false;
        }
        /// <summary>
        /// handle focus entering hotspot control (or any child controls)
        /// </summary>        
        void HotspotControl_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!IsHotSpotOrChild(e.OriginalSource))
                return;
            if (m_hotspotHasFocus)
                return;            
            FireEnterAnimation();
            m_hotspotHasFocus = true;            
        }
        /// <summary>
        /// handle focus leaving hotspot control (or any child controls)
        /// </summary>
        void HotspotControl_LostFocus(object sender, RoutedEventArgs e)
        {            
            object objectNewFocus = FocusManager.GetFocusedElement();
            if (IsHotSpotOrChild(objectNewFocus))
                return;            
            FireLeaveAnimation();
            m_hotspotHasFocus = false;
        }
        /// <summary>
        /// set animation events for this element and all child controls
        /// </summary>
        /// <param name="elem"></param>
        private void SetEvents(UIElement elem)
        {
            elem.GotFocus += new RoutedEventHandler(HotspotControl_GotFocus);
            elem.LostFocus += new RoutedEventHandler(HotspotControl_LostFocus);
            if (elem is Panel)
            {
                foreach (UIElement elemChild in ((Panel)elem).Children)
                    SetEvents(elemChild);
            }
        }
        /// <summary>
        /// shows the hotspot enter animation if defined
        /// </summary>
        protected void FireEnterAnimation()
        {
            if (this.MouseEnterAnimation != null)
            {
                this.m_timerExitAnim.Stop();
                this.MouseEnterAnimation.Begin();
            }
        }
        /// <summary>
        /// shows the hostspot leave animation if defined
        /// </summary>
        protected void FireLeaveAnimation()
        {
            if (this.MouseLeaveAnimation != null)
            {
                m_timerExitAnim.Interval = new TimeSpan((long)(Delay * 10000000));
                m_timerExitAnim.Start();
            }
        }
    }
}
