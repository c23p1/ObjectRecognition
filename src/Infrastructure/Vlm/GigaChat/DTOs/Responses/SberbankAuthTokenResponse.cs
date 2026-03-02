namespace Infrastructure.Vlm.GigaChat.DTOs.Responses;

public sealed record SberbankAuthTokenResponse
{
	public required string AccessToken { get; init; }
	public required long ExpiresAt { get; init; }
}
