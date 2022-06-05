using Logic.Dumping;

namespace Logic
{
    public interface IModel
    {
        IBoardModel BoardModel { get; }

        static IModel Instance { get; } = new Model();
    }

    internal class Model : IModel
    {
        public Model()
        {
            var dumper = new Dumper();
            BoardModel = dumper.CreateBoard();
        }
        
        public IBoardModel BoardModel { get; }
    }
}
