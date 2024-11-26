using Firefish.Core.Entities;
using Firefish.Core.Models.Skill.Responses;

namespace Firefish.Core.Mappers;

/// <summary>
///     Provides mapping functionality between Skill-related entities and models.
/// </summary>
public static class SkillMapper
{
    /// <summary>
    ///     Maps a CandidateSkill entity to a SkillResponseModel.
    /// </summary>
    /// <param name="candidateSkill">The CandidateSkill entity to map from.</param>
    /// <returns>A new SkillResponseModel.</returns>
    public static SkillResponseModel MapToSkillResponseModel(CandidateSkill candidateSkill)
    {
        return new SkillResponseModel
        {
            Id = candidateSkill.SkillId,
            Name = candidateSkill.SkillName,
        };
    }

    /// <summary>
    ///     Maps a Skill entity to a SkillResponseModel.
    /// </summary>
    /// <param name="skill">The CandidateSkill entity to map from.</param>
    /// <returns>A new SkillResponseModel.</returns>
    public static SkillResponseModel MapToSkillResponseModel(Skill skill)
    {
        return new SkillResponseModel { Id = skill.Id, Name = skill.Name! };
    }
}
