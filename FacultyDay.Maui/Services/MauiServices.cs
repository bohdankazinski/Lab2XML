using System.Text;

namespace FacultyDay.Maui.Services;

public sealed class MauiDialogService : IDialogService
{
    private static Page? CurrentPage => Application.Current?.Windows.FirstOrDefault()?.Page;

    public async Task<bool> ConfirmAsync(string title, string message, string accept, string cancel)
    {
        var page = CurrentPage;
        return page is not null && await page.DisplayAlert(title, message, accept, cancel);
    }

    public async Task ShowMessageAsync(string title, string message)
    {
        if (CurrentPage is { } page)
            await page.DisplayAlert(title, message, "OK");
    }
}

public sealed class MauiFileService : IFileService
{
    public async Task<string?> PickXmlAsync()
    {
        var xmlType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.WinUI,       new[] { ".xml" } },
            { DevicePlatform.Android,     new[] { "text/xml", "application/xml" } },
            { DevicePlatform.iOS,         new[] { "public.xml" } },
            { DevicePlatform.MacCatalyst, new[] { "public.xml" } }
        });

        var file = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Оберіть XML-файл",
            FileTypes = xmlType
        });
        return file?.FullPath;
    }

    public async Task<string> SaveHtmlAsync(string html)
    {
        string path = Path.Combine(FileSystem.AppDataDirectory, "faculty_day_report.html");
        await File.WriteAllTextAsync(path, html, new UTF8Encoding(false));
        return path;
    }
}

public sealed class MauiAppService : IAppService
{
    public void Quit() => Application.Current?.Quit();
}
