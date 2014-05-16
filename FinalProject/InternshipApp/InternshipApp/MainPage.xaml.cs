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
using System.Threading;
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
        static IEnumerable<TwitterStatus> container;
        static IEnumerable<TwitterStatus> container2;
        static ObservableCollection<TwitterStatus> posts = new ObservableCollection<TwitterStatus>();
        static List<SearchItem> saved_searches = new List<SearchItem>();

        static List<string> general = new List<string>();
        static List<string> major = new List<string>();
        static List<string> location = new List<string>();

        bool fill_posts = false;
        
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            //check if user is already logged on
            //if (ParseUser.CurrentUser != null)
            //{
            //    NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
            //}

            invalidEmail = false;
            invalidField = false;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            posts.Clear();
            //static IEnumerable<TwitterStatus> temp;
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                //validate API keys
                var service = new TwitterService(API_key, API_secret);
                service.AuthenticateWith(AccessToken, AccessToken_secret);

                //ScreenName is the profile name of the twitter user.
                try
                {
                    service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions() { ScreenName = "internshipp" }, (ts, rep) => //ts = twitter feeds
                    {
                        if (rep.StatusCode == HttpStatusCode.OK)
                        { 
                            //bind
                            this.Dispatcher.BeginInvoke(() => { container = ts; });
                        }
                    });

                    service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions() { ScreenName = "Internship_DNF" }, (ts, rep) => //ts = twitter feeds
                    {
                        if (rep.StatusCode == HttpStatusCode.OK)
                        {
                            //bind
                            this.Dispatcher.BeginInvoke(() => { container2 = ts; });
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
            
            if (container != null && container2 != null)
            {
                if (fill_posts == false)
                {
                    foreach (TwitterStatus ts in container)
                        posts.Add(ts);
                    foreach (TwitterStatus ts in container2)
                        posts.Add(ts);
                    fill_posts = true;
                }
            }

            if (posts.Count == 0)
            {
                textBlockError.Visibility = Visibility.Visible;
                textBlockError.Text = "Wait for tweets. Click again.";
            }
            else
            {

                checkFields();
                if (invalidField == false)
                {
                    string myname = textEmail.Text;
                    string mypass = passwordBox.Password;


                    try
                    {
                        var user = await ParseUser.LogInAsync(myname, mypass);
                        //login successful

                        //Get bookmarks
                        List<string> bookmark_holder = new List<string>();
                        try
                        {
                            bookmark_holder = user.Get<IList<string>>("Bookmarks").ToList();
                            ObservableCollection<TwitterStatus> queue = new ObservableCollection<TwitterStatus>();
                            IEnumerable<TwitterStatus> temp;
                            foreach (string s in bookmark_holder)
                            {
                                temp = posts.Where(o => o.Text.Contains(s)).ToList();
                                foreach (TwitterStatus ts in temp)
                                {
                                    queue.Add(ts);
                                }
                            }

                            if (queue != null)
                                bookmarks = queue.ToList();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }


                        ////////////////////////////////

                        //Get saved searches
                        try
                        {
                            general = user.Get<IList<string>>("Save_General").ToList();
                            major = user.Get<IList<string>>("Save_Major").ToList();
                            location = user.Get<IList<string>>("Save_Location").ToList();

                            for (int i = 0; i < general.Count; i++)
                            {
                                saved_searches.Add(new SearchItem(general[i], major[i], location[i]));
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }


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
            if (container != null && container2 != null)
            {
                if (fill_posts == false)
                {
                    foreach (TwitterStatus ts in container)
                        posts.Add(ts);
                    foreach (TwitterStatus ts in container2)
                        posts.Add(ts);
                    fill_posts = true;
                }
            }

            if (posts.Count == 0)
            {
                textBlockError.Visibility = Visibility.Visible;
                textBlockError.Text = "Wait for tweets. Sign again.";
            }
            else
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

                        //var check = await (from _user in ParseUser.Query
                        //                   where _user.Get<string>("username") == user.Username
                        //                   select _user).FindAsync();



                        await user.SignUpAsync();
                        NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
                    }

                    catch (Parse.ParseException ex)
                    {
                        textBlockError.Visibility = Visibility.Visible;
                        if (ex.Code == ParseException.ErrorCode.UsernameTaken)
                            textBlockError.Text = "Username already taken.";
                        else
                            textBlockError.Text = "Failed to sign up.";
                    }

                }
            }
        }
        public static IEnumerable<TwitterStatus> send_bookmarks()
        {
            return bookmarks;
        }
        public static ObservableCollection<TwitterStatus> send_posts()
        {
            return posts;
        } 
        public static List<SearchItem> send_savedSearches()
        {
            return saved_searches;
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