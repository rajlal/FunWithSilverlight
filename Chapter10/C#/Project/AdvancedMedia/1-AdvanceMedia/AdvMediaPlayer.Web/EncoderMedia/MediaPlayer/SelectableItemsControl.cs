// <copyright file="SelectableItemsControl.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MediaPlayerExtensions
{
    public class SelectableItemsControl : ItemsControl
    {
        /// <summary>
        /// currently selected item
        /// </summary>
        private object m_SelectedItem;
        /// <summary>
        /// item hovering over
        /// </summary>
        private object m_HoverItem;
        /// <summary>
        /// index of item over
        /// </summary>
        private int m_HoverIndex;
        /// <summary>
        /// dependancy property for currently selected item index
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty = 
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(SelectableItemsControl), new PropertyMetadata(0, OnSelectedIndexChanged));
        /// <summary>
        /// dependancy property for currently hovered over index
        /// </summary>
        public static readonly DependencyProperty HoverIndexProperty = 
            DependencyProperty.Register("HoverIndex", typeof(int), typeof(SelectableItemsControl), new PropertyMetadata(0, OnHoverIndexChanged));
        /// <summary>
        /// selection changed event
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;
        /// <summary>
        /// hovered over item changed event
        /// </summary>
        public event SelectionChangedEventHandler HoverChanged;
        /// <summary>
        /// create new SelectableItemsControl
        /// </summary>
        public SelectableItemsControl()
        {
            SelectedIndex = -1;
            m_HoverIndex = -1;
        }
        /// <summary>
        /// override ScrollIntoView to do nothing
        /// </summary>
        /// <param name="item"></param>
        public virtual void ScrollIntoView(object item) 
        {
        }
        /// <summary>
        /// wire events for the content container of this items control
        /// </summary>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            ContentPresenter contentPresenter = element as ContentPresenter;
            if (contentPresenter != null)
            {
                contentPresenter.MouseEnter += new MouseEventHandler(OnItemMouseEnter);
                contentPresenter.MouseLeave += new MouseEventHandler(OnItemMouseLeave);
                contentPresenter.MouseLeftButtonDown += new MouseButtonEventHandler(OnItemClicked);
            }

            base.PrepareContainerForItemOverride(element, item);
        }
        /// <summary>
        /// item clicked
        /// </summary>
        void OnItemClicked(object sender, MouseButtonEventArgs e)
        {
            ContentPresenter contentPresenter = sender as ContentPresenter;
            // get the index
            Panel panel = VisualTreeHelper.GetParent(contentPresenter) as Panel;
            if (panel == null)
            {
                return;
            }

            // force the selected index changed call..
            SelectedIndex = -1;
            SelectedIndex = panel.Children.IndexOf(contentPresenter);
        }
        /// <summary>
        /// item hovered over, fire enter state in VSM
        /// </summary>
        void OnItemMouseEnter(object sender, MouseEventArgs e)
        {
            ContentPresenter contentPresenter = sender as ContentPresenter;
            Control child = VisualTreeHelper.GetChild(contentPresenter, 0) as Control;
            if (child == null)
            {
                return;
            }
            VisualStateManager.GoToState(child, "MouseEnter", true);

            // determine what the hover item index is...
            Panel panel = VisualTreeHelper.GetParent(contentPresenter) as Panel;
            if (panel == null)
            {
                return;
            }

            HoverIndex = panel.Children.IndexOf(contentPresenter);
        }
        /// <summary>
        /// item no longer hovered over, fire leave state in VSM
        /// </summary>
        void OnItemMouseLeave(object sender, MouseEventArgs e)
        {
            ContentPresenter contentPresenter = sender as ContentPresenter;
            Control child = VisualTreeHelper.GetChild(contentPresenter, 0) as Control;
            if (child == null)
            {
                return;
            }
            VisualStateManager.GoToState(child, "MouseLeave", true);
        }
        /// <summary>
        /// property wrapper for SelectedIndex dependancy property
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }
        /// <summary>
        /// SelectedIndex dependancy property changed
        /// </summary>
        protected static void OnSelectedIndexChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SelectableItemsControl list = obj as SelectableItemsControl;
            if (list != null)
            {
                list.UpdateSelectedIndex();
            }
        }
        /// <summary>
        /// property wrapper for HoverIndex dependancy property
        /// </summary>
        public int HoverIndex
        {
            get { return (int)GetValue(HoverIndexProperty); }
            set { SetValue(HoverIndexProperty, value); }
        }        
        /// <summary>
        /// HoverIndex dependancy property changed
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        protected static void OnHoverIndexChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SelectableItemsControl list = obj as SelectableItemsControl;
            if (list != null)
            {
                list.UpdateHoverIndex();
            }
        }
        /// <summary>
        /// sync the HoverItem from the current hovered over index
        /// </summary>
        private void UpdateHoverIndex()
        {
            int hoverIndex = Math.Max(-1, Math.Min(HoverIndex, Items.Count - 1));
            if (hoverIndex == m_HoverIndex)
                return;

            m_HoverIndex = hoverIndex;
            if (hoverIndex == -1)
            {
                HoverItem = null;
            }
            else
            {
                HoverItem = Items[m_HoverIndex];
            }
        }
        /// <summary>
        /// update SelectedItem from the currently selected item index
        /// </summary>
        private void UpdateSelectedIndex()
        {
            int selectedIndex = Math.Max(-1, Math.Min(SelectedIndex, Items.Count - 1));

            if (selectedIndex == -1)
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = Items[SelectedIndex];
            }
        }
        /// <summary>
        /// currently selected item
        /// </summary>
        public object SelectedItem
        {
            get
            {
                return m_SelectedItem;
            }
            set
            {
                m_SelectedItem = value;
                if (SelectionChanged != null)
                {
                    SelectionChanged(this, null);
                }
            }
        }
        /// <summary>
        /// current item hovered over
        /// </summary>
        public object HoverItem
        {
            get
            {
                return m_HoverItem;
            }
            set
            {
                if (value == m_HoverItem)
                    return;

                m_HoverItem = value;
                if (HoverChanged != null)
                {
                    HoverChanged(this, null);
                }
            }
        }        
    }
}
