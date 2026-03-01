using System.ComponentModel.DataAnnotations;

namespace LumenForgeServer.IntegrationTests.TestSupport;

public class CreateTestUserDto
{
    [Required]
    [MinLength(1)]
    [RegularExpression(@".*\S.*")]
    public required string Username { get; init; }

    [Required]
    [MinLength(1)]
    [RegularExpression(@".*\S.*")]
    public required string Password { get; init; }

    [Required]
    [MinLength(1)]
    [RegularExpression(@".*\S.*")]
    public required string Email { get; init; }

    [Required]
    [MinLength(1)]
    [RegularExpression(@".*\S.*")]
    public required string FirstName { get; init; }

    [Required]
    [MinLength(1)]
    [RegularExpression(@".*\S.*")]
    public required string LastName { get; init; }

    public string[] Groups { get; init; } = [];
    public string[] RealmRoles { get; init; } = [];

    public static CreateTestUserDto CreateTestUser()
    {
        var guid = Guid.NewGuid().ToString("N");
        return new CreateTestUserDto
        {
            Username = $"TestUser{guid}",
            Password = "testtest1423",
            Email = $"test{guid}@test.de",
            FirstName = $"Test{guid}",
            LastName = $"Last{guid}"
        };
    }
}