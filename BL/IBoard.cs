namespace BL;

public interface IBoard
{
    bool AddFolder(IFolder folder);
    bool AddNote(IFolder folder, INote note);
    bool EditFolder(IFolder folder);
    bool EditNote(IFolder folder, INote note);
    bool RemoveFolder(string name);
    bool RemoveNote(IFolder folder, string guid);
    IEnumerable<IFolder> Folders { get; }
}