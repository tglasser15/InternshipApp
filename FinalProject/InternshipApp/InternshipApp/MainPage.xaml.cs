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
        bool invalidEmail;
        bool invalidField;
        public MainPage()
        {
            InitializeComponent();

            //check if user is already logged on
            if (ParseUser.CurrentUser != null)
            {
                NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
            }

            invalidEmail = false;
            invalidField = false;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        //private void buttonLogin(object sender, RoutedEventArgs e)

        public async void buttonLogin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
            //checkFields();
            //if (invalidField == false)
            //{
            //    string myname = textEmail.Text;
            //    string mypass = passwordBox.Password;


            //    try
            //    {
            //        await ParseUser.LogInAsync(myname, mypass);
            //        //login successful
            //        NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
            //    }

            //    catch (Exception ex)
            //    {
            //        checkValidEmail(myname);
            //        //login failed
            //        textBlockError.Visibility = Visibility.Visible;
            //        if (invalidEmail == true)
            //            textBlockError.Text = "Account does not exist.";
            //        else
            //            textBlockError.Text = "Failed to login";


            //    }
            //}

        }

        public async void checkValidEmail(string myname)
        {
            var checkEmail = await(from user in ParseUser.Query where user.Get<string>("username") == myname select user).FindAsync();
            if (!checkEmail.Any())
                invalidEmail = true;

        }

        public async void SignUp_Click(object sender, RoutedEventArgs e)
        {
            checkFields();
            if (invalidField == false)
            {
                try
                {
                    var user = new ParseUser()
                    {
                        Username = textEmail.Text,
                        Password = passwordBox.Password,
                        Email = textEmail.Text
                    };

                    await user.SignUpAsync();

                }

                catch (Exception ex)
                {
                    textBlockError.Visibility = Visibility.Visible;
                    textBlockError.Text = "Failed to sign up.";
                }

                NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
            }
        }


        private void checkFields()
        {
            if (textEmail.Text != "")
            {
                invalidField = false;
                if (ValidateEmail(textEmail.Text))
                {
                    invalidField = false;
                    if (passwordBox.Password != "")
                    {
                        invalidField = false;
                        if (passwordBox.Password.Length >= passwordBox.MaxLength)
                        {
                            invalidField = true;
                            textBlockError.Text = "Password can't be more than 20 characters long";
                            textBlockError.Visibility = Visibility.Visible;
                        }
                        else
                            invalidField = false;
                    }
                    else
                    {
                        invalidField = true;
                        textBlockError.Text = "Password cannot be empty";
                        textBlockError.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    invalidField = true;
                    textBlockError.Text = "Invalid Email address";
                    textBlockError.Visibility = Visibility.Visible;
                }
            }
            else
            {
                invalidField = true;
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