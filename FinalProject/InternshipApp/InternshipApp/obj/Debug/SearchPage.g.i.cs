﻿#pragma checksum "C:\Users\kglynn16\Documents\GitHub\InternshipApp\FinalProject\InternshipApp\InternshipApp\SearchPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E9E0D5ABBE5E8F6B8FF13BAB3A8A73D8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace InternshipApp {
    
    
    public partial class SearchPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox SearchBar;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Button buttonSearch;
        
        internal System.Windows.Controls.TextBox LocationSearch;
        
        internal System.Windows.Controls.Button buttonSeeAll;
        
        internal System.Windows.Controls.TextBlock errorSearch;
        
        internal Microsoft.Phone.Controls.ListPicker OptionSelector;
        
        internal Microsoft.Phone.Controls.ListPickerItem Option1;
        
        internal Microsoft.Phone.Controls.ListPickerItem Option2;
        
        internal Microsoft.Phone.Controls.ListPickerItem Option3;
        
        internal Microsoft.Phone.Controls.ListPickerItem Option4;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton search;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton bookmarks;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton savesearch;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/InternshipApp;component/SearchPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.SearchBar = ((System.Windows.Controls.TextBox)(this.FindName("SearchBar")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.buttonSearch = ((System.Windows.Controls.Button)(this.FindName("buttonSearch")));
            this.LocationSearch = ((System.Windows.Controls.TextBox)(this.FindName("LocationSearch")));
            this.buttonSeeAll = ((System.Windows.Controls.Button)(this.FindName("buttonSeeAll")));
            this.errorSearch = ((System.Windows.Controls.TextBlock)(this.FindName("errorSearch")));
            this.OptionSelector = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("OptionSelector")));
            this.Option1 = ((Microsoft.Phone.Controls.ListPickerItem)(this.FindName("Option1")));
            this.Option2 = ((Microsoft.Phone.Controls.ListPickerItem)(this.FindName("Option2")));
            this.Option3 = ((Microsoft.Phone.Controls.ListPickerItem)(this.FindName("Option3")));
            this.Option4 = ((Microsoft.Phone.Controls.ListPickerItem)(this.FindName("Option4")));
            this.search = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("search")));
            this.bookmarks = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("bookmarks")));
            this.savesearch = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("savesearch")));
        }
    }
}

