using Grpc.Core.Interceptors;
using Grpc.Core;

namespace gRPC_Test.Interceptors;

public class MyInterceptor : Interceptor
{
	private readonly Guid testGuid = Guid.NewGuid();

	public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
	{
		Console.WriteLine("Before call");

		var result = await continuation(request, context);

		Console.WriteLine("After call");

		return result;
	}
}
