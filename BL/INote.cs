namespace BL;

public interface INote
{
    void Edit(string header, string text);

    string Guid { get; }
    
    string Header { get; }
    
    string Text { get; }

    DateTime EditedAt { get; }
}