// Copyright (c) Adam Nathan.  All rights reserved.
// Modified by Bayo Olatunji.
using System.Windows.Media;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls;
using AdvNotes.Include;

namespace AdvNotes.Include
{
    public static class Settings
    {
        // The App data
        public static readonly Setting<ObservableCollection<Notes>> NotesList =
            new Setting<ObservableCollection<Notes>>("NotesList", new ObservableCollection<Notes>());

        // Temporary state
        public static readonly Setting<int> CurrentNoteIndex =
            new Setting<int>("CurrentNoteIndex", -1);
        public static readonly Setting<int> appState =
            new Setting<int>("MyState", 0);
        public static readonly Setting<int> EmptyNoteIndex =
            new Setting<int>("EmptyNoteIndex", -1);

        // User settings
        public static readonly Setting<bool> EnableLocation =
            new Setting<bool>("EnableLocation", false);
        public static readonly Setting<Color> BackgroundColor =
            new Setting<Color>("BackgroundColor", (Color)new PhoneApplicationPage().Resources["PhoneBackgroundColor"]);
        public static readonly Setting<Color> FontColor =
            new Setting<Color>("FontColor", (Color)new PhoneApplicationPage().Resources["PhoneForegroundColor"]);
        public static readonly Setting<int> FontSize =
            new Setting<int>("FontSize", 25);
        public static readonly Setting<string> FontFam =
            new Setting<string>("FontFamily", "Comic Sans MS");
    }
}