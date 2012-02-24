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
        bool eventOK = false;

        public SettingsPage()
        {
            InitializeComponent();
            this.DataContext = new FontFamilyHelper();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

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

            // Ok to activate event listeners
            eventOK = true;
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
            if (e.RemovedItems != null && e.RemovedItems.Count > 0)
            {
                if (FontFamilyPicker.SelectedItem != null)
                {
                    if (eventOK)
                    {
                        Settings.FontFam.Value = FontFamilyPicker.SelectedItem.ToString();
                    }
                }

            }
        }

        private void FontColorRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Get a string representation of the colors, without the leading #
            string currentColorString = Settings.FontColor.Value.ToString().Substring(1);
            string defaultColorString = Settings.FontColor.DefaultValue.ToString().Substring(1);

            // Navigate to the color picker
            NavigationService.Navigate(new Uri(
              "/Pages/ColorPickerPage.xaml?"
              + "showOpacity=false"
              + "&currentColor=" + currentColorString
              + "&defaultColor=" + defaultColorString
              + "&settingName=FontColor", UriKind.Relative));
        }

        private void BackgroundColorRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Get a string representation of the colors, without the leading #
            string currentColorString = Settings.BackgroundColor.Value.ToString().Substring(1);
            string defaultColorString = Settings.BackgroundColor.DefaultValue.ToString().Substring(1);

            // Navigate to the color picker
            this.NavigationService.Navigate(new Uri(
              "/Pages/ColorPickerPage.xaml?"
              + "&currentColor=" + currentColorString
              + "&defaultColor=" + defaultColorString
              + "&settingName=BackgroundColor", UriKind.Relative));
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