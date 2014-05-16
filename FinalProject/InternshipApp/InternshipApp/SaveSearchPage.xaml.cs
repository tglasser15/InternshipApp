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
using System.Windows.Data;
using System.Collections.ObjectModel;



namespace InternshipApp
{
    public partial class SaveSearchPage : PhoneApplicationPage
    {
        static List<SearchItem> saved_searches = new List<SearchItem>(); //contains all of the filters from search  
        static IEnumerable<TweetSharp.TwitterStatus> results; //container for results - mainly used for seaching

        public SaveSearchPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SaveSearchPage_Loaded);
        }

        void SaveSearchPage_Loaded(object sender, RoutedEventArgs e)
        {
            results = MainPage.send_posts();

            //if first visit
            if (SearchPage.send_savedSearches().Count == 0)
                saved_searches = MainPage.send_savedSearches();
            else
                saved_searches = SearchPage.send_savedSearches();

            saved_searches = saved_searches.Distinct().ToList(); //create distinct list

            //create new listbox items
            for (int i = 0; i < saved_searches.Count; i++)
            {
                ListBoxItem li = new ListBoxItem();
                string str =  "General : " + saved_searches[i].GeneralSearch + "\n"
                            + "Major   : " + saved_searches[i].DisciplineSearch + "\n"
                            + "Location: " + saved_searches[i].Location + "\n";
                li.Content = str;
                listbox.Items.Add(li);
            }
        }

        //iterate through main pages
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
        
        //if user clicks on a saved search, user is able to view results from filters
        public void save_Click(object sender, EventArgs e)
        {
            int index = (sender as ListBox).SelectedIndex; //get index of listbox item
            var search = saved_searches[index]; //get matching search from saved_searches list


            List<string> holder = new List<string>() //temporary holder for keys
            {
                search.GeneralSearch, search.DisciplineSearch, search.Location
            };
            List<string> keys = new List<string>();
            
            //exclude any empty string literals
            foreach (string s in holder)
            {
                if (s != "")
                    keys.Add(s);
            }

            //see comments from SearchPage.xaml

            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string[] words = search.GeneralSearch.Split(delimiterChars);
            string[] words2 = search.DisciplineSearch.Split(delimiterChars);
            string[] words3 = search.Location.Split(delimiterChars);
            foreach (string s in words)
            {
                if (s != "" && s.Count() != 1)
                    keys.Add(s);
            }
            foreach (string s in words2)
            {
                if (s != "" && s.Count() != 1)
                    keys.Add(s);
            }
            foreach (string s in words3)
            {
                if (s != "" && s.Count() != 1)
                    keys.Add(s);
            }

            ObservableCollection<TweetSharp.TwitterStatus> queue = new ObservableCollection<TweetSharp.TwitterStatus>();
            IEnumerable<TweetSharp.TwitterStatus> temp;
            foreach (string k in keys)
            {
                temp = results.Where(o => o.Text.ToUpper().Contains(k)).ToList();
                foreach (TweetSharp.TwitterStatus ts in temp)
                {
                    queue.Add(ts);
                }
            }

            results = queue.Distinct().ToList();

            NavigationService.Navigate(new Uri("/SearchResults.xaml?param=save_search", UriKind.Relative));
        }

        public static IEnumerable<TweetSharp.TwitterStatus> send_results()
        {
            return results;
        } //send results filtered by search criteria to be viewed. 

        public static void saves_clear()
        {
            saved_searches.Clear();
        } //clear the saved_searches upon logout

    }
}