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
    public int Id { get; set; }
    public required int CandidateId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public required int SkillId { get; set; }

    // SkillName is not a live field on DB - it is determined by the join between CandidateSkill and Skill tables.
    public string SkillName { get; set; }
}
