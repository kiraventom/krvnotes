using System.Text.Json.Serialization;
using BL;

namespace Logic.Dumping
{
    public partial class Dumper
    {
        private class DumpBoard : IBoard
        {
            [JsonConstructor] // ctor for STJ
            public DumpBoard()
            {
            }

            internal DumpBoard(IEnumerable<IFolder> folders)
            {
                Folders = folders.Select(f => new DumpFolder(f.Name, f.Notes));
            }

            public IEnumerable<DumpFolder> Folders { get; set; }
            IEnumerable<IFolder> IBoard.Folders => Folders;

            #region Methods

            public bool AddFolder(IFolder folder) => throw new NotSupportedException();
            public bool EditFolder(string oldName, string newName) => throw new NotSupportedException();
            public bool RemoveFolder(string name) => throw new NotSupportedException();

            #endregion
        }

        private class DumpFolder : IFolder
        {
            [JsonConstructor] // ctor for STJ
            public DumpFolder()
            {
            }

            internal DumpFolder(string name, IEnumerable<INote> notes)
            {
                Name = name;
                Notes = notes.Select(n => new DumpNote(n.Guid, n.Header, n.Text, n.EditedAt));
            }

            public string Name { get; set; }
            public IEnumerable<DumpNote> Notes { get; set; }
            IEnumerable<INote> IFolder.Notes => Notes;

            #region Methods

            public bool AddNote(INote note) => throw new NotSupportedException();
            public bool EditNote(INote note) => throw new NotSupportedException();
            public bool RemoveNote(string guid) => throw new NotSupportedException();

            #endregion
        }

        private class DumpNote : INote
        {
            [JsonConstructor] // ctor for STJ
            public DumpNote()
            {
            }

            internal DumpNote(string guid, string header, string text, DateTime editedAt)
            {
                Guid = guid;
                Header = header;
                Text = text;
                EditedAt = editedAt;
            }

            public string Guid { get; set; }
            public string Header { get; set; }
            public string Text { get; set; }
            public DateTime EditedAt { get; set; }
        }
    }
}
