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
    public class LoginControllerIntegrationTests : IClassFixture<CustomApplicationFactory>, IAsyncLifetime
    {
        private readonly HttpClient _client;

        private const string isExistEndpointUrl = "/api/v1/Exist/";
        private const string loginEndpointUrl = "/api/v1/Login";
        private const string tokenEndpointUrl = "/api/v1/RefreshToken";
        private const string registerEndpointUrl = "/api/v1/Register";

        private TokenDto _tokenDto = null!;

        public LoginControllerIntegrationTests(CustomApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            // Authentication when Administrator login
            var userData = new UserLoginModel()
            {
                Username = "Sonic",
                Password = "administrator"
            };

            var responseTokens = await _client.PostAsJsonAsync(loginEndpointUrl, userData);
            string responseBodyTokens = await responseTokens.Content.ReadAsStringAsync();
            _tokenDto = JsonConvert.DeserializeObject<TokenDto>(responseBodyTokens)!;
        }


        [Fact]
        public async Task IsExistAsync_WhenRequestValid_Returns_Ok()
        {
            // Arrange
            string existName = "Sonic";

            // Act
            var responseExist = await _client.GetFromJsonAsync<bool>(isExistEndpointUrl + existName);

            // Assert
            responseExist.Should().BeTrue();
        }

        [Fact]
        public async Task LoginAsync_WhenRequestValid_Returns_Ok()
        {
            // Arrange
            var userData = new UserLoginModel()
            {
                Username = "Sonic",
                Password = "administrator"
            };

            // Act
            var response = await _client.PostAsJsonAsync(loginEndpointUrl, userData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            string responseBody = await response.Content.ReadAsStringAsync();
            var responseWithValidData = JsonConvert.DeserializeObject<TokenDto>(responseBody)!;

            responseWithValidData.AccessToken.Should().NotBeNullOrEmpty();
            responseWithValidData.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task LoginAsync_WhenRequestWithInvalidData_Returns_NotFound()
        {
            // Arrange
            var userDataWithInvalidData = new UserLoginModel()
            {
                Username = "SomeNotExistedName",
                Password = "InvalidPassword"
            };

            // Act
            var responseWithInvalidData = await _client.PostAsJsonAsync(loginEndpointUrl, userDataWithInvalidData);

            // Assert
            responseWithInvalidData.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task LoginAsync_WhenRequestWithEmptyData_Returns_NotFound()
        {
            // Act
            var responseWithEmptyData = await _client.PostAsJsonAsync(loginEndpointUrl, new UserLoginModel());

            // Assert
            responseWithEmptyData.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordInvalid_Returns_Unauthorized()
        {
            // Arrange
            var userDataWithInvalidPassword = new UserLoginModel()
            {
                Username = "Sonic",
                Password = "InvalidPassword"
            };

            // Act
            var responseWithInvalidPassword = await _client.PostAsJsonAsync(loginEndpointUrl, userDataWithInvalidPassword);
            
            // Assert
            responseWithInvalidPassword.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenRequestValid_Returns_OK()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{_tokenDto.AccessToken}");

            // Act
            var responseRefresh = await _client.PostAsJsonAsync(tokenEndpointUrl, _tokenDto);
            string responseBodyRefresh = await responseRefresh.Content.ReadAsStringAsync();
            var responseWithValidDataRefresh = JsonConvert.DeserializeObject<TokenDto>(responseBodyRefresh)!;

            // Assert
            responseRefresh.StatusCode.Should().Be(HttpStatusCode.OK);

            responseWithValidDataRefresh.AccessToken.Should().NotBeNullOrEmpty();
            responseWithValidDataRefresh.RefreshToken.Should().NotBeNullOrEmpty();

            responseWithValidDataRefresh.Should().NotBeEquivalentTo(_tokenDto);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenRequestInvalid_Returns_NotFound()
        {
            // Arrange
            var tokenDataWithInvalidParameters = new TokenDto()
            {
                AccessToken = "ValidAccessToken",
                RefreshToken = "InvalidRefreshToken"
            };

            // Act
            var responseWithInvalidData = await _client.PostAsJsonAsync(tokenEndpointUrl, tokenDataWithInvalidParameters);

            // Assert
            responseWithInvalidData.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenRequestEmpty_Returns_NotFound()
        {
            // Arrange
            var tokenDataWithEmptyParameters = new TokenDto()
            {
                AccessToken = "",
                RefreshToken = ""
            };

            // Act
            var responseWithEmptyData = await _client.PostAsJsonAsync(tokenEndpointUrl, tokenDataWithEmptyParameters);

            // Assert
            responseWithEmptyData.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task RegisterAsync_WhenRequestValid_Returns_Ok()
        {
            // Arrange
            var registrationDataWithValidParameters = new UserModel()
            {
                Username = "TestName",
                EmailAddress = "testemail@gmail.com",
                Password = "password12345",
                PasswordRepeated = "password12345"
            };

            // Act
            var postResponse = await _client.PostAsJsonAsync(registerEndpointUrl, registrationDataWithValidParameters);

            // Assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public async Task DisposeAsync() => await Task.CompletedTask;
    }
}
