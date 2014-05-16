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
    public partial class Individual : PhoneApplicationPage
    {
        IEnumerable<TweetSharp.TwitterStatus> results;  //container holding the results of the search
        static ObservableCollection<TweetSharp.TwitterStatus> bookmarks = new ObservableCollection<TweetSharp.TwitterStatus>(); //container for bookmarks
        int index;  //index used to determine which internship was clicked in order to bookmark the item

        public Individual()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(BookmarksPage_Loaded); 
        }

        private void BookmarksPage_Loaded(object sender, RoutedEventArgs e)
        {
            IEnumerable<TweetSharp.TwitterStatus> temp; //temporary container
            temp = MainPage.send_bookmarks();   //set the pre-initialized bookmarks from the mainpage to temp
            //if Parse did not return an empty array of bookmarks, add each bookmark to bookmarks to be viewed
            if (temp != null)
            {
                foreach (TweetSharp.TwitterStatus ts in temp)
                {
                    bookmarks.Add(ts);
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string parameterValue = NavigationContext.QueryString["param"];

            //if navigated from the results of the internship search
            if (parameterValue == "Results")
            {
                information.Text = SearchResults.send_internshipInformation(); //set the information to the internship clicked on
                results = SearchResults.send_results(); //retrieve the search results   
                index = SearchResults.send_index();     //retrieve the index of the internship clicked on for bookmarking   
                bookmark.IsEnabled = true;
            }
            if (parameterValue == "Bookmark")
            {
                bookmark.IsEnabled = false; //if we are just viewing the bookmark information, there is not need to bookmark it again
                information.Text = Bookmarks.send_internshipInformation(); //retrieve the information of the bookmark clicked on
            }
            
        }

        private void Bookmark_Clicked(object sender, RoutedEventArgs e)
        {
            var ts = results.ElementAt(index);
            bookmarks.Add(ts); //save the bookmark
        }

        public static ObservableCollection<TweetSharp.TwitterStatus> send_bookmarks()
        {
            return bookmarks;
        } //send the bookmarks for viewing
        public static void Bookmark_Clear()
        {
            bookmarks.Clear();
        } //upon each logout, we need to clear the Bookmark to ready for next user
    }
}