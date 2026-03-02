using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Data.Repositories;

public class RecognizedObjectsRepository : IRecognizedObjectsRepository
{
	private readonly DbSet<RecognizedObject> _recognizedObjects;

	public RecognizedObjectsRepository(ApplicationDbContext dbContext)
	{
		_recognizedObjects = dbContext.Set<RecognizedObject>();
	}

	public async Task<List<RecognizedObject>> GetAllAsync() =>
		await _recognizedObjects.ToListAsync();

	public async Task<RecognizedObject?> GetByIdAsync(int id) =>
		await _recognizedObjects.FindAsync(id);

	public void Add(RecognizedObject recognizedObject) =>
		_recognizedObjects.Add(recognizedObject);
}
