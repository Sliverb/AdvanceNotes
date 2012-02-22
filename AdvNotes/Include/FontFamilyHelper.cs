using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace AdvNotes.Include
{
    public class FontFamilyHelper : DependencyObject
    {
        public ObservableCollection<FontFamily> FontFamilyCollection
        {
            get { return (ObservableCollection<FontFamily>)GetValue(FontFamilyCollectionProperty); }
            set { SetValue(FontFamilyCollectionProperty, value); }
        }

        public static readonly DependencyProperty FontFamilyCollectionProperty =
            DependencyProperty.Register("FontFamilyCollection", typeof(ObservableCollection<FontFamily>), typeof(FontFamilyHelper), new PropertyMetadata(GetFontFamilies()));


        private static ObservableCollection<FontFamily> GetFontFamilies()
        {
            var fontFamilies = new ObservableCollection<FontFamily>();
            fontFamilies.Add(new FontFamily("Arial"));
            fontFamilies.Add(new FontFamily("Arial Black"));
            fontFamilies.Add(new FontFamily("Arial Bold"));
            fontFamilies.Add(new FontFamily("Arial Italic"));
            fontFamilies.Add(new FontFamily("Calibri"));
            fontFamilies.Add(new FontFamily("Calibri Bold"));            
            fontFamilies.Add(new FontFamily("Calibri Italic"));
            fontFamilies.Add(new FontFamily("Comic Sans MS"));
            fontFamilies.Add(new FontFamily("Comic Sans MS Bold"));
            fontFamilies.Add(new FontFamily("Courier New"));
            fontFamilies.Add(new FontFamily("Courier New Bold"));
            fontFamilies.Add(new FontFamily("Courier New Italic"));
            fontFamilies.Add(new FontFamily("Georgia"));
            fontFamilies.Add(new FontFamily("Georgia Bold"));
            fontFamilies.Add(new FontFamily("Georgia Italic"));
            fontFamilies.Add(new FontFamily("Lucida Sans Unicode"));
            fontFamilies.Add(new FontFamily("Malgun Gothic"));
            fontFamilies.Add(new FontFamily("Meiryo UI"));
            fontFamilies.Add(new FontFamily("Microsoft YaHei"));
            fontFamilies.Add(new FontFamily("Segoe UI"));
            fontFamilies.Add(new FontFamily("Segoe UI Bold"));
            fontFamilies.Add(new FontFamily("Segoe WP"));
            fontFamilies.Add(new FontFamily("Segoe WP Black"));
            fontFamilies.Add(new FontFamily("Segoe WP Bold"));
            fontFamilies.Add(new FontFamily("Segoe WP Light"));
            fontFamilies.Add(new FontFamily("Segoe WP Semibold"));
            fontFamilies.Add(new FontFamily("Segoe WP SemiLight"));
            fontFamilies.Add(new FontFamily("Tahoma"));
            fontFamilies.Add(new FontFamily("Tahoma Bold"));
            fontFamilies.Add(new FontFamily("Times New Roman"));
            fontFamilies.Add(new FontFamily("Times New Roman Bold"));
            fontFamilies.Add(new FontFamily("Times New Roman Italic"));
            fontFamilies.Add(new FontFamily("Trebuchet MS"));
            fontFamilies.Add(new FontFamily("Trebuchet MS Bold"));
            fontFamilies.Add(new FontFamily("Trebuchet MS Italic"));
            fontFamilies.Add(new FontFamily("Verdana"));
            fontFamilies.Add(new FontFamily("Verdana Bold"));
            fontFamilies.Add(new FontFamily("Verdana Italic"));
            fontFamilies.Add(new FontFamily("Webdings"));
            fontFamilies.Add(new FontFamily("Wingdings"));
            return fontFamilies;
        }
    }
}
