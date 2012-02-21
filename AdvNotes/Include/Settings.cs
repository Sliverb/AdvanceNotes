// Copyright (c) Adam Nathan.  All rights reserved.
// Modified by Bayo Olatunji.
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace AdvNotes
{
    public static class Settings
    {
        // The user's data
        public static readonly Setting<ObservableCollection<Notes>> NotesList =
            new Setting<ObservableCollection<Notes>>("NotesList", new ObservableCollection<Notes>());

        // Temporary state
        public static readonly Setting<int> CurrentNoteIndex =
            new Setting<int>("CurrentNoteIndex", -1);

        public static readonly Setting<int> appState =
            new Setting<int>("MyState", 0);
    }
}