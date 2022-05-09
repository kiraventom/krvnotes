using Common;

namespace BL;

public interface IFolder
{
    string Guid { get; }
    string Name { get; }
    Constants.FolderType FolderType { get; }
    INotesCollection Notes { get; }

    event Action<INote, IFolder> NoteMoved;

    INote AddNote(string header, string text);
    void MoveNote(string noteGuid, string folderGuid);
    // ReSharper disable once UnusedMethodReturnValue.Global
    bool RemoveNote(string guid);
}

public interface INotesCollection : IEnumerable<INote>
{
    INote this[string key] { get; }
}