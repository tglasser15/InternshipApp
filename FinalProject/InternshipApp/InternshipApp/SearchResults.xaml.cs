using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using InternshipApp;
using InternshipApp.Resources;
using System.Net.NetworkInformation;
using TweetSharp;
using Windows.UI.Popups;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Parse;


namespace InternshipApp
{
    //TS = part of tweetsharp library
    public partial class SearchResults : PhoneApplicationPage
    {

        static IEnumerable<TweetSharp.TwitterStatus> results;//ienumerable holder 
        static string internship_information; //string holder for internship information
        static List<string> result_indexing = new List<string>();
        static int index;
        public SearchResults()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SearchResults_Loaded); //TS
            results = SearchPage.send_results(); //retrieve results from filtered search
        }

        public void SearchResults_Loaded(object sender, RoutedEventArgs e)
        {
            tweetList.ItemsSource = results;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string parameterValue = NavigationContext.QueryString["param"];

            if (parameterValue == "results")
            {
                results = SearchPage.send_results();
                //if there are no results, display error message
                if (results.Count() == 0)
                {
                    emptyResult.Text = "No results...";
                    emptyResult.Visibility = Visibility.Visible;

                }
            }
            if (parameterValue == "all")
            {
                results = MainPage.send_posts();
            }

        }

        private void internshipButton(object sender, RoutedEventArgs e)
        {
            result_indexing = results.Select(o => o.Text).ToList();
            internship_information = (sender as Button).Content.ToString(); //retrieve content from the items in the listbox
            index = result_indexing.IndexOf(internship_information);
            NavigationService.Navigate(new Uri("/Individual.xaml?param=Results", UriKind.Relative)); //navigate to information on individual internships
        }

        //returns information on the internship
        public static string send_internshipInformation()
        {
            return internship_information;
        }
        public static int send_index() 
        {
            return index;
        }
        public static IEnumerable<TweetSharp.TwitterStatus> send_results()
        {
            return results;
        }

        private async void Logout_Click(object sender, EventArgs e)
        {
            IEnumerable<TweetSharp.TwitterStatus> BookmarkList = Bookmarks.send_bookmarkList();
            List<string> str_books = new List<string>();
            str_books = BookmarkList.Select(o => o.Text).ToList();

            var user = ParseUser.CurrentUser;
            user["test"] = str_books;
            user.ACL = new ParseACL(ParseUser.CurrentUser);
            await user.SaveAsync();
            ParseUser.LogOut();

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

    }
}