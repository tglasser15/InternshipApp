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
using System.Net.NetworkInformation;
using TweetSharp;
using Parse;
using System.Collections.ObjectModel;

namespace InternshipApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        bool invalidEmail;
        bool invalidField;
        private string API_key = "v9QxYFQFTVUcImWwXq5Yhw";
        private string API_secret = "hVoDkA3Z9AtWAS6rS4scvZU4e2BntEe9Jth38PbPQ";
        private string AccessToken = "2412105906-r6teCDYAYMb81nCDazHbWszl0eE3uXYIpJj5jGm";
        private string AccessToken_secret = "BdR89dprE8NMdJc1tkCjoU5fCqU90XKGWpFSaR19VE7zg";

        static IEnumerable<TwitterStatus> bookmarks;
        static IEnumerable<TwitterStatus> posts;
        
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
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

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
                        if (NetworkInterface.GetIsNetworkAvailable())
            {
                //validate API keys
                var service = new TwitterService(API_key, API_secret);
                service.AuthenticateWith(AccessToken, AccessToken_secret);

                //ScreenName is the profile name of the twitter user.
                try
                {
                    service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions() { ScreenName = "Internship_DNF" }, (ts, rep) => //ts = twitter feeds
                    {
                        if (rep.StatusCode == HttpStatusCode.OK)
                        {
                            //bind
                            this.Dispatcher.BeginInvoke(() => { bookmarks = ts; });
                        }
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please check your internet connection.");
            }
        }

        public async void buttonLogin(object sender, RoutedEventArgs e)
        {
            if (bookmarks == null)
            {
                textBlockError.Visibility = Visibility.Visible;
                textBlockError.Text = "Loading";
            }
            else
            {
                posts = bookmarks;
                checkFields();
                if (invalidField == false)
                {
                    string myname = textEmail.Text;
                    string mypass = passwordBox.Password;


                    try
                    {
                        var user = await ParseUser.LogInAsync(myname, mypass);
                        //login successful
                        List<string> holder = new List<string>();
                        holder = user.Get<IList<string>>("test").ToList();
                        ObservableCollection<TwitterStatus> queue = new ObservableCollection<TwitterStatus>();
                        IEnumerable<TwitterStatus> temp;
                        foreach (string s in holder)
                        {
                            temp = bookmarks.Where(o => o.Text.Contains(s)).ToList();
                            foreach (TwitterStatus ts in temp)
                            {
                                queue.Add(ts);
                            }
                        }

                        if (queue != null)
                            bookmarks = queue.ToList();

                        NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        checkValidEmail(myname);
                        //login failed
                        textBlockError.Visibility = Visibility.Visible;
                        if (invalidEmail == true)
                            textBlockError.Text = "Account does not exist.";
                        else
                            textBlockError.Text = "Failed to login";


                    }
                }
            }
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

        public static IEnumerable<TwitterStatus> send_bookmarks()
        {
            return bookmarks;
        }

        public static IEnumerable<TwitterStatus> send_posts()
        {
            return posts;
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