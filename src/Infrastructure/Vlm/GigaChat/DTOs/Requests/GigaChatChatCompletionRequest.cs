namespace Infrastructure.Vlm.GigaChat.DTOs.Requests;

public record GigaChatChatCompletionRequest
{
	public required string Model { get; init; }
	public required GigaChatMessageRequest[] Messages { get; init; }
}
