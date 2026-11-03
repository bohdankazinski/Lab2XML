using FacultyDay.Maui.Services;
using FacultyDay.Maui.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace FacultyDay.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Залежності (DI): ViewModel отримує сервіси через конструктор
        builder.Services.AddSingleton<IDialogService, MauiDialogService>();
        builder.Services.AddSingleton<IFileService, MauiFileService>();
        builder.Services.AddSingleton<IAppService, MauiAppService>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<MainPage>();

        // Підтвердження виходу при закритті вікна хрестиком (Windows)
        builder.ConfigureLifecycleEvents(events =>
        {
#if WINDOWS
            bool exitConfirmed = false;
            events.AddWindows(windows => windows.OnWindowCreated(window =>
            {
                window.AppWindow.Closing += async (sender, args) =>
                {
                    if (exitConfirmed) return;
                    args.Cancel = true; // скасовуємо закриття до відповіді користувача

                    var dialogs = IPlatformApplication.Current!.Services.GetRequiredService<IDialogService>();
                    bool yes = await dialogs.ConfirmAsync(
                        "Вихід", "Чи дійсно ви хочете завершити роботу з програмою?", "Так", "Ні");
                    if (yes)
                    {
                        exitConfirmed = true;
                        Application.Current?.Quit();
                    }
                };
            }));
#endif
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
