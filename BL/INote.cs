namespace BL;

public interface INote
{
    string Guid { get; }

    string Header { get; set; }

    string Text { get; set; }
}