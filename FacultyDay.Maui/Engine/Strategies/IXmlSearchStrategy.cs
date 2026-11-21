using FacultyDay.Engine.Models;

namespace FacultyDay.Engine.Strategies;

public interface IXmlSearchStrategy
{
    string Name { get; }

    IReadOnlyList<EventRecord> Search(string xmlPath, IReadOnlyList<SearchCriterion> criteria);
}
