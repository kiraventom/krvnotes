using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using BL.Model;
using Logic.Dumping;

namespace Logic
{
    public class Model : IModel
    {
        public Model()
        {
            var dumper = new Dumper();
            BoardModel = dumper.CreateBoard();
        }
        
        public IBoardModel BoardModel { get; }
    }
}
