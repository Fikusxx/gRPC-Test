using gRPC_Test.GRPC.CatFeature;
using Grpc.Core;

namespace gRPC_Test.Services;

public class CatsService : CatsGrpcService.CatsGrpcServiceBase
{
	private static List<Cat> cats = new();

	// Endpoints
	// [Authorize] | [Version] | ...
	public override Task<CreateCatResponse> CreateCat(CreateCatRequest request, ServerCallContext context)
	{
		if (cats.FirstOrDefault(x => x.Name == request.Name.ToString()) is not null)
			throw new RpcException(new Status(StatusCode.AlreadyExists, "ID NAHUI."));

		var cat = new Cat() { Name = request.Name.ToString(), Age = request.Age };
		cats.Add(cat);

		return Task.FromResult(new CreateCatResponse() { Response = $"KITTY CAT CREATED WITH NAME {cat.Name} AND AGE {cat.Age}" });
	}

	public override Task<GetCatResponse> GetCat(GetCatRequest request, ServerCallContext context)
	{
		var cat = cats.FirstOrDefault(x => x.Name == request.Name);

		if (cat is null)
			throw new RpcException(new Status(StatusCode.NotFound, "IDI NAHUI XD."));

		return Task.FromResult(new GetCatResponse() { Name = cat.Name, Age = cat.Age });
	}

	public override Task<GetAllCatsResponse> GetAllCats(GetAllCatsRequest request, ServerCallContext context)
	{
		var response = cats.Select(x => new GetCatResponse() { Name = x.Name, Age = x.Age }).ToList();
		var result = new GetAllCatsResponse();
		result.Response.AddRange(response);

		return Task.FromResult(result);
	}

	public override Task<GetAllCatsResponse> GetSoket(Empty request, ServerCallContext context)
	{
		var response = cats.Select(x => new GetCatResponse() { Name = x.Name, Age = x.Age }).ToList();
		var result = new GetAllCatsResponse();
		result.Response.AddRange(response);

		return Task.FromResult(result);
	}
}

public class Cat
{
	public string Name { get; set; }
	public int Age { get; set; }
}