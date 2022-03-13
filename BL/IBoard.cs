namespace BL;

public interface IBoard
{
    bool AddNote(IFolder folder, INote note);
    bool EditNote(IFolder folder, INote note);
    bool RemoveNote(IFolder folder, string guid);
    IEnumerable<IFolder> Folders { get; }
}