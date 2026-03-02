using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IObjectRecognitionRequestsRepository
{
	Task<List<ObjectRecognitionRequest>> GetAllAsync();
	Task<ObjectRecognitionRequest?> GetByIdAsync(int id);
	void Add(ObjectRecognitionRequest recognitionRequest);
}
