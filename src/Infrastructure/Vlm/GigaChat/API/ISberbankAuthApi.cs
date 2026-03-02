using Infrastructure.Vlm.GigaChat.DTOs.Responses;
using Refit;

namespace Infrastructure.Vlm.GigaChat.API;

[Headers("Accept: application/json", "Content-Type: application/x-www-form-urlencoded")]
public interface ISberbankAuthApi
{
	[Post("/oauth")]
	Task<ApiResponse<SberbankAuthTokenResponse>> GetAuthenticationToken([Authorize("Basic")] string authorizationKey, [Header("RqUID")] string requestId, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> requestBody);
}
