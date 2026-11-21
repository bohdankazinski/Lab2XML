namespace FacultyDay.Engine.Strategies;

public static class XmlSearchStrategies
{
    public static IReadOnlyList<IXmlSearchStrategy> All { get; } = new IXmlSearchStrategy[]
    {
        new SaxSearchStrategy(),
        new DomSearchStrategy(),
        new LinqSearchStrategy()
    };
}
