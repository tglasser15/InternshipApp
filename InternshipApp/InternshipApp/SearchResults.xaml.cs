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
        static string internship_information; //string holder for internship information
        static bool result_pressed = false; //buttons in result page have not been pressed 


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
            result_pressed = true; //a button in the results page has been pressed
            SearchPage.set_button_false(); //set buttons used in the search page to false
            internship_information = (sender as Button).Content.ToString(); //retrieve content from the items in the listbox
            NavigationService.Navigate(new Uri("/Individual.xaml", UriKind.Relative)); //navigate to information on individual internships
        }

        private void backtoSearch(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack(); //navigate back to search page
        }

        //returns information on the internship
        public static string send_internshipInformation()
        {
            return internship_information;
        }

        //sets the status of the buttons to false
        public static void set_button_false()
        {
            result_pressed = false;
        }

        //retrieves status of buttons
        public static bool return_button_status()
        {
            return result_pressed;
        }

    }
}