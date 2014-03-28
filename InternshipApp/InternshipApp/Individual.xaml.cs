using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace InternshipApp
{
    public partial class Individual : PhoneApplicationPage
    {
        public Individual()
        {
            InitializeComponent();
            bool fromSearchPage = SearchPage.return_button_status(); //retrieve status of the buttons in the recent internship portion of the search page
            bool fromSearchResults = SearchResults.return_button_status(); //retrieve status of buttons in the results page

            //check to see which button on which page was pressed
            if (fromSearchPage)
                information.Text = SearchPage.send_internshipInformation(); //retrieve the internship information for the specific internship button pressed
            if (fromSearchResults)
                information.Text = SearchResults.send_internshipInformation();
        }

        private void backtoSearch(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack(); //navigate back to search page
        }
    }
}