using Firefish.Core.Entities;

namespace Firefish.Core.Contracts.Repositories;

public interface ICandidateRepository
{
    Task<IEnumerable<Candidate>> GetAllCandidatesAsync();
    Task<Candidate?> GetCandidateByIdAsync(int candidateId);
    Task<Candidate> CreateCandidateAsync(Candidate candidate);
    Task UpdateExistingCandidateAsync(Candidate candidate);
}