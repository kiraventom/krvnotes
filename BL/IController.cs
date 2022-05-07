namespace BL;

public interface IController
{
    IBoard Board { get; }
    void SetViewModel(object viewModel);
}