using System;
using System.Collections.ObjectModel;
using System.Linq;
using BL.Model;
using BL.ViewModel;
using Common;
using Common.Utils;

namespace GUI
{
    public class FolderViewModel : Notifiable, IFolderViewModel
    {
        private string _name;

        private FolderViewModel(IFolderModel folderModel)
        {
            Guid = folderModel.Guid;
            Name = folderModel.Name;
            var notes = folderModel.Notes.Select(NoteViewModel.FromNote);
            Notes = new ObservableCollection<INoteViewModel>(notes);
            FolderType = folderModel.FolderType;
        }

        public bool CanUserAdd => FolderType is not Constants.FolderType.Archive and not Constants.FolderType.RecycleBin;

        public static FolderViewModel FromFolder(IFolderModel folderModel) => new(folderModel);

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

        public ObservableCollection<INoteViewModel> Notes { get; }
    }
}
