namespace FacultyDay.Maui.Services;

public interface IDialogService
{
    Task<bool> ConfirmAsync(string title, string message, string accept, string cancel);
    Task ShowMessageAsync(string title, string message);
}

public interface IFileService
{
    Task<string?> PickXmlAsync();

    Task<string> SaveHtmlAsync(string html);
}

public interface IAppService
{
    void Quit();
}
