namespace FacultyDay.Maui.ViewModels;

/// <summary>Один атрибут, знайдений у файлі: чи вибраний користувачем і за яким словом шукати.</summary>
public sealed class AttributeOption : ObservableObject
{
    private bool _isSelected;
    private string _keyword = string.Empty;

    public AttributeOption(string name) => Name = name;

    public string Name { get; }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public string Keyword
    {
        get => _keyword;
        set => SetProperty(ref _keyword, value);
    }
}
