<h2>Internship Mobile App with Twitter API Integration</h2> <br>
<h3> User Notes</h3>
<i>This is a currently active project and will be a continued project as part of the Mobile App Development course I am currently taking. <b>Thus, some of the features of the project are unusable as of 3/28/14.</b> The project will be repeatedly updated over the course of the next two months, the ending of the course. <br>

For this project, I have only included Twitter API integration. <b>The results of the tweets shown in the application are random tweets I created on a "fake" account on twitter for testing purposes</b> I am waiting to speak with my school's Career Services to talk about posting internships in a way I can pull them onto the mobile app; my hope is to include more API's so I can pull from as many sources as I can.
<br>

<h3>Overview <br>

This application allows users to be able to efficiently search for internships either by a manual search or filter search by field of interest (major) or location interested in interning at. The main application page includes the following features:
<ul>
	<li><b>Search for internships</b> - allows user to search for internships using several filtering criteria based on field of interest or location. Recent internships will also be updated and be readied to viewed.</li>
    <li><b>Bookmarks</b> Allows user to view bookmarked internships</li>
    <li><b>Saved Searches</b> Allows user to save search results so that they may come back to the search in the future</li>
</ul> <br>
To see a gallery of screenshots and their descriptions, please click on the following link: <br> <a href="https://www.flickr.com/photos/tglasser15/sets/72157643076449664/">Internship App Photostream</a>

<h3>REQUIREMENTS </h3><br>
The following project requires the following software in order to use: <br>
<ul>
	<li>Visual Studio 2012
    <li><a href="http://www.microsoft.com/en-us/download/details.aspx?id=30668">Visual Studio 2012 SDK</a>
    <li><a href="http://msdn.microsoft.com/en-us/windows/desktop/bg162891.aspx">Windows Software Development Kit (SDK) for Windows 8.1</a>
</ul>


<h3>Getting Started</h3> <br>
<ul>
<li> Install Visual Studio 2012 and SDK components listed above</li>
<li><b>Retrieve API Keys</b> from <a href="https://dev.twitter.com/">Twitter Development</a> - you must create your own application to retrieve the keys needed for the following snippet of code:
<pre>
        //register application on https://dev.twitter.com/ to retrieve API keys below
        private string API_key = "Enter API Key Here";
        private string API_secret = "Enter API Secret Here";
        private string AccessToken = "Enter Access Token Here";
        private string AccessToken_secret = "Enter Access Token Secret Here";
</pre>
<li><b>Run the simulator</b>You should have access to all the features of the app, besides the pages that are "in progress."</li>
</ul>

