using Firefish.Core.Entities;
using Firefish.Core.Models.Candidate.Requests;
using Firefish.Core.Models.Candidate.Responses;
using Firefish.Core.Models.Skill.Requests;
using Firefish.Core.Models.Skill.Responses;

namespace Firefish.Core.Contracts.Services;

public interface ISkillService
{
    Task<IEnumerable<SkillResponseModel>> GetSkillsByCandidateIdAsync(int candidateId);
    Task<IEnumerable<SkillResponseModel>> AddSkillByCandidateIdAsync(
        CandidateSkillRequestModel candidateSkill
    );
    Task<IEnumerable<SkillResponseModel>> RemoveSkillByIdAsync(int candidateSkillId);
}
