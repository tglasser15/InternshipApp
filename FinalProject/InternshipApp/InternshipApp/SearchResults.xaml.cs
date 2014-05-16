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
    public partial class SearchResults : PhoneApplicationPage
    {
        static IEnumerable<TweetSharp.TwitterStatus> results;   //container for results from the search page
        static string internship_information; //string holder for internship information, ready to be sent for individual viewing
        static List<string> result_indexing = new List<string>();   //container to hold all the text of the results
        static int index;   //index used with result_indexing - determines potential bookmarking if user decides to

        public SearchResults()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SearchResults_Loaded); //TS
            results = SearchPage.send_results(); //retrieve results from filtered search
        }

        public void SearchResults_Loaded(object sender, RoutedEventArgs e)
        {
            tweetList.ItemsSource = results; //set data binding
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string parameterValue = NavigationContext.QueryString["param"];

            //if navigated from the search page
            if (parameterValue == "results")
            {
                results = SearchPage.send_results(); //retrieve filtered results
                //if there are no results, display error message
                if (results.Count() == 0)
                {
                    emptyResult.Text = "No results...";
                    emptyResult.Visibility = Visibility.Visible;

                }
            }

            //if navigated from saved searches
            if (parameterValue == "save_search")
            {
                results = SaveSearchPage.send_results();
                //if there are no results, display error message
                if (results.Count() == 0)
                {
                    emptyResult.Text = "No results...";
                    emptyResult.Visibility = Visibility.Visible;

                }
            }

            //if the user wishes to view all of the tweets
            if (parameterValue == "all")
            {
                results = MainPage.send_posts();
            }

        }

        private void internshipButton(object sender, RoutedEventArgs e)
        {
            result_indexing = results.Select(o => o.Text).ToList();     //retrieve all internships in text 
            internship_information = (sender as Button).Content.ToString(); //retrieve content from the items in the listbox
            index = result_indexing.IndexOf(internship_information);    //if internship information matches the text in the results_indexing, retrieve the index for that internship 
            NavigationService.Navigate(new Uri("/Individual.xaml?param=Results", UriKind.Relative)); //navigate to information on individual internships
        }

        //returns information on the internship
        public static string send_internshipInformation()
        {
            return internship_information;
        } //send internship information for individual viewing
        public static int send_index() 
        {
            return index;
        }   //send the index if the user decides to bookmark the individual internship
        public static IEnumerable<TweetSharp.TwitterStatus> send_results()
        {
            return results;
        }   //send the results so the use can bookmark the internship

    }
}