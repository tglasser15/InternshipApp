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
    //TS = part of TweetSharp Library - took code from example of online open source
    public partial class SearchPage : PhoneApplicationPage
    {
        static IEnumerable<TweetSharp.TwitterStatus> results; //create ienumberable of results to match twitter feed type
        static string internship_information; //string holder for internship information
        static List<string> result_indexing = new List<string>();
        static int index;
        static List<SearchItem> saved_searches = new List<SearchItem>();
        //register application on https://dev.twitter.com/ to retrieve API keys below

        //static List<SearchItem> saved_searches = new List<SearchItem>();
        static List<string> save_general = new List<string>();
        static List<string> save_major = new List<string>();
        static List<string> save_location = new List<string>();

        //default search entries 
        private string defaultSearch = "Search";
        private string defaultLocation = "Enter Location";
        private string defaultField = "Please select a field...";

        static IEnumerable<TweetSharp.TwitterStatus> BookmarkList;

        public SearchPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SearchPage_Loaded); //TS
            results = MainPage.send_posts();
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

        void SearchPage_Loaded(object sender, RoutedEventArgs e)
        {
            results = MainPage.send_posts();
            if (MainPage.send_bookmarks() != null)
                BookmarkList = MainPage.send_bookmarks();
            else
                BookmarkList = Individual.send_bookmarks();

            BookmarkList = BookmarkList.Distinct().ToList();
            saved_searches = MainPage.send_savedSearches();
        }


        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            
            //set text fields to upper - will be used later to filter results
            SearchBar.Text = SearchBar.Text.ToUpper();
            LocationSearch.Text = LocationSearch.Text.ToUpper();
            string field = ((ListPickerItem)OptionSelector.SelectedItem).Content.ToString();
            field = field.ToUpper();


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
                    if (results.Count() != 0)
                    {
                        //save searches
                        if (SearchBar.Text == defaultSearch.ToUpper())
                            SearchBar.Text = string.Empty;
                        if (LocationSearch.Text == defaultLocation.ToUpper())
                            LocationSearch.Text = string.Empty;
                        if (field == defaultField.ToUpper())
                            field = string.Empty;


                        saved_searches.Add(new SearchItem(SearchBar.Text, field, LocationSearch.Text));
                    }

                    NavigationService.Navigate(new Uri("/SearchResults.xaml?param=results", UriKind.Relative)); //navigate to search results page
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
            results = MainPage.send_posts();
            if (results == null)
            {
                errorSearch.Text = "Wait for tweets to load";
                errorSearch.Visibility = Visibility.Visible;
            }
            else
                NavigationService.Navigate(new Uri("/SearchResults.xaml?param=all", UriKind.Relative));
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

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private async void Logout_Click(object sender, EventArgs e)
        {
            List<string> str_books = new List<string>();
            str_books = BookmarkList.Select(o => o.Text).ToList();
            for (int i = saved_searches.Count-1; i > saved_searches.Count-6; i--)
            {
                try
                {
                    save_general.Add(saved_searches.ElementAt(i).GeneralSearch);
                    save_major.Add(saved_searches.ElementAt(i).DisciplineSearch);
                    save_location.Add(saved_searches.ElementAt(i).Location);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            var user = ParseUser.CurrentUser;
            user["Bookmarks"] = str_books;
            user["Save_General"] = save_general;
            user["Save_Major"] = save_major;
            user["Save_Location"] = save_location;
            user.ACL = new ParseACL(ParseUser.CurrentUser);
            await user.SaveAsync();
            ParseUser.LogOut();

            saved_searches.Clear();
            save_general.Clear();
            save_location.Clear();
            save_major.Clear();
            Individual.Bookmark_Clear();
            SaveSearchPage.saves_clear();

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}