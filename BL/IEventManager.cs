using System.Collections.Specialized;
using System.ComponentModel;
using BL.ViewModel;
using Common.Utils;
using Common.Utils.Exceptions;

namespace BL
{
    public interface IEventManager
    {
        void SetViewModel(IViewModel viewModel);
    }
}

