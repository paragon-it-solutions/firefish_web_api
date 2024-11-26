using Firefish.Core.Entities;
using Firefish.Core.Models.Skill.Requests;
using Firefish.Core.Models.Skill.Responses;

namespace Firefish.Core.Mappers;

/// <summary>
/// Provides mapping functionality between Skill-related entities and models.
/// </summary>
public static class SkillMapper
{
    /// <summary>
    /// Maps a SkillRequestModel to a CandidateSkill entity.
    /// </summary>
    /// <param name="skillModel">The SkillRequestModel to map from.</param>
    /// <returns>A new CandidateSkill entity.</returns>
    public static CandidateSkill MapToEntity(SkillRequestModel skillModel)
    {
        return new CandidateSkill
        {
            CandidateId = skillModel.CandidateId,
            SkillId = skillModel.SkillId,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
        };
    }

    /// <summary>
    /// Maps a CandidateSkill entity to a SkillResponseModel.
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
}
