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
using System.Net.NetworkInformation;
using TweetSharp;
using Windows.UI.Popups;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace InternshipApp
{
    //TS = part of TweetSharp Library - took code from example of online open source
    public partial class SearchPage : PhoneApplicationPage
    {

        static IEnumerable<TweetSharp.TwitterStatus> results; //create ienumberable of results to match twitter feed type
        static string internship_information; //string holder for internship information
        static bool recent_pressed = false; //initialize status of buttons in the recent internships

        //register application on https://dev.twitter.com/ to retrieve API keys below
        private string API_key = "Enter API Key Here";
        private string API_secret = "Enter API Secret Here";
        private string AccessToken = "Enter Access Token Here";
        private string AccessToken_secret = "Enter Access Token Secret Here";

        //default search entries 
        private string defaultSearch = "Search Here";
        private string defaultLocation = "Enter Location";
        private string defaultField = "Please select a field...";
        
        public SearchPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SearchPage_Loaded); //TS
            
        }

        //set textboxes based on whether or not they have been clicked
        private void SearchBar_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchBar.Text = "";
        }

        private void SearchBar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBar.Text == string.Empty)
                SearchBar.Text = "Search Here";
        }
        private void LocationBar_GotFocus(object sender, RoutedEventArgs e)
        {
            LocationSearch.Text = "";
        }

        private void LocationBar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LocationSearch.Text == string.Empty)
                LocationSearch.Text = "Enter location";
        }
        //////////////////

        //TS
        void SearchPage_Loaded(object sender, RoutedEventArgs e)
        {

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                //validate API keys
                var service = new TwitterService(API_key, API_secret );
                service.AuthenticateWith(AccessToken, AccessToken_secret);

                //ScreenName is the profile name of the twitter user.
                service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions() { ScreenName = "Internship_DNF" }, (ts, rep) => //ts = twitter feeds
                {
                    if (rep.StatusCode == HttpStatusCode.OK)
                    {
                        //bind
                        this.Dispatcher.BeginInvoke(() => { tweetList.ItemsSource = ts; });
                        results = ts; //set twitter feeds to holder since ts is a local variable
                    }
                });
            }
            else
            {

                MessageBox.Show("Please check your internet connection.");
            }

        }

        
        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            //set text fields to upper - will be used later to filter results
            SearchBar.Text = SearchBar.Text.ToUpper();
            LocationSearch.Text = LocationSearch.Text.ToUpper();
            string field = ((ListPickerItem)OptionSelector.SelectedItem).Content.ToString();
            field = field.ToUpper();
            
            //if there are no results, allow for tweets to load
            if (results == null)
            {
                errorSearch.Text = "Wait for tweets to load";
                errorSearch.Visibility = Visibility.Visible;
            }
            else
            {
                //if text edit fields are in their default texts, report invalid search
                if ((SearchBar.Text == defaultSearch || SearchBar.Text == defaultSearch.ToUpper()) && (field == defaultField || field == defaultField.ToUpper()) && (LocationSearch.Text == defaultLocation ||                              LocationSearch.Text == defaultLocation.ToUpper()))
                {
                    
                    errorSearch.Text = "Invalid search requests";
                    errorSearch.Visibility = Visibility.Visible;
                }
                else
                {
                    //only the searchbar is currently working now

                    //filter results based on what is typed in the search bar
                    if (SearchBar.Text != defaultSearch)
                        results = results.Where(o => o.Text.ToUpper().Contains(SearchBar.Text)).ToArray(); 
                    
                    NavigationService.Navigate(new Uri("/SearchResults.xaml", UriKind.Relative)); //navigate to search results page
                }
            }

        }
        
        //retrieves filtered results
        public static IEnumerable<TweetSharp.TwitterStatus> send_results()
        {
            return results;
        }

        //if an item in the listbox is pressed..
        private void internshipButton(object sender, RoutedEventArgs e)
        {
            recent_pressed = true; //set condition of buttons to true
            SearchResults.set_button_false(); //set condition of buttons from result internships to false
            internship_information = (sender as Button).Content.ToString(); //retrieve content of item on listbox
            NavigationService.Navigate(new Uri("/Individual.xaml", UriKind.Relative)); //navigate to information on individual internships
        }

        //retrieve all internships posted
        private void buttonSeeAll_click(object sender, RoutedEventArgs e)
        {
            if (results == null)
            {
                errorSearch.Text = "Wait for tweets to load";
                errorSearch.Visibility = Visibility.Visible;
            }
            else
                NavigationService.Navigate(new Uri("/SearchResults.xaml", UriKind.Relative));
        }

        private void previousPage(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        //retrieve information on internship
        public static string send_internshipInformation()
        {
            return internship_information;
        }

        //set condition of buttons to false when recent internship buttons are used
        public static void set_button_false()
        {
            recent_pressed = false;
        }
        
        //return status of button
        public static bool return_button_status()
        {
            return recent_pressed;
        }

    }
}