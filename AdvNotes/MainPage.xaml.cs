using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace AdvNotes
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        // Handle selection changed on ListBox
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (MainListBox.SelectedIndex == -1)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/Pages/DetailsPage.xaml?selectedItem=" + MainListBox.SelectedIndex, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (DataContext == null)
                DataContext = Settings.NotesList.Value;

            // Show some context when no notes available
            if (Settings.NotesList.Value.Count == 0)
                EmptyListBlock.Visibility = System.Windows.Visibility.Visible;
            else
                EmptyListBlock.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void appBar_new(object sender, EventArgs e)
        {
            // Create a new note and add it to the top of the list
            // Later on sort by last modified
            Notes newNote = new Notes();
            newNote.Filename = Guid.NewGuid().ToString();
            newNote.Modified = DateTimeOffset.Now;
            Settings.NotesList.Value.Insert(0, newNote);

            // Navigate to details page URI with the new note
            NavigationService.Navigate(new Uri("/Pages/DetailsPage.xaml?selectedItem=0", UriKind.Relative));
        }

        private void appBar_help(object sender, EventArgs e)
        {
            MessageBox.Show("Help page on Route");
            //NavigationService.Navigate(new Uri("/HelpPage.xaml?", UriKind.Relative));
        }

        private void appBar_setting(object sender, EventArgs e)
        {
            //MessageBox.Show("Settings page on Route");
            NavigationService.Navigate(new Uri("/Pages/SettingsPage.xaml", UriKind.Relative));
        }

        private void appBar_about(object sender, EventArgs e)
        {
            MessageBox.Show("About page on Route");
        }

    }
}