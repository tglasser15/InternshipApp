﻿#pragma checksum "C:\Users\tglasser15\Documents\GitHub\InternshipApp\FinalProject\InternshipApp\InternshipApp\SaveSearchPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B83C440124F7FBB0EFFDB6861EC8F47E"
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
    
    
    public partial class SaveSearchPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.ListBox listbox;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/InternshipApp;component/SaveSearchPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.listbox = ((System.Windows.Controls.ListBox)(this.FindName("listbox")));
            this.search = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("search")));
            this.bookmarks = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("bookmarks")));
            this.savesearch = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("savesearch")));
        }
    }
}

