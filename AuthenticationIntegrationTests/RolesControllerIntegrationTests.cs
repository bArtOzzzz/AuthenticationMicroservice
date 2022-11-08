using AuthenticationMicroservice.Models.Request;
using System.Net.Http.Json;
using FluentAssertions;
using Newtonsoft.Json;
using Services.Dto;
using System.Net;
using Xunit;

namespace AuthenticationIntegrationTests
{
    public class RolesControllerIntegrationTests : IClassFixture<CustomApplicationFactory>, IAsyncLifetime
    {
        private readonly CustomApplicationFactory _factory;
        private readonly HttpClient _client;

        private const string getRolesEndpointUrl = "/api/v2/Roles";
        private const string postRolesEndpointUrl = "/api/v2/Roles";
        private const string getRolesByIdEndpointUrl = "/api/v2/Roles/";
        private const string putRolesByIdEndpointUrl = "/api/v2/Roles/";
        private const string deleteRolesByIdEndpointUrl = "/api/v2/Roles/";
        private const string putRolesUpdateRoleForUserEndpointUrl = "/api/v2/Roles/updateUserRole/";

        private const string loginEndpointUrl = "/api/v1/Login";

        public RolesControllerIntegrationTests(CustomApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
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
            var responseWithValidDataTokens = JsonConvert.DeserializeObject<TokenDto>(responseBodyTokens)!;

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{responseWithValidDataTokens.AccessToken}");
        }

        [Fact]
        public async Task GetAllAsync_WhenRequestValid_Returns_Ok()
        {
            // Act
            var response = await _client.GetAsync(getRolesEndpointUrl);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetByIdAsync_WhenRequestValid_Returns_Ok()
        {
            // Act
            var response = await _client.GetAsync(getRolesByIdEndpointUrl + "a446525a-41c8-4722-8152-5c72e3efd01d");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetByIdAsync_WhenRequestWithInvalidId_Returns_NotFound()
        {
            // Act
            var response = await _client.GetAsync(getRolesByIdEndpointUrl + Guid.NewGuid());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateAsync_WhenRequestValid_Returns_Ok()
        {
            // Arrange
            RoleModel roleModel = new RoleModel()
            {
                Role = "UltimatePower"
            };

            // Act
            var response = await _client.PostAsJsonAsync(postRolesEndpointUrl, roleModel);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateByUserAsync_WhenRequestValid_Returns_Ok()
        {
            // Arrange
            RoleModel roleModel = new RoleModel()
            {
                Role = "UltimatePower"
            };

            // Act
            var response = await _client.PutAsJsonAsync(putRolesUpdateRoleForUserEndpointUrl + "0da216b5-873d-491f-9641-a6b9ebaf7ee3", roleModel);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateByUserAsync_WhenRequestInvalid_Returns_NotFound()
        {
            // Arrange
            RoleModel roleModel = new RoleModel()
            {
                Role = "UltimatePower"
            };

            // Act
            var response = await _client.PutAsJsonAsync(putRolesUpdateRoleForUserEndpointUrl + Guid.NewGuid(), roleModel);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateAsync_WhenRequestValid_Returns_Ok()
        {
            // Arrange
            RoleModel roleModel = new RoleModel()
            {
                Role = "UltimatePower"
            };

            // Act
            var response = await _client.PutAsJsonAsync(putRolesByIdEndpointUrl + "a446525a-41c8-4722-8152-5c72e3efd01d", roleModel);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateAsync_WhenRequestWithInvalidId_Returns_NotFound()
        {
            // Arrange
            RoleModel roleModel = new RoleModel()
            {
                Role = "UltimatePower"
            };

            // Act
            var response = await _client.PutAsJsonAsync(putRolesByIdEndpointUrl + Guid.NewGuid(), roleModel);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteAsync_WhenRequestValid_Returns_Ok()
        {
            // Act
            var response = await _client.DeleteAsync(deleteRolesByIdEndpointUrl + "01ff0f71-519b-48f6-8b78-3e1210d2495b");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        public async Task DisposeAsync() => await Task.CompletedTask;
    }
}
