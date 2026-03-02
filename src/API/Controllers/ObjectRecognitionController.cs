using API.Extensions;
using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("recognition")]
public class RecognitionController : ControllerBase
{
	private readonly RecognitionService _recognitionService;

	public RecognitionController(RecognitionService recognitionService)
	{
		_recognitionService = recognitionService;
	}

	[HttpPost("getMainObjects")]
	public async Task<ActionResult<MainObjectsResult>> GetMainObjectsFromImage(string imageUrl)
	{
		var result = await _recognitionService.GetMainObjectsFromImage(imageUrl);
		return result.ToActionResult();
	}

	[HttpPost("getMaterialsOfMainObjects/{requestId:int:min(0)}")]
	public async Task<ActionResult<ObjectWithMaterialsResult[]>> GetMaterialsOfMainObjectsByRequestId(int requestId, string[]? objects)
	{
		var result = await _recognitionService.GetMainObjectsMaterialsByRequestId(requestId, objects);
		return result.ToActionResult();
	}
}
