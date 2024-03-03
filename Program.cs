using Microsoft.OpenApi.Models;
using gRPC_Test.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddGrpc(options =>
{
	options.EnableDetailedErrors = true;
})
	.AddJsonTranscoding();


builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1",
		new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
});

var app = builder.Build();
app.MapGrpcService<CatsService>();

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
