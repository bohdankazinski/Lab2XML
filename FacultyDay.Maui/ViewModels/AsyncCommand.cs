using System.Windows.Input;

namespace FacultyDay.Maui.ViewModels;

/// <summary>Проста асинхронна команда; поки вона виконується, кнопка вимкнена.</summary>
public sealed class AsyncCommand : ICommand
{
    private readonly Func<Task> _execute;
    private bool _busy;

    public AsyncCommand(Func<Task> execute) => _execute = execute;

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => !_busy;

    public async void Execute(object? parameter)
    {
        _busy = true;
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        try { await _execute(); }
        finally
        {
            _busy = false;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
