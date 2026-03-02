using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services;

public class RecognitionService
{
	private readonly IApiProvider _apiProvider;
	private readonly IObjectRecognitionRequestsRepository _recognitionRequestsRepository;
	private readonly IImagesRepository _imagesRepository;
	private readonly IRecognizedObjectsRepository _recognizedObjectsRepository;
	private readonly IMaterialRecognitionRequestsRepository _materialRecognitionRequestsRepository;
	private readonly IMaterialRecognitionRequestObjectsRepository _materialRecognitionRequestObjectsRepository;
	private readonly IMaterialsRepository _materialsRepository;
	private readonly IUnitOfWork _unitOfWork;

	public RecognitionService(IApiProvider apiProvider,
		IObjectRecognitionRequestsRepository recognitionRequestsRepository,
		IImagesRepository imagesRepository,
		IRecognizedObjectsRepository recognizedObjectsRepository,
		IMaterialRecognitionRequestsRepository materialRecognitionRequestsRepository,
		IMaterialRecognitionRequestObjectsRepository materialRecognitionRequestObjectsRepository,
		IMaterialsRepository materialsRepository,
		IUnitOfWork unitOfWork)
	{
		_apiProvider = apiProvider;
		_recognitionRequestsRepository = recognitionRequestsRepository;
		_imagesRepository = imagesRepository;
		_recognizedObjectsRepository = recognizedObjectsRepository;
		_materialRecognitionRequestsRepository = materialRecognitionRequestsRepository;
		_materialRecognitionRequestObjectsRepository = materialRecognitionRequestObjectsRepository;
		_materialsRepository = materialsRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<GenericResult<MainObjectsResult>> GetMainObjectsFromImage(string imageUrl)
	{
		var imageLoadingResult = await _apiProvider.PostImage(imageUrl);

		if (!imageLoadingResult.IsSuccess)
		{
			return GenericResult<MainObjectsResult>.Failure(imageLoadingResult.Error!);
		}

		var imageId = imageLoadingResult.Value;
		var result = await _apiProvider.GetMainObjects(imageId);

		if (!result.IsSuccess)
		{
			return GenericResult<MainObjectsResult>.Failure(Error.Unexpected("Не удалось получить ответ от модели"));
		}

		var image = new Image
		{
			CloudId = imageId,
			Url = imageUrl
		};
		_imagesRepository.Add(image);

		var recognitionRequest = new ObjectRecognitionRequest
		{
			Image = image,
			CreatedAt = DateTime.UtcNow
		};
		_recognitionRequestsRepository.Add(recognitionRequest);

		foreach (var @object in result.Value)
		{
			var recognizedObject = new RecognizedObject
			{
				Name = @object,
				ObjectRecognitionRequest = recognitionRequest
			};
			_recognizedObjectsRepository.Add(recognizedObject);
		}

		await _unitOfWork.SaveChangesAsync();

		return GenericResult<MainObjectsResult>.Success(new MainObjectsResult
			{
				RequestId = recognitionRequest.Id,
				ObjectNames = result.Value
			});
	}

	public async Task<GenericResult<ObjectWithMaterialsResult[]>> GetMainObjectsMaterialsByRequestId(int requestId, string[]? mainObjects)
	{
		var request = await _recognitionRequestsRepository.GetByIdAsync(requestId);

		if (request is null)
		{
			return GenericResult<ObjectWithMaterialsResult[]>.Failure(Error.EntityNotFound(nameof(request), requestId.ToString()));
		}

		if (mainObjects is null)
		{
			mainObjects = request.RecognizedObjects.Select(o => o.Name).ToArray();
		}

		if (mainObjects.Length == 0)
		{
			return GenericResult<ObjectWithMaterialsResult[]>.Failure(Error.Validation("Массив основных объектов не может быть пустым"));
		}

		var result = await _apiProvider.GetMainObjectsMaterials(request.Image.CloudId, mainObjects);

		if (!result.IsSuccess)
		{
			return GenericResult<ObjectWithMaterialsResult[]>.Failure(result.Error!);
		}

		var requestToSave = new MaterialRecognitionRequest
		{
			ObjectRecognitionRequest = request,
			CreatedAt = DateTime.UtcNow
		};
		_materialRecognitionRequestsRepository.Add(requestToSave);

		foreach (var @object in result.Value)
		{
			var requestObject = new MaterialRecognitionRequestObject
			{
				Name = @object.ObjectName,
				MaterialRecognitionRequest = requestToSave
			};
			_materialRecognitionRequestObjectsRepository.Add(requestObject);

			foreach (var materialName in @object.Materials)
			{
				_materialsRepository.Add(new Material
				{
					Name = materialName,
					MaterialRecognitionRequestObject = requestObject
				});
			}
		}

		await _unitOfWork.SaveChangesAsync();

		return GenericResult<ObjectWithMaterialsResult[]>.Success(result.Value);
	}
}
