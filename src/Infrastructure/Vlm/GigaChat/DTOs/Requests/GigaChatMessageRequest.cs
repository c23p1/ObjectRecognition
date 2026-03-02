namespace Infrastructure.Vlm.GigaChat.DTOs.Requests;

public record GigaChatMessageRequest
{
	public required string Role { get; init; }
	public string? Content { get; init; }
	public string[]? Attachments { get; init; }
}
