using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using gRPC_Test.Interceptors;
using gRPC_Test.HealthChecks;
using HealthChecks.UI.Client;
using gRPC_Test.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddGrpc(options =>
{
	options.EnableDetailedErrors = true;
	options.Interceptors.Add<MyInterceptor>();
})
	// register interceptor specifically for a service
	//.AddServiceOptions<CatsService>(options => options.Interceptors.Add<MyInterceptor>())
	.AddJsonTranscoding();

builder.Services.AddGrpcHealthChecks()
	.AddCheck("Sample", () => HealthCheckResult.Healthy())
	.AddCheck<MyHealthCheck>(nameof(MyHealthCheck));

builder.Services.Configure<HealthCheckPublisherOptions>(options =>
{
	options.Delay = TimeSpan.Zero;
	options.Period = TimeSpan.FromSeconds(25);
});


// explicitly register interceptor as singleton opposed to default being scoped
//builder.Services.AddSingleton<MyInterceptor>();


builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1",
		new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
});

var app = builder.Build();

app.MapHealthChecks("_hc", new HealthCheckOptions()
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapGrpcService<CatsService>();
app.MapGrpcHealthChecksService();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
	});
}

app.UseAuthorization();

app.MapControllers();

app.Run();
