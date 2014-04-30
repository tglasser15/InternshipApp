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
    public partial class AppMainPage : PhoneApplicationPage
    {
        public AppMainPage()
        {
            InitializeComponent();
        }

        //create a navigation for each of the button clicks
        private void buttonLogout_Click(object sender, RoutedEventArgs e)
        {
            ParseUser.LogOut();
            var currentUser = ParseUser.CurrentUser; //current user will be null
            NavigationService.GoBack(); //navigates back to main screen
        }

        private void searchClick_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative)); //navigates to search page
        }

        private void bookmarkClick_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Bookmarks.xaml", UriKind.Relative)); //navigates to bookmarks page
        }

        private void savedsearchClick_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SaveSearchPage.xaml", UriKind.Relative)); //navigates to saved searches page
        }

    }
}