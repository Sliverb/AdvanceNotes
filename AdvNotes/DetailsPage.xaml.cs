using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System;
using Microsoft.Phone.Tasks;
using System.Device.Location;

namespace AdvNotes
{
    public partial class DetailsPage : PhoneApplicationPage
    {

        // 0 -> Other ; 1 -> edit (Want to show keyboard in this state)
        int state = 0;
        // Text extracted from note object
        string detailText = "";
        // Location placeholder
        string location = "";
        // Index for pulling the right note object
        int noteIndex = 0;
        // Back button pressed boolean for auto deleting empty notes.
        bool backPress = false;
        
        // Constructor
        public DetailsPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Show keyboard if new note added
            if (noteTextBlock.Text.Length == 0)         
                noteTextBlock.Focus();

            // Get location data only if new note
            if (noteIndex == 0)
            {
                // When the page is instantiated, we'll begin the process
                // of acquiring the physical location of the user to 
                // add to the note's file name.  This is async, requiring
                // us to implement a __Completed event.
                try
                {
                    GeoCoordinateWatcher myWatcher = new GeoCoordinateWatcher();

                    var myPosition = myWatcher.Position;

                    // In this case,since we're not working with a device, I'll just
                    // set a default value.  If we cannot get the current location,
                    // then we'll default to Redmond, WA.
                    double latitude = 47.674;
                    double longitude = -122.12;

                    if (!myPosition.Location.IsUnknown)
                    {
                        latitude = myPosition.Location.Latitude;
                        longitude = myPosition.Location.Longitude;
                    }

                    myTerraService.TerraServiceSoapClient client = new myTerraService.TerraServiceSoapClient();

                    client.ConvertLonLatPtToNearestPlaceCompleted += new EventHandler<myTerraService.ConvertLonLatPtToNearestPlaceCompletedEventArgs>(client_ConvertLonLatPtToNearestPlaceCompleted);

                    client.ConvertLonLatPtToNearestPlaceAsync(new myTerraService.LonLatPt() { Lat = latitude, Lon = longitude });
                }
                catch
                {
                    // Ignore ... for now
                }
            }
        }

        void client_ConvertLonLatPtToNearestPlaceCompleted(object sender, myTerraService.ConvertLonLatPtToNearestPlaceCompletedEventArgs e)
        {
            // This async event occurs when we successfully get a
            // response from calling the TerraService web service.
            // This event handler is attached in the Add() constructor.
            location = e.Result;
            PageTitle.Text = location;
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Get Note Index from URI
            string selectedIndex = "";
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            {
                noteIndex = int.Parse(selectedIndex);
            }

            // Show the note's content
            Notes n = Settings.NotesList.Value[noteIndex];
            if (n != null)
            {
                detailText = noteTextBlock.Text = n.GetContent();
            }
            
            // Show keyboard if user was in edit state
            if (Settings.appState.Value == 1)
                prepEdit();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if ((detailText != noteTextBlock.Text))// || ((noteTextBlock.Text.Length ==0) && !backPress))
            {
                // Automatically save the new content
                Notes n = Settings.NotesList.Value[noteIndex];

                n.SaveContent(noteTextBlock.Text);

                // Update the title now, so each one can be accessed
                // later without reading the file's contents
                string title = noteTextBlock.Text.TrimStart();

                // Don't include more than the first 100 characters, which should be long
                // enough, even in landscape with a small font
                if (title.Length > 50)
                    title = title.Substring(0, 50);

                // Fold the remaining content into a single line. We can't use
                // Environment.NewLine because it's \r\n, whereas newlines inserted from
                // a text box are just \r
                n.Title = title.Replace('\r', ' ');

                n.Modified = DateTimeOffset.Now;
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            backPress = true;
            /*
            if (noteTextBlock.Text.Length == 0)
            {
                Notes n = Settings.NotesList.Value[noteIndex];
                n.DeleteContent();
                Settings.NotesList.Value.Remove(n);
            } 
             */
        }

        // Event Handlers
       
        #region Event Handlers Sections

        private void noteTextBlock_Changed(object sender, TextChangedEventArgs e)
        {
            scrollJump();
        }

        private void noteTextBlock_Focus(object sender, RoutedEventArgs e)
        {
            updateState(1);
            ApplicationBar.IsVisible = false;
        }


        private void noteTextBlock_unFocus(object sender, RoutedEventArgs e)
        {
            updateState(0);
            ApplicationBar.IsVisible = true;
        }

        // App bar event handlers
        private void appBar_Email(object sender, System.EventArgs e)
        {
            EmailComposeTask launcher = new EmailComposeTask();
            launcher.Body = noteTextBlock.Text;
            launcher.Subject = "Note";
            launcher.Show();
        }

        private void appBar_Delete(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this note?",
                "Delete note?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                noteTextBlock.Text = "";
                Notes n = Settings.NotesList.Value[noteIndex];
                n.DeleteContent();
                Settings.NotesList.Value.Remove(n);

                if (this.NavigationService.CanGoBack)
                    this.NavigationService.GoBack();
            }
        }

        #endregion

        // Helper Functions

        # region Helpers

        public void scrollJump()
        {
            noteScrollViewer.ScrollToVerticalOffset(noteTextBlock.ActualHeight);
        }

        private void updateState(int stateNum)
        {
            state = stateNum;
            Settings.appState.Value = stateNum;
        }

        private void prepEdit()
        {
            noteTextBlock.Focus();
            noteTextBlock.Select(noteTextBlock.Text.Length, 0);
            scrollJump();
        }

        # endregion
    }
}