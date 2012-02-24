using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AdvNotes.Include
{
  public class DateConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
          DateTimeOffset date = (DateTimeOffset)value;
          // Return a custom format
          return date.LocalDateTime.ToLongDateString() + " " + date.LocalDateTime.ToShortTimeString();
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
          return DependencyProperty.UnsetValue;
      }
  }
}