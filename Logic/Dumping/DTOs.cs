// Public setters are required by STJ
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Text.Json.Serialization;
using BL;
using Common;

namespace Logic.Dumping
{
    internal class DtoBoardWrapper
    {

        [JsonConstructor] // ctor for STJ
        public DtoBoardWrapper()
        {
        }

        internal DtoBoardWrapper(BoardModel boardModel)
        {
            Folders = boardModel.Folders.Select(f => new DtoFolderWrapper(f));
        }

        public IEnumerable<DtoFolderWrapper> Folders { get; set; }
    }

    internal class DtoFolderWrapper
    {
        [JsonConstructor] // ctor for STJ
        public DtoFolderWrapper()
        {
        }

        internal DtoFolderWrapper(FolderModel folderModel)
        {
            Guid = folderModel.Guid;
            Name = folderModel.Name;
            Notes = folderModel.Notes.Select(n => new DtoNoteModelWrapper(n));
            FolderType = folderModel.FolderType;
        }
        
        public string Guid { get; set; }
        public string Name { get; set; }
        public Constants.FolderType FolderType { get; set; }
        public IEnumerable<DtoNoteModelWrapper> Notes { get; set; }
    }

    internal class DtoNoteModelWrapper : BaseNoteModel
    {
        [JsonConstructor] // ctor for STJ
        public DtoNoteModelWrapper(string guid, string header, string text, DateTime editedAt) : base(guid, header, text, editedAt)
        {
        }

        internal DtoNoteModelWrapper(INoteModel noteModel) : base(noteModel.Guid, noteModel.Header, noteModel.Text, noteModel.EditedAt)
        {
        }
    }
}
