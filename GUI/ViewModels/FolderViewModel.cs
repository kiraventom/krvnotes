using System;
using System.Collections.Generic;
using System.Linq;
using BL;
using Common;
using Common.Utils;

namespace GUI.ViewModels
{
    public class FolderViewModel : Notifiable, IFolder
    {
        private string _name;

        public static FolderViewModel FromIFolder(IFolder folder) => new(folder);

        private FolderViewModel(IFolder folder)
        {
            Guid = folder.Guid;
            Name = folder.Name;
            Notes = folder.Notes.Select(NoteViewModel.FromINote);
            FolderType = folder.FolderType;
        }

        internal bool CanUserAdd => FolderType is not Constants.FolderType.Archive and not Constants.FolderType.RecycleBin;
        internal bool CanUserEdit => FolderType is not Constants.FolderType.RecycleBin;

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

        IEnumerable<INote> IFolder.Notes => Notes;

        public IEnumerable<NoteViewModel> Notes { get; }
    }
}
