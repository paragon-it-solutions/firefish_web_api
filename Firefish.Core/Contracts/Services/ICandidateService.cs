using Firefish.Core.Models.Candidate.Requests;
using Firefish.Core.Models.Candidate.Responses;

namespace Firefish.Core.Contracts.Services;

public interface ICandidateService
{
    Task<IEnumerable<CandidateListItemResponseModel>> GetAllCandidates();
    Task<CandidateDetailsResponseModel> GetCandidateById(int candidateId);
    Task<CandidateDetailsResponseModel> CreateCandidate(CandidateModifyRequestModel candidate);
    Task UpdateExistingCandidate(CandidateModifyRequestModel candidate);
}