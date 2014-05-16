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
using Parse;


namespace InternshipApp
{
    public partial class SearchPage : PhoneApplicationPage
    {
        static IEnumerable<TweetSharp.TwitterStatus> results; //container to hold outcome of search

        static List<SearchItem> saved_searches = new List<SearchItem>(); //container to reveal user search input
        //filters of each saved search
        static List<string> save_general = new List<string>();
        static List<string> save_major = new List<string>();
        static List<string> save_location = new List<string>();

        //default search entries 
        private string defaultSearch = "Search";
        private string defaultLocation = "Enter Location";
        private string defaultField = "Please select a field...";

        static IEnumerable<TweetSharp.TwitterStatus> BookmarkList; //container of user's bookmarks

        public SearchPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SearchPage_Loaded); 
            results = MainPage.send_posts(); //results is initialized to all the posts - will be manipulated with search filters
        }

        void SearchPage_Loaded(object sender, RoutedEventArgs e)
        {
            //every time page is loaded, set to default searches
            SearchBar.Text = defaultSearch;
            LocationSearch.Text = defaultLocation;
            
            results = MainPage.send_posts(); //reinitialize results upon each visit 

            //set list of bookmarks based on navigation criteria
            if (Individual.send_bookmarks().Count == 0)
                BookmarkList = MainPage.send_bookmarks();
            else
                BookmarkList = Individual.send_bookmarks();

            BookmarkList = BookmarkList.Distinct().ToList(); //make the list distinct
            saved_searches = MainPage.send_savedSearches(); //set saved_searches to Parse retrieval
        }


        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            
            //set text fields to upper - will be used later to filter results
            SearchBar.Text = SearchBar.Text.ToUpper();
            LocationSearch.Text = LocationSearch.Text.ToUpper();
            string field = ((ListPickerItem)OptionSelector.SelectedItem).Content.ToString();
            field = field.ToUpper();


                //if text edit fields are in their default texts, report invalid search
                if ((SearchBar.Text == defaultSearch || SearchBar.Text == defaultSearch.ToUpper()) && 
                    (field == defaultField || field == defaultField.ToUpper()) && 
                    (LocationSearch.Text == defaultLocation || LocationSearch.Text == defaultLocation.ToUpper()))
                {
                    errorSearch.Text = "Invalid search requests";
                    errorSearch.Visibility = Visibility.Visible;
                }
                else
                {
                    //create a list of keys used for searching through internships
                    List<string> keys = new List<string>()
                    {
                        SearchBar.Text,field,LocationSearch.Text
                    };

                    char[] delimiterChars = { ' ', ',', '.', ':', '\t'};
                    //split each filter into words
                    string[] words = SearchBar.Text.Split(delimiterChars);
                    string[] words2 = field.Split(delimiterChars);
                    string[] words3 = LocationSearch.Text.Split(delimiterChars);
                    //restrict keys to only words
                    foreach (string s in words)
                    {
                        if (s != "" && s.Count() != 1) //exclude empty literals or single chracters
                            keys.Add(s);
                    }
                    foreach (string s in words2)
                    {
                        if (s != "" && s.Count() != 1)
                            keys.Add(s);
                    }
                    foreach (string s in words3)
                    {
                        if (s != "" && s.Count() != 1)
                            keys.Add(s);
                    }

                    ObservableCollection<TwitterStatus> queue = new ObservableCollection<TwitterStatus>(); //temporary bookmark adder
                    IEnumerable<TwitterStatus> temp;    //temporary intializer to avoid overriding results data
                    //filter results based on what is typed in the search bar
                    if (SearchBar.Text != defaultSearch || field != defaultField || LocationSearch.Text != defaultLocation)
                    {
                        //iterate through each key to determine if the internship contains the key
                        foreach (string k in keys)
                        {
                            temp = results.Where(o => o.Text.ToUpper().Contains(k)).ToList();
                            foreach (TwitterStatus ts in temp)
                            {
                                queue.Add(ts); //add to the queue
                            }
                        }

                        results = queue.Distinct().ToList(); //set results to distinct list
                    }

                    //continue if results are empty
                    if (results.Count() != 0)
                    {
                        //if any of the filters are still set to their default values, create empty literals
                        if (SearchBar.Text == defaultSearch.ToUpper())
                            SearchBar.Text = string.Empty;
                        if (LocationSearch.Text == defaultLocation.ToUpper())
                            LocationSearch.Text = string.Empty;
                        if (field == defaultField.ToUpper())
                            field = string.Empty;

                        //add a new search item
                        saved_searches.Add(new SearchItem(SearchBar.Text, field, LocationSearch.Text));
                    }

                    NavigationService.Navigate(new Uri("/SearchResults.xaml?param=results", UriKind.Relative)); //navigate to search results page
                }
            

        }

        //to set filters based on focus
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

        //iterating through main pages
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

        //retrieve all internships posted
        private void buttonSeeAll_click(object sender, RoutedEventArgs e)
        {
            results = MainPage.send_posts();
            if (results == null)
            {
                errorSearch.Text = "Wait for tweets to load";
                errorSearch.Visibility = Visibility.Visible;
            }
            else
                NavigationService.Navigate(new Uri("/SearchResults.xaml?param=all", UriKind.Relative));
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            //disallow user to return to main login page when on the search page
            e.Cancel = true;
        }

        public static IEnumerable<TweetSharp.TwitterStatus> send_results()
        {
            return results;
        } //send results to be displayed
        public static List<SearchItem> send_savedSearches()
        {
            return saved_searches;
        }   //send filter searches

        //allow user to only logout only on search page
        private async void Logout_Click(object sender, EventArgs e)
        {
            List<string> str_books = new List<string>();    //Parse can only save text
            str_books = BookmarkList.Select(o => o.Text).ToList();  //convert bookmakrs to string
            //save the recent 5 searches for the user
            if (saved_searches.Count() > 5)
            {
                for (int i = saved_searches.Count - 1; i > saved_searches.Count - 6; i--)
                {
                    save_general.Add(saved_searches.ElementAt(i).GeneralSearch);
                    save_major.Add(saved_searches.ElementAt(i).DisciplineSearch);
                    save_location.Add(saved_searches.ElementAt(i).Location);
                }
            }
            else
            {
                for (int i = 0; i > saved_searches.Count; i++)
                {
                    save_general.Add(saved_searches.ElementAt(i).GeneralSearch);
                    save_major.Add(saved_searches.ElementAt(i).DisciplineSearch);
                    save_location.Add(saved_searches.ElementAt(i).Location);
                }
            }

            //store data on Parse
            var user = ParseUser.CurrentUser;
            user["Bookmarks"] = str_books;
            user["Save_General"] = save_general;
            user["Save_Major"] = save_major;
            user["Save_Location"] = save_location;
            user.ACL = new ParseACL(ParseUser.CurrentUser);
            await user.SaveAsync();
            ParseUser.LogOut(); //logout

            //clear out lists if another user decides to logon
                //mainly for emulator purposes
            saved_searches.Clear();
            save_general.Clear();
            save_location.Clear();
            save_major.Clear();
            Individual.Bookmark_Clear();
            SaveSearchPage.saves_clear();

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative)); //navigate back to main page
        }
    }
}