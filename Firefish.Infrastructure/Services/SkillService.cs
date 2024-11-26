using Firefish.Core.Contracts.Repositories;
using Firefish.Core.Contracts.Services;
using Firefish.Core.Mappers;
using Firefish.Core.Models.Skill.Requests;
using Firefish.Core.Models.Skill.Responses;

namespace Firefish.Infrastructure.Services;

public class SkillService(ISkillRepository skillRepository) : ISkillService
{
    /// <summary>
    /// Retrieves all skills associated with a specific candidate.
    /// </summary>
    /// <param name="candidateId">The ID of the candidate.</param>
    /// <returns>A collection of SkillResponseModel representing the skills associated with the candidate.</returns>
    public async Task<IEnumerable<SkillResponseModel>> GetSkillsByCandidateIdAsync(int candidateId)
    {
        try
        {
            var skills = await skillRepository.GetSkillsByCandidateIdAsync(candidateId);
            return skills.Select(SkillMapper.MapToSkillResponseModel).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving skills for candidate: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Adds a candidateSkill to a candidate's profile.
    /// </summary>
    /// <param name="candidateSkill">The CandidateSkillRequestModel containing the candidate ID and candidateSkill ID.</param>
    /// <returns>An updated collection of SkillResponseModel representing the skills associated with the candidate.</returns>
    public async Task<IEnumerable<SkillResponseModel>> AddSkillByCandidateIdAsync(
        CandidateSkillRequestModel candidateSkill
    )
    {
        try
        {
            if (!await skillRepository.SkillExistsAsync(candidateSkill.SkillId))
            {
                throw new ArgumentException(
                    $"Skill with ID {candidateSkill.SkillId} does not exist."
                );
            }

            var updatedSkills = await skillRepository.AddSkillByCandidateIdAsync(
                candidateSkill.CandidateId,
                candidateSkill.SkillId
            );
            return updatedSkills.Select(SkillMapper.MapToSkillResponseModel).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error adding candidateSkill to candidate: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Removes a candidateSkill from a candidate's profile.
    /// </summary>
    /// <param name="candidateSkill">The CandidateSkillRequestModel containing the candidate ID and candidateSkill ID.</param>
    /// <returns>An updated collection of SkillResponseModel representing the remaining skills associated with the candidate.</returns>
    public async Task<IEnumerable<SkillResponseModel>> RemoveSkillByCandidateIdAsync(
        CandidateSkillRequestModel candidateSkill
    )
    {
        try
        {
            var updatedSkills = await skillRepository.RemoveSkillByCandidateIdAsync(
                candidateSkill.CandidateId,
                candidateSkill.SkillId
            );
            return updatedSkills.Select(SkillMapper.MapToSkillResponseModel);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error removing candidateSkill from candidate: {ex.Message}", ex);
        }
    }
}
