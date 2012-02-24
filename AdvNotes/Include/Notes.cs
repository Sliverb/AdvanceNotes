// Copyright (c) Adam Nathan.  All rights reserved.
// Modified by Bayo Olatunji.
using System;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media;

namespace AdvNotes.Include
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