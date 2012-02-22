using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using AdvNotes.Include;
using System.Windows.Navigation;
using System.IO.IsolatedStorage;

namespace AdvNotes.Pages
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            this.DataContext = new FontFamilyHelper();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //MessageBox.Show(Settings.EnableLocation.Value.ToString());

            //Get Font from name
            FontFamily tempFF = new FontFamily(Settings.FontFam.Value);

            // Show the saved settings
            LocationToggle.IsChecked = Settings.EnableLocation.Value;
            FontFamilyPicker.SelectedItem = tempFF;
            FontColorRectangle.Fill = new SolidColorBrush(Settings.FontColor.Value);
            BackgroundColorRectangle.Fill = new SolidColorBrush(Settings.BackgroundColor.Value);
            FontSizeSlider.Value = Settings.FontSize.Value;
            SampleBackground.Background = BackgroundColorRectangle.Fill;
            SampleText.Foreground = FontColorRectangle.Fill;
            SampleText.FontFamily = tempFF;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            IsolatedStorageSettings.ApplicationSettings.Save();        
        }

        private void ToggleSwitch_Changed(object sender, RoutedEventArgs e)
        {
            bool LocValue = LocationToggle.IsChecked.Value;
            Settings.EnableLocation.Value = LocValue;
            if (LocValue)
                LocationToggle.Content = "Enabled";
            else
                LocationToggle.Content = "Disabled";
        }


        private void FontFamilyPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Settings.FontFam.Value;           
            //MessageBox.Show(e.AddedItems.ToString());
            //FontFamilyPicker.SelectedItem = FontFamilyPicker.SelectedItem;
        }

        private void FontColorRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void BackgroundColorRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void FontSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Gets called during InitializeComponent
            if (FontSizeSlider != null)
            {
                int FontSize = (int)Math.Round(FontSizeSlider.Value);
                Settings.FontSize.Value = FontSize;
                SampleText.FontSize = FontSize;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            int FontSize = Settings.FontSize.DefaultValue;
            this.FontSizeSlider.Value = FontSize;
            this.SampleText.FontSize = FontSize;
        }
    }
}