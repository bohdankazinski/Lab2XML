using System.Text;

namespace FacultyDay.Engine.Models;

/// <summary>Захід та все його піддерево, розкладене у плаский список елементів.</summary>
public sealed class EventRecord
{
    public List<ElementRecord> Elements { get; } = new();

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var el in Elements)
        {
            sb.Append(' ', el.Depth * 2).Append(el.Name);
            if (el.Attributes.Count > 0)
                sb.Append(": ").Append(string.Join("; ", el.Attributes.Select(a => $"{a.Key}={a.Value}")));
            sb.AppendLine();
        }
        return sb.ToString().TrimEnd();
    }
}
