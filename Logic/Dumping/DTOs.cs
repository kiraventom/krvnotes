using System.Text.Json.Serialization;
using BL;

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
            Name = folder.Name;
            Notes = folder.Notes.Select(n => new DtoNoteWrapper(n));
        }
        
        public string Name { get; set; }
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
