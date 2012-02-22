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
using AdvNotes.Include;

namespace AdvNotes.Pages
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            this.DataContext = new FontFamilyHelper();
        }

        private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void FontColorRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void BackgroundColorRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void FontSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}