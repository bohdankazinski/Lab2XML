namespace FacultyDay.Engine.Models;

/// <summary>Одна умова пошуку: значення атрибута <see cref="Attribute"/> містить <see cref="Keyword"/>.</summary>
public sealed record SearchCriterion(string Attribute, string Keyword);

/// <summary>
/// Спільне правило збігу (використовує SAX-стратегія, яка перевіряє записи на льоту).
/// Захід підходить, якщо для КОЖНОЇ умови десь у його піддереві є атрибут із потрібним іменем,
/// значення якого містить ключове слово (без урахування регістру).
/// </summary>
public static class SearchMatcher
{
    public static bool Matches(EventRecord ev, IEnumerable<SearchCriterion> criteria) =>
        criteria.All(c => ev.Elements.Any(el => el.Attributes.Any(a =>
            a.Key == c.Attribute &&
            a.Value.Contains(c.Keyword, StringComparison.OrdinalIgnoreCase))));
}
