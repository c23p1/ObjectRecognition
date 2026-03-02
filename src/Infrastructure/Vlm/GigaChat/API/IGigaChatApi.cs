using Infrastructure.Vlm.GigaChat.DTOs.Responses;
using Infrastructure.Vlm.GigaChat.DTOs.Requests;
using Refit;

namespace Infrastructure.Vlm.GigaChat.API;

[Headers("Accept: application/json")]
public interface IGigaChatApi
{
	[Post("/chat/completions")]
	Task<ApiResponse<GigaChatChatCompletionResponse>> GetModelResponse([Body] GigaChatChatCompletionRequest request);

	[Post("/files")]
	[Multipart]
	Task<ApiResponse<GigaChatPostFileResponse>> PostFile(StreamPart file, string purpose);
}
