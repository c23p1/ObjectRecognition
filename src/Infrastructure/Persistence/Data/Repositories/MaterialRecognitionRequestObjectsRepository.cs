using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Data.Repositories;

public class MaterialRecognitionRequestObjectsRepository : IMaterialRecognitionRequestObjectsRepository
{
	private readonly DbSet<MaterialRecognitionRequestObject> _materialRecognitionRequestObjects;

	public MaterialRecognitionRequestObjectsRepository(ApplicationDbContext dbContext)
	{
		_materialRecognitionRequestObjects = dbContext.Set<MaterialRecognitionRequestObject>();
	}

	public async Task<List<MaterialRecognitionRequestObject>> GetAllAsync() =>
		await _materialRecognitionRequestObjects.ToListAsync();

	public async Task<MaterialRecognitionRequestObject?> GetByIdAsync(int id) =>
		await _materialRecognitionRequestObjects.FindAsync(id);

	public void Add(MaterialRecognitionRequestObject materialRecognitionRequestObject) =>
		_materialRecognitionRequestObjects.Add(materialRecognitionRequestObject);
}
