using Logic.Dumping;

namespace Logic
{
    public class Model
    {
        public static Model Instance { get; } = new();

        private Model()
        {
            var dumper = new Dumper();
            BoardModel = dumper.CreateBoard();
        }

        public BoardModel BoardModel { get; }
    }
}
