// Copyright (c) Adam Nathan.  All rights reserved.
//
// By purchasing the book that this source code belongs to, you may use and
// modify this code for commercial and non-commercial applications, but you
// may not publish the source code.
// THE SOURCE CODE IS PROVIDED "AS IS", WITH NO WARRANTIES OR INDEMNITIES.
using System;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using AdvNotes.Include;

namespace AdvNotes.Pages
{
    public partial class ColorPickerPage : PhoneApplicationPage
    {
        HSVColor currentColor;
        HSVColor defaultColor;
        bool keyPressed;
        bool showOpacity = true;
        string settingName;

        public ColorPickerPage()
        {
            InitializeComponent();

            this.SaturationValueMap.MouseMove += SatValMove;
            this.HueSlider.MouseMove += HueMove;
            this.AlphaSlider.MouseMove += AlphaMove;
            this.ColorTextBox.TextChanged += ColorTextBox_TextChanged;
            this.ColorTextBox.KeyDown += ColorTextBox_KeyDown;
            this.ResetButton.Click += ResetButton_Click;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            this.State["CurrentColor"] = this.currentColor;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (this.State.ContainsKey("CurrentColor"))
            {
                SetCurrentColor((HSVColor)this.State["CurrentColor"]);
            }
            else if (NavigationContext.QueryString.ContainsKey("currentColor"))
            {
                Color color;
                if (TryGetColorFromText("#" + NavigationContext.QueryString["currentColor"], out color))
                    SetCurrentColor(ColorConverter.RGBToHSV(color));
            }

            if (NavigationContext.QueryString.ContainsKey("defaultColor"))
            {
                Color color;
                if (TryGetColorFromText("#" + NavigationContext.QueryString["defaultColor"], out color))
                    defaultColor = ColorConverter.RGBToHSV(color);

                if (defaultColor == currentColor)
                {
                    this.Focus(); // So focus doesn't go to TextBox and bring up keyboard
                    this.ResetButton.IsEnabled = false;
                }
            }

            if (NavigationContext.QueryString.ContainsKey("settingName"))
                settingName = NavigationContext.QueryString["settingName"];

            if (NavigationContext.QueryString.ContainsKey("showOpacity"))
            {
                bool newValue = Convert.ToBoolean(NavigationContext.QueryString["showOpacity"]);

                // Only do this if switching from showing to not showing, to guard against multiple navigations to this page
                // (for example, from locking/unlocking the phone)
                if (showOpacity && !newValue)
                {
                    // Hide options for non-opaque colors
                    AlphaPanel.Visibility = Visibility.Collapsed;
                    TransparentPresetColor.Visibility = Visibility.Collapsed;

                    // Spread out remaining UI
                    Canvas.SetTop(ColorPresetsPanel, Canvas.GetTop(ColorPresetsPanel) + 14);
                    Canvas.SetTop(SaturationValuePanel, Canvas.GetTop(SaturationValuePanel) + 30);
                    Canvas.SetTop(HuePanel, Canvas.GetTop(HuePanel) + 50);
                }
                showOpacity = newValue;
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (settingName.Equals("FontColor"))
            {
                Settings.FontColor.Value = ColorConverter.HSVToRGB(this.currentColor);
            }
            else if (settingName.Equals("BackgroundColor"))
            {
                Settings.BackgroundColor.Value = ColorConverter.HSVToRGB(this.currentColor);
            }
            //IsolatedStorageSettings.ApplicationSettings[settingName] = ColorConverter.HSVToRGB(this.currentColor);
            base.OnBackKeyPress(e);
        }

        // Could have been a dependency property that pieces of the UI bind to, but the interaction is complex enough
        // to make this the easier route:
        public void SetCurrentColor(HSVColor value)
        {
            this.currentColor = value;
            Color rgbColor = ColorConverter.HSVToRGB(value);

            this.ResetButton.IsEnabled = true;

            this.PreviewTile.Fill = new SolidColorBrush(rgbColor);

            // Only update the TextBox if this isn't being done in response to typing.  That prevents
            // text like "red" from changing to "#FF0000" and prevents the cursor from jumping to the beginning.
            if (!keyPressed)
                this.ColorTextBox.Text = value.ToString();

            this.HueGradientStop.Color = ColorConverter.HSVToRGB(new HSVColor { A = 255, H = value.H, S = 1, V = 1 });
            Canvas.SetLeft(this.HueThumb, (value.H / 360) * this.HueSlider.Width - this.HueThumb.Width / 2);

            // Update AlphaSlider and AlphaThumb:
            Color opaqueColor = rgbColor;
            opaqueColor.A = 255;
            Color transparentColor = rgbColor;
            transparentColor.A = 0;
            this.alphaGradientStop.Color = opaqueColor;
            this.alphaGradientStop2.Color = transparentColor;
            Canvas.SetLeft(this.AlphaThumb, ((double)value.A / 255) * this.AlphaSlider.Width - this.AlphaThumb.Width / 2);

            // Update SaturationValueMap:
            Point saturationValuePoint = new Point(value.S * this.SaturationValueMap.Width, value.V * this.SaturationValueMap.Height);
            this.SelectionLineX1.X1 = this.SelectionLineX1.X2 = saturationValuePoint.X;
            this.SelectionLineX2.X1 = this.SelectionLineX2.X2 = saturationValuePoint.X;
            this.SelectionLineY1.Y1 = this.SelectionLineY1.Y2 = saturationValuePoint.Y;
            this.SelectionLineY2.Y1 = this.SelectionLineY2.Y2 = saturationValuePoint.Y;
        }

        private void ColorPreset_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentColor(ColorConverter.RGBToHSV(((sender as Button).Background as SolidColorBrush).Color));
        }

