using System.Xml.Linq;
using FacultyDay.Engine.Models;

namespace FacultyDay.Engine.Strategies;

/// <summary>LINQ to XML: запит складається динамічно — по одному Where на кожну умову користувача.</summary>
public sealed class LinqSearchStrategy : IXmlSearchStrategy
{
    public string Name => "LINQ to XML (XDocument)";

    public IReadOnlyList<EventRecord> Search(string xmlPath, IReadOnlyList<SearchCriterion> criteria)
    {
        var doc = XDocument.Load(xmlPath);

        IEnumerable<XElement> events = doc.Descendants(XmlNames.Event);
        foreach (var criterion in criteria)
        {
            var c = criterion; // копія для замикання
            events = events.Where(e =>
                e.DescendantsAndSelf()
                 .Attributes(c.Attribute)
                 .Any(a => a.Value.Contains(c.Keyword, StringComparison.OrdinalIgnoreCase)));
        }

        return events.Select(e =>
        {
            var record = new EventRecord();
            Collect(e, 0, record);
            return record;
        }).ToList();
    }

    private static void Collect(XElement el, int depth, EventRecord record)
    {
        var attrs = el.Attributes()
            .Where(a => !a.IsNamespaceDeclaration)
            .Select(a => new KeyValuePair<string, string>(a.Name.LocalName, a.Value))
            .ToList();
        record.Elements.Add(new ElementRecord(el.Name.LocalName, depth, attrs));

        foreach (var child in el.Elements())
            Collect(child, depth + 1, record);
    }
}
