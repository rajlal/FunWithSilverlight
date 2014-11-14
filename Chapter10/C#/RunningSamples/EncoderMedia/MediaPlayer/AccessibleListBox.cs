// <copyright file="AccessibleListBox.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <author>Microsoft Expression Encoder Team</author>
using System.Windows;
using System.Windows.Controls;
using System.Windows.Automation;
using System;
using System.Windows.Media;

namespace ExpressionMediaPlayer
{
    /// <summary>
    /// allows screen reader to read items defined in a listbox which are not listbox items.
    /// </summary>
    public class AccessibleListBox : ListBox
    {
        private static DependencyObject GetVisualParentByType(DependencyObject dpo, Type type)
        {
            while (dpo != null && dpo.GetType() != type)
            {
                dpo = VisualTreeHelper.GetParent(dpo);
            }

            return dpo;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            IAccessible accessible = item as IAccessible;
            if (accessible!=null)
            {
                Button btnFake = new Button();
                btnFake.IsEnabled = false;
                btnFake.Visibility = Visibility.Collapsed;
                btnFake.SetValue(AutomationProperties.NameProperty, accessible.AccessibilityText);
                element.SetValue(AutomationProperties.LabeledByProperty, (UIElement)btnFake);
            }

            ListBoxItem lbi = (ListBoxItem)GetVisualParentByType(element, typeof(ListBoxItem));
            
        }
    }
}
