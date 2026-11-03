namespace FacultyDay.Engine.Strategies;

/// <summary>Реєстр доступних стратегій (для вибору в інтерфейсі).</summary>
public static class XmlSearchStrategies
{
    public static IReadOnlyList<IXmlSearchStrategy> All { get; } = new IXmlSearchStrategy[]
    {
        new SaxSearchStrategy(),
        new DomSearchStrategy(),
        new LinqSearchStrategy()
    };
}
