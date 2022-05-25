namespace BL.Model;

public interface INoteModel
{
    void Edit(string header, string text);

    string Guid { get; }
    
    string Header { get; }
    
    string Text { get; }

    DateTime EditedAt { get; }
}