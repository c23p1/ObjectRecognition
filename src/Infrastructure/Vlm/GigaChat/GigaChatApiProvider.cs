using System.Text.Json;
using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Services;
using Infrastructure.Vlm.GigaChat.API;
using Infrastructure.Vlm.GigaChat.DTOs.Requests;
using Infrastructure.Vlm.GigaChat.DTOs.Responses;
using Microsoft.Extensions.Configuration;
using Refit;

namespace Infrastructure.Vlm.GigaChat;

public sealed class GigaChatApiProvider : IApiProvider
{
	private const string Purpose = "general";

	private readonly string _vlmName;
	private readonly IGigaChatApi _gigaChatApi;
	private readonly IConfiguration _configuration;
	private readonly ImageLoadingService _imageLoadingService;

	public GigaChatApiProvider(IGigaChatApi gigaChatApi, IConfiguration configuration, ImageLoadingService imageLoadingService)
	{
		_gigaChatApi = gigaChatApi;
		_configuration = configuration;
		_imageLoadingService = imageLoadingService;
		_vlmName = _configuration["Vlm:Providers:GigaChat:Model"]
			?? throw new KeyNotFoundException("Название Vlm не найдено");
	}

	public async Task<GenericResult<string[]>> GetMainObjects(string imageId)
	{
		var getMainObjectsSystemPrompt = _configuration["Vlm:Prompts:GetMainObjects"]
			?? throw new KeyNotFoundException("Не найден промпт GetMainObjects");

		var response = await _gigaChatApi.GetModelResponse(new GigaChatChatCompletionRequest
			{
				Model = _vlmName,
				Messages = new GigaChatMessageRequest[]
				{
					new GigaChatMessageRequest
					{
						Role = "system",
						Content = getMainObjectsSystemPrompt
					},
					new GigaChatMessageRequest
					{
						Role = "user",
						Attachments = new string[] { imageId },
					}
				}
			});

		if (!response.IsSuccessful)
		{
			return GenericResult<string[]>.Failure(Error.Unexpected($"Сервер ответил с ошибкой. Код: {response.Error.StatusCode}"));
		}

		string[] result;
		try
		{
			result = JsonSerializer.Deserialize<string[]>(response.Content.Choices[0].Message.Content)
				?? throw new InvalidOperationException("Не удалось преобразовать ответ модели");
		}
		catch
		{
			return GenericResult<string[]>.Failure(Error.Unexpected("Ответ модели имеет неверный формат"));
		}

		if (result.Length == 0)
		{
			return GenericResult<string[]>.Failure(Error.InvalidOperation("Не удалось определить основные предметы на изображении"));
		}

		return GenericResult<string[]>.Success(result);
	}

	public async Task<GenericResult<ObjectWithMaterialsResult[]>> GetMainObjectsMaterials(string imageId, string[] mainObjects)
	{
		var getMainObjectsMaterialsSystemPrompt = _configuration["Vlm:Prompts:GetMainObjectsMaterials"]
			?? throw new KeyNotFoundException("Не найден промпт GetMainObjectsMaterials");

		var response = await _gigaChatApi.GetModelResponse(new GigaChatChatCompletionRequest
			{
				Model = _vlmName,
				Messages = new GigaChatMessageRequest[]
				{
					new GigaChatMessageRequest
					{
						Role = "system",
						Content = getMainObjectsMaterialsSystemPrompt
					},
					new GigaChatMessageRequest
					{
						Role = "user",
						Content = string.Join(", ", mainObjects)
					},
					new GigaChatMessageRequest
					{
						Role = "user",
						Attachments = new string[] { imageId },
					}
				}
			});

		if (!response.IsSuccessful)
		{
			return GenericResult<ObjectWithMaterialsResult[]>.Failure(Error.Unexpected($"Сервер ответил с ошибкой. Код: {response.Error.StatusCode}"));
		}

		ObjectWithMaterialsResult[] result;
		try
		{
			result = JsonSerializer.Deserialize<ObjectWithMaterialsResult[]>(response.Content.Choices[0].Message.Content)
				?? throw new InvalidOperationException("Не удалось преобразовать ответ модели");
		}
		catch
		{
			return GenericResult<ObjectWithMaterialsResult[]>.Failure(Error.Unexpected("Ответ модели имеет неверный формат"));
		}

		if (result.Length == 0)
		{
			return GenericResult<ObjectWithMaterialsResult[]>.Failure(Error.InvalidOperation("Не удалось определить основные предметы и их материалы"));
		}

		return GenericResult<ObjectWithMaterialsResult[]>.Success(result);
	}

	public async Task<GenericResult<string>> PostImage(string imageUrl)
	{
		if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var imageUri))
		{
			return GenericResult<string>.Failure(Error.Validation("Неверно указана ссылка на изображение"));
		}

		var extension = Path.GetExtension(imageUri.AbsolutePath).ToLowerInvariant();
		var mimeType = extension switch
		{
			".jpg" or ".jpeg" => "image/jpeg",
			".png" => "image/png",
			_ => null
		};

		if (mimeType is null)
		{
			return GenericResult<string>.Failure(Error.Validation("Изображение имеет неверный формат"));
		}

		
		ApiResponse<GigaChatPostFileResponse> response;
		try
		{
			await using var imageStream = await _imageLoadingService.GetImageStreamByUrl(imageUrl);
			var streamPart = new StreamPart(imageStream, $"image{extension}", mimeType);
			response = await _gigaChatApi.PostFile(streamPart, Purpose);
		}
		catch
		{
			return GenericResult<string>.Failure(Error.InvalidOperation("Не удалось загрузить изображение"));
		}

		if (!response.IsSuccessful)
		{
			return GenericResult<string>.Failure(Error.Unexpected($"Сервер ответил с ошибкой. Код: {response.Error.StatusCode}"));
		}

		return GenericResult<string>.Success(response.Content.Id);
	}
}
