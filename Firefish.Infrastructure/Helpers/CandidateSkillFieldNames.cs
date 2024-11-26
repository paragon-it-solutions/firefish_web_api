using Firefish.Core.Entities;

namespace Firefish.Infrastructure.Helpers;

/// <summary>
/// Provides constant string representations of field names for the Skill entity.
/// </summary>
/// <remarks>
/// This static class contains string constants that match the property names of the Skill class.
/// These constants can be used for consistent property name references across the application, and
/// to avoid "magic strings" in the code.
/// </remarks>
public static class CandidateSkillFieldNames
{
    public const string Id = nameof(CandidateSkill.Id);
    public const string CandidateId = nameof(CandidateSkill.CandidateId);
    public const string CreatedDate = nameof(CandidateSkill.CreatedDate);
    public const string UpdatedDate = nameof(CandidateSkill.UpdatedDate);
    public const string SkillId = nameof(CandidateSkill.SkillId);
    public const string SkillName = $"{nameof(Skill)}.{nameof(Skill.Name)}"; // SkillName is not a live field on DB - it is determined by the join between CandidateSkill and Skill tables.
}
