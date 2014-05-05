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

namespace InternshipApp
{
    public partial class Individual : PhoneApplicationPage
    {
        IEnumerable<TweetSharp.TwitterStatus> results;
        static ObservableCollection<TweetSharp.TwitterStatus> bookmarks = new ObservableCollection<TweetSharp.TwitterStatus>();
        int index;
        public Individual()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(BookmarksPage_Loaded); //TS
            
        }

        private void BookmarksPage_Loaded(object sender, RoutedEventArgs e)
        {
            bookmark.IsEnabled = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string parameterValue = NavigationContext.QueryString["param"];

            if (parameterValue == "RecentInternships")
            {
                information.Text = SearchPage.send_internshipInformation(); //retrieve the internship information for the specific internship button pressed
                results = SearchPage.send_results();
                index = SearchPage.send_index();
            }
            if (parameterValue == "Results")
            {
                information.Text = SearchResults.send_internshipInformation();
                results = SearchResults.send_results();
                index = SearchResults.send_index();
            }
            if (parameterValue == "Bookmark")
            {
                bookmark.IsEnabled = false;
                information.Text = Bookmarks.send_internshipInformation();
            }
            
        }

        private void Bookmark_Clicked(object sender, RoutedEventArgs e)
        {
            var ts = results.ElementAt(index);
            bookmarks.Add(ts);
        }

        public static ObservableCollection<TweetSharp.TwitterStatus> send_bookmarks()
        {
            return bookmarks;
        }

    }
}