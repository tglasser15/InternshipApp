﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Parse;
using System.Windows.Data;

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
            if (SearchPage.send_savedSearches().Count == 0)
                saved_searches = MainPage.send_savedSearches();
            else
                saved_searches = SearchPage.send_savedSearches();

            saved_searches = saved_searches.Distinct().ToList();
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
        public static void saves_clear()
        {
            saved_searches.Clear();
        }
        public void save_Click(object sender, EventArgs e)
        {
            var box = (sender as Button);
            box.GetBindingExpression(Button.).UpdateSource(); 
            //result_indexing = results.Select(o => o.Text).ToList();
            //internship_information = (sender as Button).Content.ToString(); //retrieve content from the items in the listbox
            //index = result_indexing.IndexOf(internship_information);
            //NavigationService.Navigate(new Uri("/Individual.xaml?param=Results", UriKind.Relative)); //navigate to information on individual 
        }

        

    }
}