using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Data.Repositories;

public class MaterialsRepository : IMaterialsRepository
{
	private readonly DbSet<Material> _materials;

	public MaterialsRepository(ApplicationDbContext dbContext)
	{
		_materials = dbContext.Set<Material>();
	}

	public async Task<List<Material>> GetAllAsync() =>
		await _materials.ToListAsync();

	public async Task<Material?> GetByIdAsync(int id) =>
		await _materials.FindAsync(id);

	public void Add(Material material) =>
		_materials.Add(material);
}
