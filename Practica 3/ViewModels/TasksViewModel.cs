using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Practica3MVVM.ViewModels;

// ViewModel de lista de tareas (ToDo) con MVVM simple
public class TasksViewModel : INotifyPropertyChanged
{
    private string _newTask = string.Empty;
    private ObservableCollection<string> _tasks = new();
    private string? _selectedTask;

    public string NewTask
    {
        get => _newTask;
        set
        {
            if (_newTask != value)
            {
                _newTask = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<string> Tasks
    {
        get => _tasks;
        set
        {
            if (_tasks != value)
            {
                _tasks = value;
                OnPropertyChanged();
            }
        }
    }

    public string? SelectedTask
    {
        get => _selectedTask;
        set
        {
            if (_selectedTask != value)
            {
                _selectedTask = value;
                OnPropertyChanged();
                (RemoveTaskCommand as Command)?.ChangeCanExecute();
            }
        }
    }

    public ICommand AddTaskCommand { get; }
    public ICommand RemoveTaskCommand { get; }
    public ICommand ClearTasksCommand { get; }

    public TasksViewModel()
    {
        AddTaskCommand = new Command(AddTask, CanAddTask);
        RemoveTaskCommand = new Command(RemoveTask, CanRemoveTask);
        ClearTasksCommand = new Command(ClearTasks, CanClearTasks);

        // Datos iniciales
        Tasks.Add("Comprar leche");
        Tasks.Add("Enviar correo a profesor");
        Tasks.Add("Leer documentación de MAUI");
    }

    private bool CanAddTask() => !string.IsNullOrWhiteSpace(NewTask);
    private void AddTask()
    {
        if (!CanAddTask()) return;
        Tasks.Add(NewTask.Trim());
        NewTask = string.Empty;
        (AddTaskCommand as Command)?.ChangeCanExecute();
        (ClearTasksCommand as Command)?.ChangeCanExecute();
    }

    private bool CanRemoveTask() => SelectedTask != null;
    private void RemoveTask()
    {
        if (SelectedTask == null) return;
        Tasks.Remove(SelectedTask);
        SelectedTask = null;
        (RemoveTaskCommand as Command)?.ChangeCanExecute();
        (ClearTasksCommand as Command)?.ChangeCanExecute();
    }

    private bool CanClearTasks() => Tasks.Count > 0;
    private void ClearTasks()
    {
        Tasks.Clear();
        (ClearTasksCommand as Command)?.ChangeCanExecute();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
