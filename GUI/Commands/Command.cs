using System;
using System.Windows.Input;

namespace GUI.Commands;

public class Command : ICommand
{
    public Command(Action action)
    {
        _action = action;
    }

    private readonly Action _action;
    
    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter) => _action.Invoke();

#pragma warning disable 67
    public event EventHandler CanExecuteChanged;
#pragma warning restore 67
}

public class Command<T> : ICommand
{
    private readonly Action<T> _action;
    private readonly Predicate<T> _predicate;

    public Command(Action<T> action, Predicate<T> predicate)
    {
        _action = action;
        _predicate = predicate;
    }
    
    public bool CanExecute(object parameter)
    {
        if (parameter is T t)
            return CanExecute(t);
        
        return false;
    }
    
    public bool CanExecute(T parameter)
    {
        return _predicate.Invoke(parameter);
    }

    public void Execute(object parameter)
    {
        if (parameter is T t)
            Execute(t);
        else
            throw new ArgumentException("Invalid parameter type", nameof(parameter));
    }

    public void Execute(T parameter)
    {
        _action.Invoke(parameter);
    }
    
    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public void RaiseCanExecuteChanged()
    {
        CommandManager.InvalidateRequerySuggested();
    }
}