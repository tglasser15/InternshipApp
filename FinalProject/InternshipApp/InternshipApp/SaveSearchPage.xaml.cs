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
        static List<string> save_general = new List<string>();
        static List<string> save_major = new List<string>();
        static List<string> save_location = new List<string>();
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
            for (int i = 0; i < saved_searches.Count; i++)
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
            user["Save_General"] = save_general;
            user["Save_Major"] = save_major;
            user["Save_Location"] = save_location;
            user.ACL = new ParseACL(ParseUser.CurrentUser);
            try
            {
                await user.SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            ParseUser.LogOut();

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

    }
}