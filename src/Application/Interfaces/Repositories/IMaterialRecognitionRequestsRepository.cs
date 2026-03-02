using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IMaterialRecognitionRequestsRepository
{
	Task<List<MaterialRecognitionRequest>> GetAllAsync();
	Task<MaterialRecognitionRequest?> GetByIdAsync(int id);
	void Add(MaterialRecognitionRequest request);
}
