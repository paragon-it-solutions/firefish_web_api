namespace Firefish.Core.Entities;

/// <summary>
///     Candidate class representing a Skill entity from the database.
/// </summary>
/// <remarks>
///     All non-nullable properties are marked with the "required" attribute and
///     all nullable properties are marked as nullable reference types with "?".
/// </remarks>
public class Skill
{
    public required int Id { get; set; }
    public string? Name { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}