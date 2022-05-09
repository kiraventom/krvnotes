﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using BL;
using Common;
using Common.Utils;

namespace GUI
{
    public class FolderWrapper : Notifiable
    {
        private string _name;

        private FolderWrapper(IFolder folder)
        {
            Guid = folder.Guid;
            Name = folder.Name;
            var notes = folder.Notes.Select(NoteWrapper.FromNote);
            Notes = new ObservableCollection<NoteWrapper>(notes);
            FolderType = folder.FolderType;
        }

        public bool CanUserAdd => FolderType is not Constants.FolderType.Archive and not Constants.FolderType.RecycleBin;

        public static FolderWrapper FromFolder(IFolder folder) => new(folder);

        public string Guid { get; }

        public Constants.FolderType FolderType { get; }

        public string Name
        {
            get => _name;
            set
            {
                if (!string.IsNullOrWhiteSpace(_name))
                    throw new NotSupportedException("Name is already set");

                _name = value;
            }
        }

        public ObservableCollection<NoteWrapper> Notes { get; }
    }
}
