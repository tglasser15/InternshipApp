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
using System.Collections.ObjectModel;


namespace InternshipApp
{
    //TS = part of TweetSharp Library - took code from example of online open source
    public partial class SearchPage : PhoneApplicationPage
    {
        static IEnumerable<TweetSharp.TwitterStatus> results; //create ienumberable of results to match twitter feed type
        static string internship_information; //string holder for internship information
        static List<string> result_indexing = new List<string>();
        static int index;
        static List<SearchItem> saved_searches = new List<SearchItem>();
        //register application on https://dev.twitter.com/ to retrieve API keys below

        private string API_key = "v9QxYFQFTVUcImWwXq5Yhw";
        private string API_secret = "hVoDkA3Z9AtWAS6rS4scvZU4e2BntEe9Jth38PbPQ";
        private string AccessToken = "2412105906-r6teCDYAYMb81nCDazHbWszl0eE3uXYIpJj5jGm";
        private string AccessToken_secret = "BdR89dprE8NMdJc1tkCjoU5fCqU90XKGWpFSaR19VE7zg";

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
            //savesearch.IsEnabled = false;

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
                            this.Dispatcher.BeginInvoke(() => { tweetList.ItemsSource = ts.Take(3); });
                            results = ts; //set twitter feeds to holder since ts is a local variable

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
                errorSearch.Text = "Please check your internet connection.";
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
                if ((SearchBar.Text == defaultSearch || SearchBar.Text == defaultSearch.ToUpper()) && (field == defaultField || field == defaultField.ToUpper()) && (LocationSearch.Text == defaultLocation || LocationSearch.Text == defaultLocation.ToUpper()))
                {

                    errorSearch.Text = "Invalid search requests";
                    errorSearch.Visibility = Visibility.Visible;
                }
                else
                {
                    //search GOOGLE API for city search
                    List<string> keys = new List<string>()
                    {
                        SearchBar.Text,field,LocationSearch.Text
                    };

                    char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                    string[] words = SearchBar.Text.Split(delimiterChars);
                    foreach (string s in words)
                    {
                        keys.Add(s);
                    }

                   // List<string> queue = new List<string>();
                    
                    //int index = -1;
                    ObservableCollection<TwitterStatus> queue = new ObservableCollection<TwitterStatus>();
                    IEnumerable<TwitterStatus> temp;
                    //filter results based on what is typed in the search bar
                    if (SearchBar.Text != defaultSearch || field != defaultField || LocationSearch.Text != defaultLocation)
                    {
                        foreach (string k in keys)
                        {
                            temp = results.Where(o => o.Text.ToUpper().Contains(k)).ToList();
                            foreach (TwitterStatus ts in temp)
                            {
                                queue.Add(ts);
                            }
                        }

                        results = queue.Distinct().ToList();
                    }
                    //save searches
                    if (SearchBar.Text == defaultSearch.ToUpper())
                        SearchBar.Text = string.Empty;
                    if (LocationSearch.Text == defaultLocation.ToUpper())
                        LocationSearch.Text = string.Empty;
                    if (field == defaultField.ToUpper())
                        field = string.Empty;
                    

                    saved_searches.Add(new SearchItem(SearchBar.Text, field, LocationSearch.Text));

                    NavigationService.Navigate(new Uri("/SearchResults.xaml", UriKind.Relative)); //navigate to search results page
                }
            }

        }

        //sends filtered results
        public static IEnumerable<TweetSharp.TwitterStatus> send_results()
        {
            return results;
        }

        //if an item in the listbox is pressed..
        private void internshipButton(object sender, RoutedEventArgs e)
        {
            result_indexing = results.Select(o => o.Text).ToList();
            internship_information = (sender as Button).Content.ToString(); //retrieve content from the items in the listbox
            index = result_indexing.IndexOf(internship_information);
            NavigationService.Navigate(new Uri("/Individual.xaml?param=RecentInternships", UriKind.Relative)); //navigate to information on individual internships
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

        //retrieve information on internship
        public static string send_internshipInformation()
        {
            return internship_information;
        }

        private void Search_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
        }
        private void Bookmarks_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Bookmarks.xaml", UriKind.Relative));
        }
        private void SavedSearches_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SaveSearchPage.xaml", UriKind.Relative));
        }
        public static int send_index()
        {
            return index;
        }
        public static List<SearchItem> send_savedSearches()
        {
            return saved_searches;
        }
    }
}