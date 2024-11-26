using Firefish.Core.Models.Candidate.Requests;
using Firefish.Core.Models.Candidate.Responses;

namespace Firefish.Core.Contracts.Services;

public interface ICandidateService
{
    Task<List<CandidateListItemResponseModel>> GetAllCandidates();
    Task<CandidateDetailsResponseModel> GetCandidateById(int candidateId);
    Task<CandidateDetailsResponseModel> CreateCandidate(CandidateCreateOrUpdateRequestModel candidate);
    Task UpdateExistingCandidate(CandidateCreateOrUpdateRequestModel candidate);
}