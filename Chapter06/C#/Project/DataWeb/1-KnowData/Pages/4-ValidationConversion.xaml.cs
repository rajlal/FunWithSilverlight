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
using System.Windows.Data;

namespace KnowData
{
    public partial class ValidationConversion : UserControl
    {
        HeterogeneousData myData = new HeterogeneousData();
        public ValidationConversion()
        {
            InitializeComponent();
        }
        private void MyTextBox_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (e.Action == ValidationErrorEventAction.Added)
            {
                if (tb.Name == "txtName")
                    errName.Visibility = Visibility.Visible;
                else if (tb.Name == "txtEmail")
                    errEmail.Visibility = Visibility.Visible;
                else if (tb.Name == "txtZipcode")
                    errZipcode.Visibility = Visibility.Visible;


            }
            else if (e.Action == ValidationErrorEventAction.Removed)
            {
                if (tb.Name == "txtName")
                    errName.Visibility = Visibility.Collapsed;
                else if (tb.Name == "txtEmail")
                    errEmail.Visibility = Visibility.Collapsed;
                else if (tb.Name == "txtZipcode")
                    errZipcode.Visibility = Visibility.Collapsed;
            }
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
           
                txtName.Text = "Developer";
                txtEmail.Text = "Dev@silverlightfun.com";
                txtZipcode.Text = "92126";
        }
        private void Select(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            string Selected = t.Text;
            StatusBar.Text = ToolTipService.GetToolTip(t).ToString();
            if (Selected == "Validation")
            {
                CanvasValidation.Visibility = Visibility.Visible;
                CanvasConversion.Visibility = Visibility.Collapsed;
            }
            else if (Selected == "Conversion")
            {
                CanvasValidation.Visibility = Visibility.Collapsed;
                CanvasConversion.Visibility = Visibility.Visible;
                stackDataConverted.DataContext = myData; 
            }
        }
        private void layoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            CanvasValidation.Visibility = Visibility.Visible;
            CanvasConversion.Visibility = Visibility.Collapsed;
            stackDataOriginal.DataContext = myData;
        }
       
    }
    public class Account
    {
        private string name = "Developer";
        private string email = "Dev@silverlightfun.com";
        private string zipcode ="92126";

        public string Name
        {
            get { return name; }
            set
            {
                if (value.Length < 1)
                    throw new Exception("Name is Required");
                name = value;
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                if (!(value.Contains("@") && value.Contains(".")))
                    throw new Exception("Invalid Email");
                email = value;
            }
        }
        public string Zipcode
        {
            get { return zipcode; }
            set
            {
                if (Convert.ToInt32(value) > 0)
                {
                    if ((int.Parse(value) < 10000 || int.Parse(value) > 99999))
                        throw new Exception("5 Digit zipcode required");
                    zipcode = value;
                }
            }
        }

    }
    public class HeterogeneousData 
    {
        private DateTime dataDateTime= DateTime.Now;
        private string dataPhone = "8581112345";
        private double dataRating = 4.7867;
        public DateTime DataDateTime
        {
            get { return dataDateTime; }
            set
            {
                dataDateTime = value;
            }
        }
        public string DataPhone
        {
            get { return dataPhone; }
            set
            {

                dataPhone = value;
            }
        }
        public double DataRating
        {
            get { return dataRating; }
            set
            {

                dataRating = value;
            }
        }
    }
    public class PhoneToFormatedPhoneConverter : IValueConverter
    {
        // Define the Convert method to change a DateTime object to a month string.
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // value is the data from the source object.
            string inputPhone = (string)value;
            string outputPhone = "(" + inputPhone.Substring(0, 3) + ")" + inputPhone.Substring(3, 3) + "-" + inputPhone.Substring(6);
            // Return the value to pass to the target.
            return outputPhone;

        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DateTimeToDateConverter : IValueConverter
    {
        // Define the Convert method to change a DateTime object to a month string.
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // value is the data from the source object.
            DateTime inputdate = (DateTime)value;
            
            string outputDate = inputdate.ToLongDateString(); 
           
            // Return the value to pass to the target.
            return outputDate;

        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class RatingFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter != null)
            {
                string formatterString = parameter.ToString();

                if (!string.IsNullOrEmpty(formatterString))
                {
                    return string.Format(culture, formatterString, value);
                }
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
