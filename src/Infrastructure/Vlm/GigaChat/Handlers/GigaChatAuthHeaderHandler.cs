using System.Net.Http.Headers;
using Infrastructure.Vlm.GigaChat.API;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Vlm.GigaChat.Handlers;

public class GigaChatAuthHeaderHandler : DelegatingHandler
{
	private readonly ISberbankAuthApi _sberbankAuthApi;
	private readonly IConfiguration _configuration;

	public GigaChatAuthHeaderHandler(ISberbankAuthApi sberbankAuthApi, IConfiguration configuration)
	{
		_sberbankAuthApi = sberbankAuthApi;
		_configuration = configuration;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var authorizationKey = _configuration["Vlm:Providers:GigaChat:AuthorizationKey"]
			?? throw new KeyNotFoundException("Не удалось найти параметр AuthorizationKey для провайдера GigaChat");

		var authorizationScope = _configuration["Vlm:Providers:GigaChat:AuthorizationScope"]
			?? throw new KeyNotFoundException("Не удалось найти параметр AuthorizationScope для провайдера GigaChat");

		var authResponse = await _sberbankAuthApi.GetAuthenticationToken(authorizationKey, Guid.NewGuid().ToString(),
			new Dictionary<string, string> { ["scope"] = authorizationScope });

		if (!authResponse.IsSuccessful)
		{
			throw new KeyNotFoundException($"Не удалось получить access token от Sberbank Auth API. Код: {authResponse.StatusCode}");
		}

		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.Content.AccessToken);

		return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
	}
}
