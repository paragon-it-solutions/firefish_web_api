using Firefish.Core.Entities;
using Firefish.Core.Models.Candidate.Requests;
using Firefish.Core.Models.Candidate.Responses;

namespace Firefish.Core.Mappers;

public static class CandidateMapper
{
    /// <summary>
    /// Maps a CandidateModifyRequestModel to a new Candidate entity.
    /// </summary>
    /// <param name="model">The CandidateModifyRequestModel containing the candidate's information.</param>
    /// <returns>A new Candidate entity populated with the data from the input model.</returns>
    public static Candidate MapToEntity(CandidateModifyRequestModel model)
    {
        return new Candidate
        {
            FirstName = model.FirstName,
            Surname = model.Surname,
            DateOfBirth = model.DateOfBirth,
            Address = model.Address,
            Town = model.Town,
            Country = model.Country,
            PostCode = model.PostCode,
            PhoneHome = model.PhoneHome,
            PhoneMobile = model.PhoneMobile,
            PhoneWork = model.PhoneWork,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Updates an existing Candidate entity with data from a CandidateModifyRequestModel.
    /// </summary>
    /// <param name="candidate">The existing Candidate entity to be updated.</param>
    /// <param name="model">The CandidateModifyRequestModel containing the updated candidate information.</param>
    public static void MapToCandidateModifyRequest(Candidate candidate, CandidateModifyRequestModel model)
    {
        candidate.FirstName = model.FirstName;
        candidate.Surname = model.Surname;
        candidate.DateOfBirth = model.DateOfBirth;
        candidate.Address = model.Address;
        candidate.Town = model.Town;
        candidate.Country = model.Country;
        candidate.PostCode = model.PostCode;
        candidate.PhoneHome = model.PhoneHome;
        candidate.PhoneMobile = model.PhoneMobile;
        candidate.PhoneWork = model.PhoneWork;
        candidate.UpdatedDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Maps a Candidate entity to a CandidateDetailsResponseModel.
    /// </summary>
    /// <param name="candidate">The Candidate entity to be mapped.</param>
    /// <returns>A CandidateDetailsResponseModel containing the candidate's detailed information.</returns>
    public static CandidateDetailsResponseModel MapToCandidateDetailsResponse(Candidate candidate)
    {
        return new CandidateDetailsResponseModel
        {
            Id = candidate.Id,
            Name = $"{candidate.FirstName} {candidate.Surname}".Trim(),
            DateOfBirth = candidate.DateOfBirth,
            Address = candidate.Address,
            Town = candidate.Town,
            Country = candidate.Country,
            PostCode = candidate.PostCode,
            PhoneHome = candidate.PhoneHome,
            PhoneMobile = candidate.PhoneMobile,
            PhoneWork = candidate.PhoneWork,
            CreatedDate = candidate.CreatedDate,
            UpdatedDate = candidate.UpdatedDate
        };
    }

    /// <summary>
    /// Maps a Candidate entity to a CandidateListItemResponseModel.
    /// </summary>
    /// <param name="candidate">The Candidate entity to be mapped.</param>
    /// <returns>A CandidateListItemResponseModel containing the candidate's summary information for list display.</returns>
    public static CandidateListItemResponseModel MapToCandidateListItemResponse(Candidate candidate)
    {
        return new CandidateListItemResponseModel
        {
            Id = candidate.Id,
            Name = $"{candidate.FirstName} {candidate.Surname}".Trim(),
            DateOfBirth = candidate.DateOfBirth,
            Town = candidate.Town,
            Phone = candidate.PhoneMobile ?? candidate.PhoneHome ?? candidate.PhoneWork
        };
    }
}