namespace BL;

public interface IBoard
{
    bool Add(string guid, string header, string text);
    bool Edit(string guid, string header, string text);
    bool Remove(string guid);
    IReadOnlyDictionary<string, INote> Notes { get; }
}

public interface INote
{
    string Header { get; }
    string Text { get; }
    DateTime CreatedAt { get; }
}

public class WrongAppEntryPointException : Exception
{
    
}

public static class Controller
{
    public static IBoard GetBoard()
    {
        if (_board is null)
        {
            if (BoardRequested is null)
                throw new WrongAppEntryPointException();
            
            BoardRequested.Invoke(null, EventArgs.Empty);
            BoardSet.WaitOne();
        }

        return _board;
    }

    /// <summary>
    /// Works only for the first call
    /// </summary>
    /// <param name="board"></param>
    public static void SetBoard(IBoard board)
    {
        if (_board is not null) 
            return;
        
        ArgumentNullException.ThrowIfNull(board);
        _board = board;
        BoardSet.Set();
    }
    
    private static IBoard _board;

    private static readonly ManualResetEvent BoardSet = new(initialState: false);
    public static event EventHandler BoardRequested;
}