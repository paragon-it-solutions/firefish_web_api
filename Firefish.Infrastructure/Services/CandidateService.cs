using Firefish.Core.Contracts.Repositories;
using Firefish.Core.Contracts.Services;
using Firefish.Core.Entities;
using Firefish.Core.Mappers;
using Firefish.Core.Models.Candidate.Requests;
using Firefish.Core.Models.Candidate.Responses;

namespace Firefish.Infrastructure.Services;

// Exception handling fairly barebones, should use logger. Eror handling will be handled better at Controller layer in API
public class CandidateService(ICandidateRepository candidateRepository) : ICandidateService
{
    /// <summary>
    ///     Retrieves all candidates from the repository.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     an IEnumerable of CandidateListItemResponseModel representing all candidates.
    /// </returns>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving candidates.</exception>
    public async Task<IEnumerable<CandidateListItemResponseModel>> GetAllCandidates()
    {
        try
        {
            var candidates = await candidateRepository.GetAllCandidatesAsync();
            return candidates.Select(CandidateMapper.MapToCandidateListItemResponse).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while getting all candidates", ex);
        }
    }

    /// <summary>
    ///     Retrieves a specific candidate by their ID.
    /// </summary>
    /// <param name="candidateId">The unique identifier of the candidate to retrieve.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     a CandidateDetailsResponseModel representing the requested candidate.
    /// </returns>
    /// <exception cref="Exception">Thrown when the candidate is not found or an error occurs during retrieval.</exception>
    public async Task<CandidateDetailsResponseModel> GetCandidateById(int candidateId)
    {
        try
        {
            Candidate? candidate = await candidateRepository.GetCandidateByIdAsync(candidateId);

            if (candidate == null)
            {
                throw new Exception("Candidate not found.");
            }

            return CandidateMapper.MapToCandidateDetailsResponse(candidate);
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"Error occurred while getting candidate with ID {candidateId}",
                ex
            );
        }
    }

    /// <summary>
    ///     Creates a new candidate in the repository.
    /// </summary>
    /// <param name="candidateRequestModel">The CandidateModifyRequestModel containing the data for the new candidate.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     a CandidateDetailsResponseModel representing the newly created candidate.
    /// </returns>
    /// <exception cref="Exception">Thrown when the candidate creation fails or an error occurs during the process.</exception>
    public async Task<CandidateDetailsResponseModel> CreateCandidate(
        CandidateModifyRequestModel candidateRequestModel
    )
    {
        try
        {
            Candidate candidate = CandidateMapper.MapToEntity(candidateRequestModel);
            Candidate createdCandidate = await candidateRepository.CreateCandidateAsync(candidate);

            if (createdCandidate == null || createdCandidate.Id == 0 || createdCandidate.Id == null)
            {
                throw new Exception("Failed to create candidate.");
            }

            return CandidateMapper.MapToCandidateDetailsResponse(createdCandidate);
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while creating candidate", ex);
        }
    }

    /// <summary>
    ///     Updates an existing candidate in the repository.
    /// </summary>
    /// <param name="candidateRequestModel">The CandidateModifyRequestModel containing the updated data for the candidate.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="Exception">Thrown when an error occurs during the update process.</exception>
    public async Task UpdateExistingCandidate(CandidateModifyRequestModel candidateRequestModel)
    {
        try
        {
            Candidate candidate = CandidateMapper.MapToEntity(candidateRequestModel);
            await candidateRepository.UpdateExistingCandidateAsync(candidate);
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while updating candidate", ex);
        }
    }
}
