using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Parse;

namespace InternshipApp
{
    public partial class SaveSearchPage : PhoneApplicationPage
    {
        static List<SearchItem> saved_searches = new List<SearchItem>();
        public SaveSearchPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SaveSearchPage_Loaded);
        }

        void SaveSearchPage_Loaded(object sender, RoutedEventArgs e)
        {
            saved_searches = SearchPage.send_savedSearches();
            tweetList.ItemsSource = saved_searches;
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
        private async void Logout_Click(object sender, EventArgs e)
        {
            IEnumerable<TweetSharp.TwitterStatus> BookmarkList = Bookmarks.send_bookmarkList();
            List<string> str_books = new List<string>();
            str_books = BookmarkList.Select(o => o.Text).ToList();
            var user = ParseUser.CurrentUser;
            user["Bookmarks"] = str_books;
            await user.SaveAsync();
            ParseUser.LogOut();

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

    }
}