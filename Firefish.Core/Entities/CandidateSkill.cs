namespace Firefish.Core.Entities;

/// <summary>
///     Candidate class representing a CandidateSkill entity from the database.
///     This is the bridging table between the Candidate and Skill tables (many-many).
/// </summary>
/// <remarks>
///     All non-nullable properties are marked with the "required" attribute and
///     all nullable properties are marked as nullable reference types with "?".
/// </remarks>
public class CandidateSkill
{
    public required int Id { get; set; }
    public required int CandidateId { get; set; }
    public required DateTime CreatedDate { get; set; }
    public required DateTime UpdatedDate { get; set; }
    public required int SkillId { get; set; }
}