namespace FacultyDay.Maui;

public partial class MainPage : ContentPage
{
    // Жодних обробників Clicked: сторінка лише отримує ViewModel
    public MainPage(ViewModels.MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
