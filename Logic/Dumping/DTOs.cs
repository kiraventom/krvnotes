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

        internal DtoBoardWrapper(IBoard board)
        {
            Folders = board.Folders.Select(f => new DtoFolderWrapper(f));
        }

        public IEnumerable<DtoFolderWrapper> Folders { get; set; }
    }

    internal class DtoFolderWrapper
    {
        [JsonConstructor] // ctor for STJ
        public DtoFolderWrapper()
        {
        }

        internal DtoFolderWrapper(IFolder folder)
        {
            Guid = folder.Guid;
            Name = folder.Name;
            Notes = folder.Notes.Select(n => new DtoNoteWrapper(n));
            FolderType = folder.FolderType;
        }
        
        public string Guid { get; set; }
        public string Name { get; set; }
        public Constants.FolderType FolderType { get; set; }
        public IEnumerable<DtoNoteWrapper> Notes { get; set; }
    }

    internal class DtoNoteWrapper : BaseNote
    {
        [JsonConstructor] // ctor for STJ
        public DtoNoteWrapper(string guid, string header, string text, DateTime editedAt) : base(guid, header, text, editedAt)
        {
        }

        internal DtoNoteWrapper(INote note) : base(note.Guid, note.Header, note.Text, note.EditedAt)
        {
        }
    }
}
