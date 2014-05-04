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


namespace InternshipApp
{
    //TS = part of tweetsharp library
    public partial class SearchResults : PhoneApplicationPage
    {

        IEnumerable<TweetSharp.TwitterStatus> results; //ienumerable holder 
        //static List<string> results;
        static string internship_information; //string holder for internship information

        public SearchResults()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SearchResults_Loaded); //TS
            results = SearchPage.send_results(); //retrieve results from filtered search

            //if there are not results, display error message
            if (results.Count() == 0)
            {
                emptyResult.Text = "No results...";
                emptyResult.Visibility = Visibility.Visible;

            }
        }

        void SearchResults_Loaded(object sender, RoutedEventArgs e)
        {
            tweetList.ItemsSource = results; //set listbox to items in result
        }

        private void internshipButton(object sender, RoutedEventArgs e)
        {
            internship_information = (sender as Button).Content.ToString(); //retrieve content from the items in the listbox
            NavigationService.Navigate(new Uri("/Individual.xaml?param=Results", UriKind.Relative)); //navigate to information on individual internships
        }

        //private void item_Tapped(object sender, RoutedEventArgs e)
        //{
        //    internship_information = (sender as TextBlock).Text;
        //    NavigationService.Navigate(new Uri("/Individual.xaml?param=Results", UriKind.Relative));
        //}

        //returns information on the internship
        public static string send_internshipInformation()
        {
            return internship_information;
        }



    }
}