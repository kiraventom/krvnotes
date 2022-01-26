namespace Logic;

public class Controller
{
    public Controller()
    {
        var didLoad = Dumper.TryLoad(out var board);
        Board = didLoad ? board : new Board();
    }

    public Board Board { get; }
}