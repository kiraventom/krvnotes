using BL;

namespace Starter;

internal class Controller : IController
{
    public Controller(IBoard board)
    {
        Board = board;
    }
    
    public IBoard Board { get; }
}