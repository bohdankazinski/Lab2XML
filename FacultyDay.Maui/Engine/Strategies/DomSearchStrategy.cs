using System.Xml;
using FacultyDay.Engine.Models;

namespace FacultyDay.Engine.Strategies;

/// <summary>DOM-підхід: документ повністю завантажується в дерево, пошук виконується XPath-запитом.</summary>
public sealed class DomSearchStrategy : IXmlSearchStrategy
{
    public string Name => "DOM (XmlDocument)";

    public IReadOnlyList<EventRecord> Search(string xmlPath, IReadOnlyList<SearchCriterion> criteria)
    {
        var doc = new XmlDocument();
        doc.Load(xmlPath);

        string xpath = XPathBuilder.Build(criteria); // динамічно згенерований запит
        var nodes = doc.SelectNodes(xpath)?.OfType<XmlElement>() ?? Enumerable.Empty<XmlElement>();

        var result = new List<EventRecord>();
        foreach (var node in nodes)
        {
            var record = new EventRecord();
            Collect(node, 0, record);
            result.Add(record);
        }
        return result;
    }

    private static void Collect(XmlElement el, int depth, EventRecord record)
    {
        var attrs = el.Attributes.Cast<XmlAttribute>()
            .Select(a => new KeyValuePair<string, string>(a.Name, a.Value))
            .ToList();
        record.Elements.Add(new ElementRecord(el.Name, depth, attrs));

        foreach (var child in el.ChildNodes.OfType<XmlElement>())
            Collect(child, depth + 1, record);
    }
}
