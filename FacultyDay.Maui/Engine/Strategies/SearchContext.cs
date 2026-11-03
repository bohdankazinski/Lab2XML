using FacultyDay.Engine.Models;

namespace FacultyDay.Engine.Strategies;

/// <summary>Контекст шаблону «Стратегія»: не знає, як саме читається XML, лише делегує поточній стратегії.</summary>
public sealed class SearchContext
{
    public IXmlSearchStrategy Strategy { get; set; } = XmlSearchStrategies.All[0];

    public IReadOnlyList<EventRecord> Execute(string xmlPath, IReadOnlyList<SearchCriterion> criteria) =>
        Strategy.Search(xmlPath, criteria);
}
