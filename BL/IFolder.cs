using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace BL
{
    public interface IFolder
    {
        string Guid { get; }
        string Name { get; }
        Constants.FolderType FolderType { get; }
        IKeyedCollection<INote> Notes { get; }
    }
}
