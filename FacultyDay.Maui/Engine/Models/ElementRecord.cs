namespace FacultyDay.Engine.Models;

/// <summary>Один елемент XML (Event, Responsible, Tasks, Task) з його атрибутами.</summary>
/// <param name="Depth">Глибина відносно вузла Event (сам Event = 0).</param>
public sealed record ElementRecord(
    string Name,
    int Depth,
    IReadOnlyList<KeyValuePair<string, string>> Attributes);
