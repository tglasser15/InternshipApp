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
        private void Logout_Click(object sender, EventArgs e)
        {
            var user = new ParseObject("User");
            //user["Bookmarks"] = bookmarks;
            user.ACL = new ParseACL(ParseUser.CurrentUser);
            user.SaveAsync();
            ParseUser.LogOut();
        }
    }
}