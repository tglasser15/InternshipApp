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
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string parameterValue = NavigationContext.QueryString["param"];

            if (parameterValue == "RecentInternships")
                information.Text = SearchPage.send_internshipInformation(); //retrieve the internship information for the specific internship button pressed
            if (parameterValue == "Results")
                information.Text = SearchResults.send_internshipInformation();
            
        }

        private void Bookmark_Clicked(object sender, RoutedEventArgs e)
        {

        }

    }
}