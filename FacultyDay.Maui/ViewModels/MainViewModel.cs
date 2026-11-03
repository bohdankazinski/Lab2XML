using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Xml;
using FacultyDay.Engine.Models;
using FacultyDay.Engine.Services;
using FacultyDay.Engine.Strategies;
using FacultyDay.Maui.Services;
using FacultyDay.Engine;

namespace FacultyDay.Maui.ViewModels;

/// <summary>
/// Уся логіка екрана. Кнопки прив'язані до команд, а code-behind сторінки порожній —
/// так ми уникаємо антишаблону «магічна кнопка» (логіка в обробниках Clicked).
/// </summary>
public sealed class MainViewModel : ObservableObject
{
    private readonly IDialogService _dialogs;
    private readonly IFileService _files;
    private readonly IAppService _app;

    private readonly SearchContext _context = new();   // контекст шаблону «Стратегія»
    private readonly XslTransformer _transformer = new();

    private string? _filePath;
    private string _fileName = "Файл не обрано";
    private string _resultText = string.Empty;
    private string _status = "Оберіть XML-файл, щоб почати.";
    private IXmlSearchStrategy _selectedStrategy;
    private WebViewSource? _htmlSource;

    public MainViewModel(IDialogService dialogs, IFileService files, IAppService app)
    {
        _dialogs = dialogs;
        _files = files;
        _app = app;

        Strategies = XmlSearchStrategies.All;
        _selectedStrategy = Strategies[0];

        OpenFileCommand = new AsyncCommand(OpenFileAsync);
        SearchCommand = new AsyncCommand(SearchAsync);
        TransformCommand = new AsyncCommand(TransformAsync);
        ClearCommand = new AsyncCommand(ClearAsync);
        ExitCommand = new AsyncCommand(ExitAsync);
    }

    // ----- Дані для прив'язки -----
    public IReadOnlyList<IXmlSearchStrategy> Strategies { get; }
    public ObservableCollection<AttributeOption> Attributes { get; } = new();

    public IXmlSearchStrategy SelectedStrategy
    {
        get => _selectedStrategy;
        set => SetProperty(ref _selectedStrategy, value);
    }

    public string FileName { get => _fileName; private set => SetProperty(ref _fileName, value); }
    public string ResultText { get => _resultText; private set => SetProperty(ref _resultText, value); }
    public string Status { get => _status; private set => SetProperty(ref _status, value); }
    public WebViewSource? HtmlSource { get => _htmlSource; private set => SetProperty(ref _htmlSource, value); }

    // ----- Команди -----
    public ICommand OpenFileCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand TransformCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand ExitCommand { get; }

    // ----- Реалізація -----

    /// <summary>«Підгрузка даних»: вибір файлу → пошук атрибутів, що в ньому є.</summary>
    private async Task OpenFileAsync()
    {
        string? path = await _files.PickXmlAsync();
        if (path is null) return;

        try
        {
            var names = await Task.Run(() => AttributeScanner.Scan(path));
            if (names.Count == 0)
            {
                await _dialogs.ShowMessageAsync("Увага", $"У файлі немає вузлів <{XmlNames.Event}> з атрибутами.");
                return;
            }

            _filePath = path;
            FileName = Path.GetFileName(path);
            Attributes.Clear();
            foreach (var n in names) Attributes.Add(new AttributeOption(n));
            ResultText = string.Empty;
            HtmlSource = null;
            Status = $"Файл завантажено. Знайдено атрибутів: {names.Count}. Оберіть ті, за якими шукати.";
        }
        catch (XmlException ex)
        {
            await _dialogs.ShowMessageAsync("Некоректний XML", ex.Message);
        }
        catch (Exception ex)
        {
            await _dialogs.ShowMessageAsync("Помилка", ex.Message);
        }
    }

    private async Task SearchAsync()
    {
        if (_filePath is null)
        {
            Status = "Спочатку оберіть XML-файл.";
            return;
        }

        // Динамічно формуємо набір умов із того, що відмітив користувач
        var criteria = Attributes
            .Where(a => a.IsSelected)
            .Select(a => new SearchCriterion(a.Name, a.Keyword.Trim()))
            .ToList();

        if (criteria.Count == 0)
        {
            Status = "Оберіть хоча б один атрибут для пошуку.";
            return;
        }

        try
        {
            _context.Strategy = SelectedStrategy;
            var path = _filePath;
            var sw = Stopwatch.StartNew();
            var found = await Task.Run(() => _context.Execute(path, criteria));
            sw.Stop();

            ResultText = found.Count == 0
                ? "Нічого не знайдено."
                : string.Join(Environment.NewLine + Environment.NewLine, found.Select(f => f.ToString()));
            Status = $"[{SelectedStrategy.Name}] знайдено заходів: {found.Count} (за {sw.ElapsedMilliseconds} мс)";
        }
        catch (Exception ex)
        {
            await _dialogs.ShowMessageAsync("Помилка пошуку", ex.Message);
        }
    }

    private async Task TransformAsync()
    {
        if (_filePath is null)
        {
            Status = "Спочатку оберіть XML-файл.";
            return;
        }

        try
        {
            var path = _filePath;
            string html = await Task.Run(() => _transformer.Transform(path));
            string saved = await _files.SaveHtmlAsync(html);
            HtmlSource = new HtmlWebViewSource { Html = html };
            Status = $"HTML сформовано та збережено: {saved}";
        }
        catch (Exception ex)
        {
            await _dialogs.ShowMessageAsync("Помилка трансформації", ex.Message);
        }
    }

    /// <summary>Clear: очищає поле виводу та задані параметри пошуку (файл і список атрибутів лишаються).</summary>
    private Task ClearAsync()
    {
        foreach (var a in Attributes)
        {
            a.IsSelected = false;
            a.Keyword = string.Empty;
        }
        ResultText = string.Empty;
        HtmlSource = null;
        Status = "Очищено.";
        return Task.CompletedTask;
    }

    private async Task ExitAsync()
    {
        bool yes = await _dialogs.ConfirmAsync(
            "Вихід", "Чи дійсно ви хочете завершити роботу з програмою?", "Так", "Ні");
        if (yes) _app.Quit();
    }
}
