using System.Text;
using System.Xml;
using FacultyDay.Engine.Models;

namespace FacultyDay.Engine.Strategies;

internal static class XPathBuilder
{
    private const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZАБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
    private const string Lower = "abcdefghijklmnopqrstuvwxyzабвгґдеєжзиіїйклмнопрстуфхцчшщьюя";

    public static string Build(IEnumerable<SearchCriterion> criteria)
    {
        var sb = new StringBuilder("//").Append(XmlNames.Event);
        foreach (var c in criteria)
        {
            XmlConvert.VerifyNCName(c.Attribute);
            string attr = "@" + c.Attribute;
            sb.Append("[descendant-or-self::*[").Append(attr).Append("]")
              .Append("[contains(translate(").Append(attr).Append(", ")
              .Append(Literal(Upper)).Append(", ").Append(Literal(Lower)).Append("), ")
              .Append(Literal(c.Keyword.ToLowerInvariant())).Append(")]]");
        }
        return sb.ToString();
    }

    /// <summary>Екранує рядок для XPath (у XPath 1.0 лапки екрануються лише через concat).</summary>
    private static string Literal(string s)
    {
        if (!s.Contains('\'')) return $"'{s}'";
        if (!s.Contains('"')) return $"\"{s}\"";
        return "concat(" + string.Join(", \"'\", ", s.Split('\'').Select(p => $"'{p}'")) + ")";
    }
}
