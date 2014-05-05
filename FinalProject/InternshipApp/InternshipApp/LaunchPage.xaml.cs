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
    public partial class LaunchPage : PhoneApplicationPage
    {
        public LaunchPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(LaunchPage_Loaded); //TS
            
        }

        private void LaunchPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}