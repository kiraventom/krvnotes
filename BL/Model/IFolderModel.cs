using Common;

namespace BL.Model;

public interface IFolderModel
{
    string Guid { get; }
    string Name { get; }
    Constants.FolderType FolderType { get; }
    INotesCollection Notes { get; }

    INoteModel AddNote(string header, string text);
    void MoveNote(string noteGuid, string folderGuid);
    // ReSharper disable once UnusedMethodReturnValue.Global
    bool RemoveNote(string guid);
}

public interface INotesCollection : IEnumerable<INoteModel>
{
    INoteModel this[string key] { get; }
}