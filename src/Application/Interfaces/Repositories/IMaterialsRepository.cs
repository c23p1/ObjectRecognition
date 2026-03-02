using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IMaterialsRepository
{
	Task<List<Material>> GetAllAsync();
	Task<Material?> GetByIdAsync(int id);
	void Add(Material material);
}
