using Firefish.Core.Entities;

namespace Firefish.Core.Contracts.Repositories;

public interface ISkillRepository
{
    Task<IEnumerable<Skill>> GetSkillsByCandidateIdAsync(int candidateId);
    Task<IEnumerable<Skill>> AddSkillByCandidateIdAsync(int candidateId, int skillId);
    Task<IEnumerable<Skill>> RemoveSkillByCandidateIdAsync(int candidateId, int skillId);
    Task<bool> SkillExistsAsync(int skillId);
}
