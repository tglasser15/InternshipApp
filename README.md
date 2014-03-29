<h2>Windows Phone Internship App in C# with Twitter API Integration</h2>
<h3>Project Notes</h3>
<i>This is a currently active project and will be a continued project as part of the Mobile App Development course I am currently taking. <b>Thus, some of the features of the project are unusable as of 3/28/14.</b> The project will be repeatedly updated over the course of the next two months which is the ending of the course. <br>

For this project, I have only included Twitter API integration. <b>The results of the tweets shown in the application are random tweets I created on a "fake" account on twitter for testing purposes</b> I am waiting to get in touch with Whitworth Univeresity's Career Services to talk about posting internships in a way I can pull them onto the mobile app; my hope is to include more API's so I can pull from as many sources as I can to be readily available for students.<br>

The Twitter API Integration works as follows:
<pre>
void SearchPage_Loaded(object sender, RoutedEventArgs e)
        {

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                //validate API keys
                var service = new TwitterService(API_key, API_secret );
                service.AuthenticateWith(AccessToken, AccessToken_secret);

                //ScreenName is the profile name of the twitter user.
                service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions() { ScreenName = "Internship_DNF" }, (ts, rep) => //ts = twitter feeds
                {
                    if (rep.StatusCode == HttpStatusCode.OK)
                    {
                        //bind
                        //tweetList.ItemsSource is the listbox that tweets will be 	set to as it updates
                        this.Dispatcher.BeginInvoke(() => { tweetList.ItemsSource = ts; });
                        results = ts; //set twitter feeds to holder since ts is a local variable
                    }
                });
            }
            else
            {

                MessageBox.Show("Please check your internet connection.");
            }

        }
</pre>
An example of how I am using this is given below.

<h3>Overview</h3>

This application allows users to be able to efficiently search for internships either by a manual search or filter search by field of interest (major) or location interested in interning at. The main application page includes the following features:
<ul>
	<li><b>Search for internships</b> - allows user to search for internships using several filtering criteria based on field of interest or location. Recent internships will also be updated and be readied to viewed.</li>
    <li><b>Bookmarks</b> Allows user to view bookmarked internships</li>
    <li><b>Saved Searches</b> Allows user to save search results so that they may come back to the search in the future</li>
</ul> <br>
To see a gallery of screenshots and their descriptions, please click on the following link: <br> <a href="https://www.flickr.com/photos/tglasser15/sets/72157643076449664/">Internship App Photostream</a><br>
<br> 
For all other information, see the folder <a href="https://github.com/tglasser15/InternshipApp/tree/master/InternshipApp">InternshipApp</a>.

<h3>REQUIREMENTS </h3>
The following project requires the following software in order to use: <br>
<ul>
	<li>Visual Studio 2012</li>
    <li>Windows 8.1</li>
    <li><a href="http://www.microsoft.com/en-us/download/details.aspx?id=30668">Visual Studio 2012 SDK</a></li>
    <li><a href="http://msdn.microsoft.com/en-us/windows/desktop/bg162891.aspx">Windows Software Development Kit (SDK) for Windows 8.1</a></li>
</ul>


<h3>Getting Started</h3>
<ul>
<li> Install Visual Studio 2012 and SDK components listed above</li>
<li><b>Download the .zip file</b> - This folder contains the project.</li>
<li><b>Retrieve API Keys</b> from <a href="https://dev.twitter.com/">Twitter Development</a> - you must create your own application to retrieve the keys needed for the following snippet of code:
<pre>
        //register application on https://dev.twitter.com/ to retrieve API keys below
        private string API_key = "Enter API Key Here";
        private string API_secret = "Enter API Secret Here";
        private string AccessToken = "Enter Access Token Here";
        private string AccessToken_secret = "Enter Access Token Secret Here";
</pre>
<li><b>Run the simulator</b>- You should have access to all the features of the app, besides the pages that are "in progress."</li>
</ul>

<h3>Example</h3>
Let's say I wanted to look for an internship about "computer science". When I enter this into the search bar text field, the textfield SearchBar.Text will be initialized with "computer science". Once the space bar is pressed, I arrive at the following snipper of code:
<pre>
//filter results based on what is typed in the search bar
                    if (SearchBar.Text != defaultSearch)
                        results = results.Where(o => o.Text.ToUpper().Contains(SearchBar.Text)).ToArray();
</pre>
From the API integration snippet above, the variable results is initialized with the tweets. results will only be set equal to the tweets that are associated with "computer science". The listbox in the results page is then initialized...
<pre>
void SearchResults_Loaded(object sender, RoutedEventArgs e)
        {
            tweetList.ItemsSource = results; //set listbox to items in result
        }
</pre>
and ready to be accessed for information:
<pre>
private void internshipButton(object sender, RoutedEventArgs e)
        {
            result_pressed = true; //a button in the results page has been pressed
            SearchPage.set_button_false(); //set buttons used in the search page to false
            internship_information = (sender as Button).Content.ToString(); //retrieve content from the items in the listbox
            NavigationService.Navigate(new Uri("/Individual.xaml", UriKind.Relative)); //navigate to information on individual internships
        }
</pre>
The following is the result:
<pre>
private void internshipButton(object sender, RoutedEventArgs e)
        {
            result_pressed = true; //a button in the results page has been pressed
            SearchPage.set_button_false(); //set buttons used in the search page to false
            internship_information = (sender as Button).Content.ToString(); //retrieve content from the items in the listbox
            //internship_information = 
            //	computer science internship
            //  #WhitworthInternships
            NavigationService.Navigate(new Uri("/Individual.xaml", UriKind.Relative)); //navigate to information on individual internships
        }
</pre>