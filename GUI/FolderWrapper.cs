using System;
using System.Collections.Generic;
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

        public FolderWrapper()
        {
            throw new NotImplementedException();
        }

        private FolderWrapper(IFolder folder)
        {
            Name = folder.Name;
            var notes = folder.Notes.Select(NoteWrapper.FromNote);
            Notes = new ObservableCollection<NoteWrapper>(notes);
        }

        public Constants.FolderType FolderType => ParseFolderName(Name);

        public static FolderWrapper FromFolder(IFolder folder) => new(folder);

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

        private static Constants.FolderType ParseFolderName(string name)
        {
            var didParse = Enum.TryParse<Constants.FolderType>(name, out var type);
            return didParse ? type : Constants.FolderType.Custom;
        }
    }
}
