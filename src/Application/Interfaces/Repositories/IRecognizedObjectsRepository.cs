using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IRecognizedObjectsRepository
{
	Task<List<RecognizedObject>> GetAllAsync();
	Task<RecognizedObject?> GetByIdAsync(int id);
	void Add(RecognizedObject recognizedObject);
}
