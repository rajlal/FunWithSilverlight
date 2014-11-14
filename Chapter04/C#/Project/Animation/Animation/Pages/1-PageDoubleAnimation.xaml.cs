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
using System.Windows.Media.Imaging;

namespace Animation
{
    public partial class PageDoubleAnimation : UserControl
    {
        private bool flagPaused;
        private string Selected = "Truck";
        private string SelectedAnim = "Basic";
        private Storyboard myStoryboard = new Storyboard();

        public PageDoubleAnimation()
        {
            InitializeComponent();
        }

        private void btnPlay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GetStoryboard();
            myStoryboard.Stop();
            myStoryboard.Begin();

        }
        private void btnPause_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GetStoryboard();
            if (flagPaused)
            {
                myStoryboard.Resume();
                flagPaused = false;
            }
            else
            {
                myStoryboard.Pause();
                flagPaused = true;
            }

        }
        private void btnStop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GetStoryboard();
            myStoryboard.Stop();
        }

        private void ShowBasic(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            canvasBasic.Visibility = Visibility.Visible;
            SelectedAnim = "Basic";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void ShowSpeed(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            canvasSpeed.Visibility = Visibility.Visible;
            SelectedAnim = "Speed";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();

        }
        private void sliderFastDuration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                myStoryboard.Stop();

                TimeSpan animDuration = new TimeSpan(0,0, 0, 0, (int)sliderFastDuration.Value);
                animFastCar.Duration = animDuration;
                txtFastCarDuration.Text = "." + (int)sliderFastDuration.Value;
                
            }
            catch (Exception)
            { }
        }
        private void ShowRotation(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            canvasRotation.Visibility = Visibility.Visible;
            SelectedAnim = "Rotation";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void ShowRace(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            canvasRace.Visibility = Visibility.Visible;
            SelectedAnim = "Race";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();

        }
        private void sliderDuration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            try
            {
                myStoryboard.Stop();
                double animHours = sliderDuration.Value / 3600.0;
                double minutesLeft = sliderDuration.Value % 3600.0;
                double animMinutes = minutesLeft / 60.0;
                double animSeconds = minutesLeft % 60.0;

                TimeSpan animDuration = new TimeSpan((int)animHours, (int)animMinutes, (int)animSeconds);
                if (Selected == "Truck")
                    animDoubleTruck.Duration = animDuration;
                else
                    animDoubleCar.Duration = animDuration;

                txtDuration.Text = "Truck: " + animDoubleTruck.Duration.TimeSpan.Hours + ":" + animDoubleTruck.Duration.TimeSpan.Minutes + ":" + animDoubleTruck.Duration.TimeSpan.Seconds;
                txtDuration.Text += " / Car:" + animDoubleCar.Duration.TimeSpan.Hours + ":" + animDoubleCar.Duration.TimeSpan.Minutes + ":" + animDoubleCar.Duration.TimeSpan.Seconds;

            }
            catch (Exception)
            { }
        }
        private void ImgTruck_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Selected = "Truck";
            txtDuration.Text = "Truck: " + animDoubleTruck.Duration.TimeSpan.Hours + ":" + animDoubleTruck.Duration.TimeSpan.Minutes + ":" + animDoubleTruck.Duration.TimeSpan.Seconds;
            txtDuration.Text += " / Car:" + animDoubleCar.Duration.TimeSpan.Hours + ":" + animDoubleCar.Duration.TimeSpan.Minutes + ":" + animDoubleCar.Duration.TimeSpan.Seconds;
            StatusBar.Text = "Selected: Truck";


        }
        private void ImgCar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Selected = "Car";
            txtDuration.Text = "Truck: " + animDoubleTruck.Duration.TimeSpan.Hours + ":" + animDoubleTruck.Duration.TimeSpan.Minutes + ":" + animDoubleTruck.Duration.TimeSpan.Seconds;
            txtDuration.Text += " / Car:" + animDoubleCar.Duration.TimeSpan.Hours + ":" + animDoubleCar.Duration.TimeSpan.Minutes + ":" + animDoubleCar.Duration.TimeSpan.Seconds;
            StatusBar.Text = "Selected: Car";

        }
        private void CollapseAll()
        {
            myStoryboard.Stop();
            canvasBasic.Visibility = Visibility.Collapsed;
            canvasRotation.Visibility = Visibility.Collapsed;
            canvasRace.Visibility = Visibility.Collapsed;
            canvasSpeed.Visibility = Visibility.Collapsed;
        }
        private Storyboard GetStoryboard()
        {
            if (SelectedAnim == "Basic")
            {
                myStoryboard = myBasicStoryboard;
            }
            else if (SelectedAnim == "Speed")
            {
                myStoryboard = mySpeedStoryboard;
            }
            else if (SelectedAnim == "Rotation")
            {
                myStoryboard = myRotationStoryboard;
            }
            else
            {
                myStoryboard = myRaceStoryboard;
            }
            return myStoryboard;
        }


        


    }
}
