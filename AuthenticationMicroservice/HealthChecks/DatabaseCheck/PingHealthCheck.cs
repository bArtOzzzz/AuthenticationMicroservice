using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.NetworkInformation;

namespace AuthenticationMicroservice.HealthChecks.DatabaseCheck
{
    public class PingHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
			try
			{
				using (var ping = new Ping())
				{
					var reply = ping.Send("localhost");
					if(reply.Status != IPStatus.Success)
					{
						return Task.FromResult(HealthCheckResult.Unhealthy("Ping is" + reply));
					}

					if(reply.RoundtripTime > 100)
					{
						return Task.FromResult(HealthCheckResult.Degraded("Ping is" + reply));
					}
				}

				return Task.FromResult(HealthCheckResult.Healthy("Ping is OK"));
			}
			catch
			{
				return Task.FromResult(HealthCheckResult.Unhealthy("Something went wrong with health checking"));
			}
        }
    }
}
