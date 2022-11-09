using AuthenticationMicroservice.Models.Request;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Newtonsoft.Json;
using Services.Dto;
using System.Net;
using Xunit;

namespace AuthenticationIntegrationTests
{
    public class UsersControllerIntegrationTestsWhenUser : IClassFixture<CustomApplicationFactory>, IAsyncLifetime
    {
        private readonly HttpClient _client;

        private const string getUsersEndpointUrl = "/api/v2/Users";
        private const string getUserByIdEndpointUrl = "/api/v2/Users/";
        private const string putUserByIdEndpointUrl = "/api/v2/Users/";
        private const string deleteUserByIdEndpointUrl = "/api/v2/Users/";
        private const string putUserNameByIdEndpointUrl = "/api/v2/Users/name/";
        private const string putUsersEmailByIdEndpointUrl = "/api/v2/Users/email/";
        private const string putUsersPasswordByIdEndpointUrl = "/api/v2/Users/password/";
        private const string putUsersResetPasswordByIdEndpointUrl = "/api/v2/Users/reset/";

        private const string loginEndpointUrl = "/api/v1/Login";

        public UsersControllerIntegrationTestsWhenUser(CustomApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            var userData = new UserLoginModel()
            {
                Username = "User",
                Password = "useruser"
            };

            var responseTokens = await _client.PostAsJsonAsync(loginEndpointUrl, userData);
            string responseBodyTokens = await responseTokens.Content.ReadAsStringAsync();
            var responseWithValidDataTokens = JsonConvert.DeserializeObject<TokenDto>(responseBodyTokens)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{responseWithValidDataTokens.AccessToken}");
        }

        [Fact]
        public async Task GetAllAsync_WhenRequestValidButRoleInvalid_Returns_Forbidden()
        {
            // Act
            var response = await _client.GetAsync(getUsersEndpointUrl);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetByIdAsync_WhenRequestValid_Returns_Ok()
        {
            // Act
            var response = await _client.GetAsync(getUserByIdEndpointUrl + "0da216b5-873d-491f-9641-a6b9ebaf7ee3");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetByIdAsync_WhenRequestInvalid_Returns_NotFound()
        {
            // Act
            var response = await _client.GetAsync(getUserByIdEndpointUrl + Guid.NewGuid());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateAsync_WhenRequestValid_Returns_Ok()
        {
            // Arrange
            UserModel userModel = new()
            {
                Username = "NewName",
                EmailAddress = "newname@gmail.com",
                Password = "newpassword1234",
                PasswordRepeated = "newpassword1234"
            };

            // Act
            var response = await _client.PutAsJsonAsync(putUserByIdEndpointUrl + "071fc8e4-28b9-4bf4-b169-9ef3a063ce8b", userModel);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateAsync_WhenRequestWithInvalidId_Returns_NotFound()
        {
            // Arrange
            UserModel userModel = new()
            {
                Username = "NewName",
                EmailAddress = "newname@gmail.com",
                Password = "newpassword1234",
                PasswordRepeated = "newpassword1234"
            };

            // Act
            var response = await _client.PutAsJsonAsync(putUserByIdEndpointUrl + Guid.NewGuid(), userModel);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateAsync_WhenRequestWithExistedName_Returns_NotFound()
        {
            // Arrange
            UserModel userModel = new()
            {
                Username = "Sonic",
                EmailAddress = "newname@gmail.com",
                Password = "newpassword1234",
                PasswordRepeated = "newpassword1234"
            };

            // Act
            var response = await _client.PutAsJsonAsync(putUserByIdEndpointUrl + "071fc8e4-28b9-4bf4-b169-9ef3a063ce8b", userModel);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateNameAsync_WhenRequestValid_Returns_Ok()
        {
            // Arrange
            UserNameModel userNameModel = new() { Username = "NewTestName" };

            // Act
            var response = await _client.PutAsJsonAsync(putUserNameByIdEndpointUrl + "0da216b5-873d-491f-9641-a6b9ebaf7ee3", userNameModel);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateNameAsync_WhenRequestWithIdInvalid_Returns_NotFound()
        {
            // Arrange
            UserNameModel userNameModel = new() { Username = "AbsolutelyNewTestName" };

            // Act
            var response = await _client.PutAsJsonAsync(putUserNameByIdEndpointUrl + Guid.NewGuid(), userNameModel);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateNameAsync_WhenRequestWithExistedName_Returns_NotFound()
        {
            // Arrange
            UserNameModel userNameModel = new() { Username = "AnotherTestUserName" };

            // Act
            var response = await _client.PutAsJsonAsync(putUserNameByIdEndpointUrl + "94749344-4583-406e-9e76-846c39e6b6d2", userNameModel);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateEmailAsync_WhenRequestValid_Returns_Ok()
        {
            // Arrange
            UserEmailModel userEmailModel = new() { EmailAddress = "testemail@gmail.com" };

            // Act
            var response = await _client.PutAsJsonAsync(putUsersEmailByIdEndpointUrl + "cefb4981-6f36-4405-b633-361270085433", userEmailModel);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateEmailAsync_WhenRequestWithInvalidId_Returns_NotFound()
        {
            // Arrange
            UserEmailModel userEmailModel = new() { EmailAddress = "testemail@gmail.com" };

            // Act
            var response = await _client.PutAsJsonAsync(putUsersEmailByIdEndpointUrl + Guid.NewGuid(), userEmailModel);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdatePasswordAsync_WhenRequestValid_Returns_Ok()
        {
            // Arrange
            UserPasswordModel userPasswordModel = new() { Password = "AnyValidPassword" };

            // Act
            var response = await _client.PutAsJsonAsync(putUsersPasswordByIdEndpointUrl + "071fc8e4-28b9-4bf4-b169-9ef3a063ce8b", userPasswordModel);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdatePasswordAsync_WhenRequestWithInvalidId_Returns_NotFound()
        {
            // Arrange
            UserPasswordModel userPasswordModel = new() { Password = "AnyValidPassword" };

            // Act
            var response = await _client.PutAsJsonAsync(putUsersPasswordByIdEndpointUrl + Guid.NewGuid(), userPasswordModel);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ResetPasswordAsync_WhenRequestValidButRoleInvalid_Returns_Forbidden()
        {
            // Act
            // Any data model
            var response = await _client.PutAsJsonAsync(putUsersResetPasswordByIdEndpointUrl + "071fc8e4-28b9-4bf4-b169-9ef3a063ce8b", new UserModel());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteAsync_WhenRequestValid_Returns_NoContent()
        {
            // Act
            var response = await _client.DeleteAsync(deleteUserByIdEndpointUrl + "071fc8e4-28b9-4bf4-b169-9ef3a063ce8b");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteAsync_WhenRequestValid_Returns_NotFound()
        {
            // Act
            var response = await _client.DeleteAsync(deleteUserByIdEndpointUrl + Guid.NewGuid());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        public async Task DisposeAsync() => await Task.CompletedTask;
    }
}
