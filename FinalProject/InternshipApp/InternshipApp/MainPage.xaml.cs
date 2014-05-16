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
        //flags to determine whether or not a certain field or email is invalid
        bool invalidEmail;
        bool invalidField;
        //flag to determine whether or not posts have been pulled from twitter 
        bool fill_posts = false;

        //secret API keys and tokens to access twitter permissions
        private string API_key = "v9QxYFQFTVUcImWwXq5Yhw";
        private string API_secret = "hVoDkA3Z9AtWAS6rS4scvZU4e2BntEe9Jth38PbPQ";
        private string AccessToken = "2412105906-r6teCDYAYMb81nCDazHbWszl0eE3uXYIpJj5jGm";
        private string AccessToken_secret = "BdR89dprE8NMdJc1tkCjoU5fCqU90XKGWpFSaR19VE7zg";
        //containers to gather the twitter feeds
        static IEnumerable<TwitterStatus> container;    //from internshipp
        static IEnumerable<TwitterStatus> container2;   //from Internship_DNF


        static IEnumerable<TwitterStatus> bookmarks;    //container for bookmarks retrieved from Parse
        static ObservableCollection<TwitterStatus> posts = new ObservableCollection<TwitterStatus>();   //contains the total amount of twitter posts
        static List<SearchItem> saved_searches = new List<SearchItem>(); //contains all the search fields

        //search fields pulled from Parsed
        List<string> general = new List<string>();
        List<string> major = new List<string>();
        List<string> location = new List<string>();

        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            //check if user is already logged on
            //if (ParseUser.CurrentUser != null)
            //{
            //    NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
            //}

            //preset invalid fields to false
            invalidEmail = false;
            invalidField = false;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            posts.Clear(); //upon each visit of page, clear posts to be reinitialized, otherwise posts will keep expanding (with same tweets)

            //Source: http://code.msdn.microsoft.com/wpapps/Twitter-Sample-using-f36bab75 
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                //validate API keys
                var service = new TwitterService(API_key, API_secret);
                service.AuthenticateWith(AccessToken, AccessToken_secret);

                //ScreenName is the profile name of the twitter user.
                try
                {
                    //pulling from two separate twitter screen names
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
                    //for debugging purposes
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
            //if not all of the tweets have been pulled, do not add to total posts
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
            
            //if there are no posts. prompt user to wait and try again
            if (posts.Count == 0)
            {
                textBlockError.Visibility = Visibility.Visible;
                textBlockError.Text = "Wait for tweets. Click again.";
            }
            else
            {
                checkFields(); //check if user input is valid
                if (invalidField == false)
                {
                    //user identification output for Parse authorization
                    string myname = textEmail.Text;
                    string mypass = passwordBox.Password;

                    //Parse uses various try and catch due to sync implementations - would want to catch errors or exceptions instead of continuing or breaking
                    try
                    {
                        var user = await ParseUser.LogInAsync(myname, mypass); //login
                        //login successful

                        //Get bookmarks
                        List<string> bookmark_holder = new List<string>(); //container to hold text of bookmarks
                        try
                        {
                            bookmark_holder = user.Get<IList<string>>("Bookmarks").ToList();    //retrieve the bookmark strings from Parse
                            ObservableCollection<TwitterStatus> queue = new ObservableCollection<TwitterStatus>(); //temporary holder from tweets
                            IEnumerable<TwitterStatus> temp; //used in replacement of posts to avoid overriding posts
                            foreach (string s in bookmark_holder)
                            {
                                temp = posts.Where(o => o.Text.Contains(s)).ToList(); //if any of the posts contain the bookmark text, 
                                foreach (TwitterStatus ts in temp)
                                {
                                    queue.Add(ts); //add to the queue 
                                }
                            }

                            if (queue != null)
                                bookmarks = queue.ToList(); //set queue to bookmarks which holds user's previous bookmarks
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        //Get saved searches
                        try
                        {
                            //retrieve user's saved search fields
                            general = user.Get<IList<string>>("Save_General").ToList();
                            major = user.Get<IList<string>>("Save_Major").ToList();
                            location = user.Get<IList<string>>("Save_Location").ToList();

                            for (int i = 0; i < general.Count; i++)
                            {
                                saved_searches.Add(new SearchItem(general[i], major[i], location[i])); //add search fields back into originally class object
                            }
                        }
                        catch (Exception ex)
                        {
                            //debugging purposes
                            Console.WriteLine(ex.Message);
                        }


                        NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative)); //navigate to default search page upon login completion
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message); //for debugging purposes
                        checkValidEmail(myname);    //check if username is valid email
                        //if login failed
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
                        //create new user
                        var user = new ParseUser()
                        {
                            Username = textEmail.Text,
                            Password = passwordBox.Password,
                            Email = textEmail.Text
                        };

                        await user.SignUpAsync(); //sign new user
                        NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative)); //navigate to default page
                    }
                    catch (Parse.ParseException ex)
                    {
                        //if user already exists
                        textBlockError.Visibility = Visibility.Visible;
                        if (ex.Code == ParseException.ErrorCode.UsernameTaken)
                            textBlockError.Text = "Username already taken.";
                        else
                            textBlockError.Text = "Failed to sign up.";
                    }

                }
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


        }   //check for invali fields
        private bool ValidateEmail(string str)
        {
            return Regex.IsMatch(str, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }   //function for validating email
        //focus property of password box
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

        public static IEnumerable<TwitterStatus> send_bookmarks()
        {
            return bookmarks;
        } //send user's bookmarks
        public static ObservableCollection<TwitterStatus> send_posts()
        {
            return posts;
        } //send total posts for viewing and searching
        public static List<SearchItem> send_savedSearches()
        {
            return saved_searches;
        }   //send user's saved searches
    }
}