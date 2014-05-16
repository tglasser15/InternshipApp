using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Parse;

namespace InternshipApp
{
    public partial class Bookmarks : PhoneApplicationPage
    {
        static IEnumerable<TweetSharp.TwitterStatus> BookmarkList;  //container for all bookmarks
        static string internship_information;   //holds string text of the internship information within each item
        public Bookmarks()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Bookmarks_Loaded);
        }

        void Bookmarks_Loaded(object sender, RoutedEventArgs e)
        {
            //if the bookmarks page is the first visit, initialize with bookmarks from the main page
            if (Individual.send_bookmarks().Count == 0)
                BookmarkList = MainPage.send_bookmarks();
            else
                BookmarkList = Individual.send_bookmarks();

            //if BookmarkList is not null, proceed to data binding the BookmarkList
            if (BookmarkList != null)
            {
                BookmarkList = BookmarkList.Distinct().ToList(); //create distinct list
                tweetList.ItemsSource = BookmarkList;
            }
        }

        private void internshipButton(object sender, RoutedEventArgs e)
        {
            internship_information = (sender as Button).Content.ToString(); //retrieve content of item in listbox
            NavigationService.Navigate(new Uri("/Individual.xaml?param=Bookmark", UriKind.Relative)); //navigate to information on individual internships
        }

        //navigating between the three pages
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

        public static string send_internshipInformation()
        {
            return internship_information;
        }   //sends the internship information of the internship clicked on
        public static IEnumerable<TweetSharp.TwitterStatus> send_bookmarkList()
        {
            return BookmarkList;
        }   //send the BookmarkList

        
    }
}