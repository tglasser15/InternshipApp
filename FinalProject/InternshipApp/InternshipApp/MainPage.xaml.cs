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
using Parse;

namespace InternshipApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //check if user is already logged on
            if (ParseUser.CurrentUser != null)
            {
                NavigationService.Navigate(new Uri("/AppMainPage.xaml", UriKind.Relative));
            }

            

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        //private void buttonLogin(object sender, RoutedEventArgs e)
        
        public async void buttonLogin(object sender, RoutedEventArgs e)
        {
            //if (textEmail.Text == "" || passwordBox.Password == "")
            //    checkFields();
            //else

            try {
                await ParseUser.LogInAsync("myname", "mypass");
                //login successful
                NavigationService.Navigate(new Uri("/AppMainPage.xaml", UriKind.Relative));
            }
                
            catch (Exception ex)
            {
                //login failed
            }
            

           
        }

        public async void SignUp_Click(object sender, RoutedEventArgs e)
        {
            var user = new ParseUser()
            {
                Username = textEmail.Text,
                Password = passwordBox.DataContext.ToString(),
                Email = textEmail.Text
            };

            await user.SignUpAsync();

            //go to next page
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