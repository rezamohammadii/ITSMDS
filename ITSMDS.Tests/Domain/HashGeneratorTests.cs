

namespace ITSMDS.Tests.Domain;

using ITSMDS.Domain.Tools;
using Xunit;

public class HashGeneratorTests
{
    [Fact]
    public void GenerateHashSHA512_ShouldReturnCorrectHash()
    {
        // Arrange
        string password = "MySecretPassword";
        string expectedHash = "366d3dfd31710e60eacd90662027cc8dd401faf490d8cb07af2bb921eb6733e8d179bb9aa5ea8787181955e6983cc42527af3156139a705c896df92c3e835323";
        Console.WriteLine(HashGenerator.GenerateHashSHA512(password));


        // Act
        string actualHash = HashGenerator.GenerateHashSHA512(password);

        // Assert
        Assert.Equal(expectedHash, actualHash);
    }

    [Fact]
    public void HashPasswordAndValidate_ShouldReturnTrueForCorrectPassword()
    {
        // Arrange
        string password = "MySecretPassword";

        // Act
        string hashed = HashGenerator.HashPassword(password);
        bool isValid = HashGenerator.IsHashPasswordValid(hashed, password);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void HashPasswordAndValidate_ShouldReturnFalseForWrongPassword()
    {
        // Arrange
        string password = "MySecretPassword";
        string wrongPassword = "WrongPassword";

        // Act
        string hashed = HashGenerator.HashPassword(password);
        bool isValid = HashGenerator.IsHashPasswordValid(hashed, wrongPassword);

        // Assert
        Assert.False(isValid);
    }
}
