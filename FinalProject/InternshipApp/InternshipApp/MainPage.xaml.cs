using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using InternshipApp.Resources;
using System.Text.RegularExpressions;

namespace InternshipApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void buttonLogin(object sender, RoutedEventArgs e)
        {
            //if (textEmail.Text == "" || passwordBox.Password == "")
            //    checkFields();
            //else
            NavigationService.Navigate(new Uri("/AppMainPage.xaml", UriKind.Relative));
        }

        private void checkFields()
        {
            if (textEmail.Text != "")
            {
                if (ValidateEmail(textEmail.Text))
                {
                    if (passwordBox.Password != "")
                    {
                        if (passwordBox.Password.Length >= passwordBox.MaxLength)
                        {
                            textBlockError.Text = "Password can't be more than 20 characters long";
                            //passwordBox.BorderBrush = red;
                            textBlockError.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        textBlockError.Text = "Password cannot be empty";
                        textBlockError.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    textBlockError.Text = "Invalid Email address";
                    textBlockError.Visibility = Visibility.Visible;
                }
            }
            else
            {
                textBlockError.Text = "Email cannot be empty";
                textBlockError.Visibility = Visibility.Visible;
            }


        }

        private static bool ValidateEmail(string str)
        {
            return Regex.IsMatch(str, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }

        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            textBlockError.Text = "";
            textBlockError.Visibility = Visibility.Collapsed;
        }

        private void passwordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            textBlockError.Text = "";
            textBlockError.Visibility = Visibility.Collapsed;
        }
    }
}