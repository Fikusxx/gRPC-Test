using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace gRPC_Test.HealthChecks;

public class MyHealthCheck : IHealthCheck
{
	public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
	{
		await Task.Delay(100, cancellationToken);

		await Console.Out.WriteLineAsync("CHECKING........");

		return HealthCheckResult.Healthy();
	}
}
