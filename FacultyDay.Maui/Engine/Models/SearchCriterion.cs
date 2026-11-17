namespace FacultyDay.Engine.Models;

public sealed record SearchCriterion(string Attribute, string Keyword);

public static class SearchMatcher
{
    public static bool Matches(EventRecord ev, IEnumerable<SearchCriterion> criteria) =>
        criteria.All(c => ev.Elements.Any(el => el.Attributes.Any(a =>
            a.Key == c.Attribute &&
            a.Value.Contains(c.Keyword, StringComparison.OrdinalIgnoreCase))));
}
