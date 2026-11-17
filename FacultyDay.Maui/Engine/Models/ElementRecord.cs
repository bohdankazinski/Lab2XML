namespace FacultyDay.Engine.Models;

public sealed record ElementRecord(
    string Name,
    int Depth,
    IReadOnlyList<KeyValuePair<string, string>> Attributes);
