namespace BL;

public interface IFolder
{
    string Name { get; }
    IEnumerable<INote> Notes { get; }
}