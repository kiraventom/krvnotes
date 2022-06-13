using Common;

namespace BL
{
    public interface IFolder
    {
        string Guid { get; }
        string Name { get; }
        Constants.FolderType FolderType { get; }
        IKeyedCollection<INote> Notes { get; }
    }
}
