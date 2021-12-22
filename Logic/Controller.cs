namespace Logic;

public class Controller
{
    public Controller()
    {
        Dumper = new Dumper();
        
        var didLoad = Dumper.TryLoad(out var board);
        Board = didLoad ? board : new Board();
    }

    public Board Board { get; }
    public Dumper Dumper { get; }
}