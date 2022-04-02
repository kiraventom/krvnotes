namespace BL;

public interface IFolder
{
    string Name { get; }
    IEnumerable<INote> Notes { get; }
    bool AddNote(INote note);
    bool EditNote(INote note);
    bool RemoveNote(string guid);
}