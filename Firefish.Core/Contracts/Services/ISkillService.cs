using Firefish.Core.Entities;
using Firefish.Core.Models.Candidate.Requests;
using Firefish.Core.Models.Candidate.Responses;
using Firefish.Core.Models.Skill.Requests;
using Firefish.Core.Models.Skill.Responses;

namespace Firefish.Core.Contracts.Services;

public interface ISkillService
{
    Task<IEnumerable<SkillResponseModel>> GetAllSkillsAsync();
    Task<IEnumerable<CandidateSkillResponseModel>> GetSkillsByCandidateIdAsync(int candidateId);
    Task<IEnumerable<CandidateSkillResponseModel>> AddSkillByCandidateIdAsync(
        CandidateSkillRequestModel candidateSkill
    );
    Task<IEnumerable<CandidateSkillResponseModel>> RemoveSkillByIdAsync(int candidateSkillId);
}
