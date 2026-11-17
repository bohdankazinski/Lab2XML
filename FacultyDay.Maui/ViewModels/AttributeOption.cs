namespace FacultyDay.Maui.ViewModels;

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
