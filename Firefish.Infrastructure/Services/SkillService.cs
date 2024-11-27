using Firefish.Core.Contracts.Repositories;
using Firefish.Core.Contracts.Services;
using Firefish.Core.Mappers;
using Firefish.Core.Models.Skill.Requests;
using Firefish.Core.Models.Skill.Responses;

namespace Firefish.Infrastructure.Services;

public class SkillService(ISkillRepository skillRepository) : ISkillService
{
    /// <summary>
    /// Retrieves all skills available in the system.
    /// </summary>
    /// <returns>
    /// A collection of SkillResponseModel representing all the skills in the system.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when an error occurs during the retrieval of skills.
    /// </exception>
    public async Task<IEnumerable<SkillResponseModel>> GetAllSkillsAsync()
    {
        try
        {
            var skills = await skillRepository.GetAllSkillsAsync();
            return skills.Select(SkillMapper.MapToSkillResponseModel).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving skills: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Retrieves all skills associated with a specific candidate.
    /// </summary>
    /// <param name="candidateId">The ID of the candidate.</param>
    /// <returns>A collection of CandidateSkillResponseModel representing the skills associated with the candidate.</returns>
    public async Task<IEnumerable<CandidateSkillResponseModel>> GetSkillsByCandidateIdAsync(
        int candidateId
    )
    {
        try
        {
            var skills = await skillRepository.GetSkillsByCandidateIdAsync(candidateId);
            return skills.Select(SkillMapper.MapToCandidateSkillResponseModel).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving skills for candidate: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Adds a skill to a candidate's profile.
    /// </summary>
    /// <param name="candidateSkill">
    /// A <see cref="CandidateSkillRequestModel"/> containing the candidate ID and skill ID to be added.
    /// </param>
    /// <returns>
    /// A collection of <see cref="CandidateSkillResponseModel"/> representing the updated list of skills
    /// associated with the candidate after the new skill has been added.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the specified skill does not exist in the system.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown when an error occurs during the process of adding the skill to the candidate.
    /// </exception>
    public async Task<IEnumerable<CandidateSkillResponseModel>> AddSkillByCandidateIdAsync(
        CandidateSkillRequestModel candidateSkill
    )
    {
        try
        {
            var updatedSkills = await skillRepository.AddSkillByCandidateIdAsync(
                candidateSkill.CandidateId,
                candidateSkill.SkillId
            );
            return updatedSkills.Select(SkillMapper.MapToCandidateSkillResponseModel).ToList();
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error adding candidateSkill to candidate: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Removes a skill from a candidate's profile by the candidate skill ID.
    /// </summary>
    /// <param name="candidateSkillId">The unique identifier of the candidate skill to be removed.</param>
    /// <returns>
    /// A collection of <see cref="CandidateSkillResponseModel"/> representing the remaining skills
    /// associated with the candidate after removal.
    /// </returns>
    /// <exception cref="Exception">Thrown when an error occurs during the skill removal process.</exception>
    public async Task<IEnumerable<CandidateSkillResponseModel>> RemoveSkillByIdAsync(
        int candidateSkillId
    )
    {
        try
        {
            var updatedSkills = await skillRepository.RemoveSkillByIdAsync(candidateSkillId);
            return updatedSkills.Select(SkillMapper.MapToCandidateSkillResponseModel);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error removing candidateSkill from candidate: {ex.Message}", ex);
        }
    }
}
