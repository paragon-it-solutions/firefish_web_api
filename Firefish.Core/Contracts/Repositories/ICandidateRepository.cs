using Firefish.Core.Entities;

namespace Firefish.Core.Contracts.Repositories;

public interface ICandidateRepository
{
    Task<Candidate?> GetCandidateById(int candidateId);
    Task<List<Candidate>> GetAllCandidates();
    Task<Candidate> CreateCandidate(Candidate candidate);
    Task UpdateExistingCandidate(Candidate candidate);
}