namespace Infrastructure.Vlm.GigaChat.DTOs.Responses;

public record GigaChatPostFileResponse
{
	public required string Id { get; init; }
	public required string Filename { get; init; }
	public required long CreatedAt { get; init; }
}