        void ColorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Don't do anything if this TextChanged event is happening from the CurrentColor changing.
            if (!keyPressed)
                return;

            Color color;
            if (this.TryGetColorFromText(ColorTextBox.Text, out color))
                SetCurrentColor(ColorConverter.RGBToHSV(color));

            keyPressed = false;
        }

        void ColorTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // So pressing the Enter key dismisses the keyboard:
                this.Focus();
                return;
            }
            keyPressed = true;
        }

        void SatValMove(object sender, MouseEventArgs e)
        {
            keyPressed = false;
            SetCurrentColor(new HSVColor
                {
                    A = currentColor.A,
                    H = currentColor.H,
                    S = e.GetPosition(this.SaturationValueMap).X / this.SaturationValueMap.Width,
                    V = e.GetPosition(this.SaturationValueMap).Y / this.SaturationValueMap.Width
                }
            );
        }

        void HueMove(object sender, MouseEventArgs e)
        {
            keyPressed = false;
            double x = e.GetPosition(this.HueSlider).X;

            // The last value is the same as the first value, so prevent the thumb from jumping
            // to the beginning by tapping on the very end:
            if (x == this.HueSlider.Width)
                x--;

            double hue = Math.Round((x / this.HueSlider.Width) * 360);
            HSVColor newColor = currentColor;
            newColor.H = hue;
            SetCurrentColor(newColor);
        }

        void AlphaMove(object sender, MouseEventArgs e)
        {
            keyPressed = false;
            double x = e.GetPosition(this.AlphaSlider).X;
            HSVColor newColor = currentColor;
            newColor.A = (byte)Math.Round((x / this.AlphaSlider.Width) * 255);
            SetCurrentColor(newColor);
        }

        void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            keyPressed = false;
            SetCurrentColor(this.defaultColor);
            // Move focus to a different Button so it doesn't go to TextBox and bring up the keyboard:
            FirstPresetButton.Focus();
            this.ResetButton.IsEnabled = false;
        }

        bool TryGetColorFromText(string text, out Color color)
        {
            try
            {
                // Leverage Silverlight's XAML parsing to convert text like "red" or "#ff00" into a Color
                Rectangle rect = XamlReader.Load("<Rectangle xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Fill='" + text + "'/>") as Rectangle;
                color = (rect.Fill as SolidColorBrush).Color;

                if (!showOpacity)
                {
                    // Don't allow non-opaque colors:
                    color.A = 255;
                }

                return true;
            }
            catch
            {
                color = Colors.Black;
                return false;
            }
        }
    }
}