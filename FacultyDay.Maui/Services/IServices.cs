namespace FacultyDay.Maui.Services;

/// <summary>Діалоги. Інтерфейс дозволяє ViewModel не залежати від UI-класів.</summary>
public interface IDialogService
{
    Task<bool> ConfirmAsync(string title, string message, string accept, string cancel);
    Task ShowMessageAsync(string title, string message);
}

/// <summary>Робота з файловою системою.</summary>
public interface IFileService
{
    /// <summary>Показує вибір XML-файлу; повертає шлях або null, якщо користувач скасував.</summary>
    Task<string?> PickXmlAsync();

    /// <summary>Зберігає HTML у файл і повертає шлях до нього.</summary>
    Task<string> SaveHtmlAsync(string html);
}

/// <summary>Керування життєвим циклом застосунку.</summary>
public interface IAppService
{
    void Quit();
}
