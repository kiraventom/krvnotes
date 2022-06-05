namespace BL;

public interface INote
{
    string Guid { get; }

    string Header { get; }

    string Text { get; }
}