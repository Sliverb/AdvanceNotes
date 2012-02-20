using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System;
using Microsoft.Phone.Tasks;

namespace AdvNotes
{
    public partial class DetailsPage : PhoneApplicationPage
    {

        // 0 -> Other ; 1 -> edit (Want to show keyboard in this state)
        int state = 0;
        string detailText = "";
        int noteIndex = 0;
        
        // Constructor
        public DetailsPage()
        {
            InitializeComponent();
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

            // Show keyboard if user was in edit state or new note added
            if (noteTextBlock.Text.Length == 0 || Settings.appState.Value == 1)
            {
                prepEdit();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (detailText != noteTextBlock.Text)
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