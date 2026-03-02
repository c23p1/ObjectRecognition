using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IMaterialRecognitionRequestObjectsRepository
{
	Task<List<MaterialRecognitionRequestObject>> GetAllAsync();
	Task<MaterialRecognitionRequestObject?> GetByIdAsync(int id);
	void Add(MaterialRecognitionRequestObject materialRecognitionRequestObject);
}
