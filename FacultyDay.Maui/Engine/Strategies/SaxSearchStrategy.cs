using System.Xml;
using FacultyDay.Engine.Models;

namespace FacultyDay.Engine.Strategies;

/// <summary>
/// SAX-підхід: потокове читання «вперед» без завантаження документа в пам'ять.
/// У .NET роль SAX виконує XmlReader (pull-модель; SAX у Java — push-модель, але ідея та сама).
/// </summary>
public sealed class SaxSearchStrategy : IXmlSearchStrategy
{
    public string Name => "SAX (XmlReader)";

    public IReadOnlyList<EventRecord> Search(string xmlPath, IReadOnlyList<SearchCriterion> criteria)
    {
        var result = new List<EventRecord>();
        var settings = new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true };

        EventRecord? current = null;
        int eventDepth = -1;

        void Finish()
        {
            if (SearchMatcher.Matches(current!, criteria))
                result.Add(current!);
            current = null;
            eventDepth = -1;
        }

        using var reader = XmlReader.Create(xmlPath, settings);
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (current is null)
                    {
                        if (reader.Name != XmlNames.Event) break;
                        current = new EventRecord();
                        eventDepth = reader.Depth;
                    }
                    current.Elements.Add(ReadElement(reader, reader.Depth - eventDepth));
                    if (reader.IsEmptyElement && reader.Depth == eventDepth)
                        Finish();
                    break;

                case XmlNodeType.EndElement:
                    if (current is not null && reader.Depth == eventDepth)
                        Finish();
                    break;
            }
        }
        return result;
    }

    private static ElementRecord ReadElement(XmlReader reader, int depth)
    {
        string name = reader.Name;
        var attrs = new List<KeyValuePair<string, string>>();
        if (reader.HasAttributes)
        {
            while (reader.MoveToNextAttribute())
                attrs.Add(new KeyValuePair<string, string>(reader.Name, reader.Value));
            reader.MoveToElement();
        }
        return new ElementRecord(name, depth, attrs);
    }
}
