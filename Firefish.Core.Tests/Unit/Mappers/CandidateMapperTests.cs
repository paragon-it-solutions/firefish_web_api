using Firefish.Core.Entities;
using Firefish.Core.Mappers;

namespace Firefish.Core.Tests.Unit.Mappers;

public class CandidateMapperTests
{
    [Fact]
    public void MapToCandidateDetailsResponse_ShouldCorrectlyMapAllFields()
    {
        // Arrange
        var candidate = new Candidate
        {
            Id = 1,
            FirstName = "John",
            Surname = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Address = "123 Main St",
            Town = "Anytown",
            Country = "USA",
            PostCode = "12345",
            PhoneHome = "555-1234",
            PhoneMobile = "555-5678",
            PhoneWork = "555-9012",
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            UpdatedDate = DateTime.UtcNow,
        };

        // Act
        var result = CandidateMapper.MapToCandidateDetailsResponse(candidate);

        // Assert
        Assert.Equal(candidate.Id, result.Id);
        Assert.Equal($"{candidate.FirstName} {candidate.Surname}".Trim(), result.Name);
        Assert.Equal(candidate.DateOfBirth, result.DateOfBirth);
        Assert.Equal(candidate.Address, result.Address);
        Assert.Equal(candidate.Town, result.Town);
        Assert.Equal(candidate.Country, result.Country);
        Assert.Equal(candidate.PostCode, result.PostCode);
        Assert.Equal(candidate.PhoneHome, result.PhoneHome);
        Assert.Equal(candidate.PhoneMobile, result.PhoneMobile);
        Assert.Equal(candidate.PhoneWork, result.PhoneWork);
        Assert.Equal(candidate.CreatedDate, result.CreatedDate);
        Assert.Equal(candidate.UpdatedDate, result.UpdatedDate);
    }

    [Fact]
    public void MapToCandidateDetailsResponse_ShouldHandleNullValuesGracefully()
    {
        // Arrange
        var candidate = new Candidate
        {
            Id = 1,
            FirstName = null,
            Surname = null,
            DateOfBirth = DateTime.Today,
            Address = null,
            Town = null,
            Country = null,
            PostCode = null,
            PhoneHome = null,
            PhoneMobile = null,
            PhoneWork = null,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
        };

        // Act
        var result = CandidateMapper.MapToCandidateDetailsResponse(candidate);

        // Assert
        Assert.Equal(1, result.Id);
        Assert.Equal(string.Empty, result.Name);
        Assert.Null(result.Address);
        Assert.Null(result.Town);
        Assert.Null(result.Country);
        Assert.Null(result.PostCode);
        Assert.Null(result.PhoneHome);
        Assert.Null(result.PhoneMobile);
        Assert.Null(result.PhoneWork);
    }
}
