// <copyright file="SmoothSlider.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace ExpressionMediaPlayer
{
    public class SmoothSlider : SensitiveSlider
    {
        /// <summary>
        /// animation storyboard for smooth slider
        /// </summary>
        private Storyboard m_Animation;
        private DoubleAnimation m_DoubleAnimation;
        /// <summary>
        /// smooth animating slider
        /// </summary>
        public SmoothSlider()
        {
            m_Animation = new Storyboard();
            m_DoubleAnimation = new DoubleAnimation();
            m_DoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            Storyboard.SetTarget(m_DoubleAnimation, this);
            Storyboard.SetTargetProperty(m_DoubleAnimation, new PropertyPath("(SmoothSlider.Value)"));
            m_Animation.Children.Add(m_DoubleAnimation);
            CircleEase circleEasing = new CircleEase();
            circleEasing.EasingMode = EasingMode.EaseInOut;
            m_DoubleAnimation.EasingFunction = circleEasing;
        }
        /// <summary>
        /// value of slider
        /// </summary>
        public new double Value
        {
            set
            {
                m_DoubleAnimation.From = base.Value;
                m_DoubleAnimation.To = value;
                m_Animation.Begin();
            }
            get
            {
                return base.Value;
            }
        }
        /// <summary>
        /// overriden to prevent updating the mediaelement position while the slider is animating.
        /// </summary>
        public override bool IsDragging
        {
            get
            {
                if (base.IsDragging)
                {
                    return true;
                }

                return m_Animation.GetCurrentState() == ClockState.Active;
            }
        }
        /// <summary>
        /// update value on mouse click
        /// </summary>
        protected override void OnMouseClick(object sender, MouseButtonEventArgs args)
        {
            Value = CalcValue(args);
        }
    }
}
