using Common;

namespace BL;

public interface IFolder
{
    string Guid { get; }
    string Name { get; }
    Constants.FolderType FolderType { get; }
    INotesCollection Notes { get; }
    
    INote AddNote(string header, string text);

    // ReSharper disable once UnusedMethodReturnValue.Global
    bool RemoveNote(string guid);
}

public interface INotesCollection : IEnumerable<INote>
{
    INote this[string key] { get; }
}