using Application.Common;
using Application.DTOs;

namespace Application.Interfaces;

public interface IApiProvider
{
	Task<GenericResult<string[]>> GetMainObjects(string imageId);
	Task<GenericResult<ObjectWithMaterialsResult[]>> GetMainObjectsMaterials(string imageId, string[] mainObjects);
	Task<GenericResult<string>> PostImage(string imageUrl);
}
