using Firefish.Core.Models.Candidate.Requests;
using Firefish.Core.Models.Candidate.Responses;

namespace Firefish.Core.Contracts.Services;

public interface ICandidateService
{
    Task<IEnumerable<CandidateListItemResponseModel>> GetAllCandidatesAsync();
    Task<CandidateDetailsResponseModel> GetCandidateByIdAsync(int candidateId);
    Task<CandidateDetailsResponseModel> CreateCandidateAsync(
        CandidateModifyRequestModel candidateModel
    );
    Task<CandidateDetailsResponseModel> UpdateExistingCandidateAsync(
        int candidateId,
        CandidateModifyRequestModel candidateModel
    );
}
