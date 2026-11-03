using System.Xml;

namespace FacultyDay.Engine.Services;

/// <summary>
/// «Підгрузка даних»: після вибору файлу знаходить усі атрибути, які реально присутні
/// у вузлах Event та їхніх нащадках. Саме з них користувач обирає параметри пошуку.
/// </summary>
public static class AttributeScanner
{
    public static IReadOnlyList<string> Scan(string xmlPath)
    {
        var names = new List<string>();
        var seen = new HashSet<string>();
        var settings = new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true };
        int eventDepth = -1;

        using var reader = XmlReader.Create(xmlPath, settings);
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                bool inEvent = eventDepth >= 0;
                if (!inEvent && reader.Name == XmlNames.Event)
                {
                    eventDepth = reader.Depth;
                    inEvent = true;
                }

                if (inEvent && reader.HasAttributes)
                {
                    while (reader.MoveToNextAttribute())
                    {
                        if (reader.Prefix == "xmlns" || reader.Name == "xmlns") continue;
                        if (seen.Add(reader.Name)) names.Add(reader.Name);
                    }
                    reader.MoveToElement();
                }

                if (inEvent && reader.IsEmptyElement && reader.Depth == eventDepth)
                    eventDepth = -1;
            }
            else if (reader.NodeType == XmlNodeType.EndElement && reader.Depth == eventDepth)
            {
                eventDepth = -1;
            }
        }
        return names;
    }
}
