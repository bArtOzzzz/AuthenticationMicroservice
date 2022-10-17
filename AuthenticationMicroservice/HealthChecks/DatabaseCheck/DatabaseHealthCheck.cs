using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Data.SqlClient;

namespace AuthenticationMicroservice.HealthChecks.DatabaseCheck
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public DatabaseHealthCheck(IConfiguration configuration) => _configuration = configuration;

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
                                                        CancellationToken cancellationToken = default)
        {
            var isHealthy = HealthCheckResult.Healthy();

            try
            {
                var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

                connection.Open();

                isHealthy = HealthCheckResult.Healthy("Database connection is OK");
            }
            catch (Exception)
            {
                isHealthy = HealthCheckResult.Unhealthy("Database connection ERROR");
            }

            return Task.FromResult(isHealthy);
        }
    }
}
