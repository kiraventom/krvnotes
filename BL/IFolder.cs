namespace BL;

public interface IFolder
{
    string Name { get; }
    INotesCollection Notes { get; }
    
    // TODO: remove return type
    INote AddNote(string header, string text);

    bool RemoveNote(string guid);
}

public interface INotesCollection : IEnumerable<INote>
{
    INote this[string key] { get; }
}