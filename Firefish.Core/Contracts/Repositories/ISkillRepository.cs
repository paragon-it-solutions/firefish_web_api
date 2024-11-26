using Firefish.Core.Entities;

namespace Firefish.Core.Contracts.Repositories;

public interface ISkillRepository
{
    Task<IEnumerable<CandidateSkill>> GetSkillsByCandidateIdAsync(int candidateId);
    Task<IEnumerable<CandidateSkill>> AddSkillByCandidateIdAsync(int candidateId, int skillId);
    Task<IEnumerable<CandidateSkill>> RemoveSkillByIdAsync(int candidateSkillId);
    Task<bool> SkillExistsAsync(int skillId);
    Task<bool> SkillExistsForCandidateAsync(int skillId, int candidateId);
}
