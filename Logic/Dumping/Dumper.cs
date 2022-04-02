using System.Text.Json;
using BL;

namespace Logic.Dumping;

public class Dumper : IDumper
{
    #region DTOs

    private class DumpBoard : IBoard
    {
        public DumpBoard()
        {
        } // for STJ

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
        public DumpFolder()
        {
        } // for STJ

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
        public DumpNote()
        {
        } // for STJ

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

    #endregion

    public Dumper()
    {
        Directory.CreateDirectory(Constants.FolderPath);
    }
    
    public IBoard CreateBoard()
    {
        if (!File.Exists(Constants.FilePath)) 
            return new Board(this);
        
        string json = File.ReadAllText(Constants.FilePath);
        var dumpBoard = JsonSerializer.Deserialize<DumpBoard>(json, Constants.SerializerOptions);
        if (dumpBoard is null)
            throw new FormatException($"Could not parse {Constants.FilePath}");

        return new Board(this, dumpBoard);
    }

    void IDumper.Save(IBoard board) => Save(board);

    private static void Save(IBoard board)
    {
        var dumpBoard = new DumpBoard(board.Folders);
        string json = JsonSerializer.Serialize(dumpBoard, Constants.SerializerOptions);
        File.WriteAllText(Constants.FilePath, json);
    }
}

public interface IDumper
{
    internal void Save(IBoard board);
}