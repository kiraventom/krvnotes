using System;
using System.Collections.Generic;
using System.Linq;
using BL;
using Common;
using Common.Utils;

namespace GUI.ViewModels
{
    public interface IFolderViewModel : IFolder
    {
        static IFolderViewModel FromIFolder(IFolder folder) => new FolderViewModel(folder);
    }

    internal class FolderViewModel : Notifiable, IFolderViewModel
    {
        private string _name;

        internal FolderViewModel(IFolder folder)
        {
            Guid = folder.Guid;
            Name = folder.Name;
            Notes = folder.Notes.Select(INoteViewModel.FromINote);
            FolderType = folder.FolderType;
        }

        public bool CanUserAdd => FolderType is not Constants.FolderType.Archive and not Constants.FolderType.RecycleBin;
        
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

        public IEnumerable<INoteViewModel> Notes { get; }
    }
}
