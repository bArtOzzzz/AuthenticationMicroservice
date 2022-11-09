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
    public class UsersControllerIntegrationTestsWhenAdministrator : IClassFixture<CustomApplicationFactory>, IAsyncLifetime
    {
        private readonly HttpClient _client;

        private const string getUsersEndpointUrl = "/api/v2/Users";
        private const string putUsersResetPasswordByIdEndpointUrl = "/api/v2/Users/reset/";

        private const string loginEndpointUrl = "/api/v1/Login";

        public UsersControllerIntegrationTestsWhenAdministrator(CustomApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            var userData = new UserLoginModel()
            {
                Username = "Sonic",
                Password = "administrator"
            };

            var responseTokens = await _client.PostAsJsonAsync(loginEndpointUrl, userData);
            string responseBodyTokens = await responseTokens.Content.ReadAsStringAsync();
            var responseWithValidDataTokens = JsonConvert.DeserializeObject<TokenDto>(responseBodyTokens)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{responseWithValidDataTokens.AccessToken}");
        }

        [Fact]
        public async Task GetAsync_WhenRequestValid_Returns_Ok()
        {
            // Act
            var response = await _client.GetAsync(getUsersEndpointUrl);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ResetPasswordAsync_WhenRequestValid_Returns_Ok()
        {
            // Act
            var response = await _client.PutAsJsonAsync(putUsersResetPasswordByIdEndpointUrl + "0da216b5-873d-491f-9641-a6b9ebaf7ee3", new UserModel());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public async Task DisposeAsync() => await Task.CompletedTask;
    }
}
