namespace Infrastructure.Vlm.GigaChat.DTOs.Responses;

public record GigaChatChatCompletionResponse
{
	public required List<GigaChatChatCompletionChoice> Choices { get; init; }
}

public record GigaChatChatCompletionChoice
{
	public required GigaChatChatCompletionMessage Message { get; init; }
}

public record GigaChatChatCompletionMessage
{
	public required string Content { get; init; }
}
