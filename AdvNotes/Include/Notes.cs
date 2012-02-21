// Copyright (c) Adam Nathan.  All rights reserved.
// Modified by Bayo Olatunji.
using System;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media;

namespace AdvNotes
{
    public class Notes : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // A helper method used by the properties
        void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        DateTimeOffset modified;
        public DateTimeOffset Modified
        {
            get { return this.modified; }
            set
            {
                if (value != this.modified)
                {
                    this.modified = value;
                    NotifyPropertyChanged("Modified");
                }
            }
        }


        string title;
        public string Title
        {
            get { return this.title; }
            set
            {
                if (value != this.title)
                {
                    this.title = value; NotifyPropertyChanged("Title");
                }
            }
        }

        string location;
        public string Location
        {
            get { return this.location; }
            set
            {
                if (value != this.location)
                {
                    this.location = value; NotifyPropertyChanged("Location");
                }
            }
        }

        public string Filename { get; set; }

        // Future implementation properties
        # region Objects to be implemented later
        int textSize;
        public int TextSize
        {
            get { return this.textSize; }
            set 
            {
                if (value != this.textSize)
                {
                    this.textSize = value; NotifyPropertyChanged("TextSize");
                }
            }
        }

        Color screenColor;
        public Color ScreenColor
        {
            get { return this.screenColor; }
            set
            {
                if (value != this.screenColor)
                {
                    this.screenColor = value;
                    NotifyPropertyChanged("ScreenColor");
                    NotifyPropertyChanged("ScreenBrush");
                }
            }
        }

        Color textColor;
        public Color TextColor
        {
            get { return this.textColor; }
            set
            {
                if (value != this.textColor)
                {
                    this.textColor = value;
                    NotifyPropertyChanged("TextColor");
                    NotifyPropertyChanged("TextBrush");
                }
            }
        }

        // Three readonly properties whose value is computed from other properties:

        public Brush ScreenBrush
        {
            get { return new SolidColorBrush(this.ScreenColor); }
        }

        public Brush TextBrush
        {
            get { return new SolidColorBrush(this.TextColor); }
        }
        # endregion

        // Storage methods
        # region Methods dealing with ISO Storage
        public void SaveContent(string content)
        {
            using (IsolatedStorageFile appStore = IsolatedStorageFile.GetUserStoreForApplication())
            using (IsolatedStorageFileStream fileStream = appStore.CreateFile(this.Filename))
            using (StreamWriter sWriter = new StreamWriter(fileStream))
            {
                sWriter.Write(content);
            }
        }

        public string GetContent()
        {
            using (IsolatedStorageFile appStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!appStore.FileExists(this.Filename))
                    return "";
                else
                {
                    using (IsolatedStorageFileStream fileStream = appStore.OpenFile(this.Filename, FileMode.Open))
                    using (StreamReader sReader = new StreamReader(fileStream))
                        return sReader.ReadToEnd();
                }
            }
        }

        public void DeleteContent()
        {
            using (IsolatedStorageFile appStore = IsolatedStorageFile.GetUserStoreForApplication())
                appStore.DeleteFile(this.Filename);
        }
        # endregion 
    }
}