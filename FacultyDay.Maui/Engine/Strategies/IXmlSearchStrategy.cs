using FacultyDay.Engine.Models;

namespace FacultyDay.Engine.Strategies;

/// <summary>Шаблон «Стратегія»: спільний контракт для трьох способів аналізу XML.</summary>
public interface IXmlSearchStrategy
{
    /// <summary>Назва для відображення в інтерфейсі.</summary>
    string Name { get; }

    IReadOnlyList<EventRecord> Search(string xmlPath, IReadOnlyList<SearchCriterion> criteria);
}
