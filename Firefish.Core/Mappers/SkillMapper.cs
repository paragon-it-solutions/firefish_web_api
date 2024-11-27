using Firefish.Core.Entities;
using Firefish.Core.Models.Skill.Responses;

namespace Firefish.Core.Mappers;

/// <summary>
///     Provides mapping functionality between Skill-related entities and models.
/// </summary>
public static class SkillMapper
{
    /// <summary>
    ///     Maps a CandidateSkill entity to a CandidateSkillResponseModel.
    /// </summary>
    /// <param name="candidateSkill">The CandidateSkill entity to map from.</param>
    /// <returns>A new CandidateSkillResponseModel.</returns>
    public static CandidateSkillResponseModel MapToCandidateSkillResponseModel(
        CandidateSkill candidateSkill
    )
    {
        return new CandidateSkillResponseModel
        {
            CandidateSkillId = candidateSkill.Id,
            SkillId = candidateSkill.SkillId,
            Name = candidateSkill.SkillName,
        };
    }

    /// <summary>
    ///     Maps a Skill entity to a CandidateSkillResponseModel.
    /// </summary>
    /// <param name="skill">The SKill entity to map from.</param>
    /// <returns>A new CandidateSkillResponseModel.</returns>
    public static SkillResponseModel MapToSkillResponseModel(Skill skill)
    {
        return new SkillResponseModel { SkillId = skill.Id, Name = skill.Name! };
    }
}
