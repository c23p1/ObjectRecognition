using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Data.Repositories;

public class MaterialRecognitionRequestsRepository : IMaterialRecognitionRequestsRepository
{
	private readonly DbSet<MaterialRecognitionRequest> _materialRecognitionRequests;

	public MaterialRecognitionRequestsRepository(ApplicationDbContext dbContext)
	{
		_materialRecognitionRequests = dbContext.Set<MaterialRecognitionRequest>();
	}

	public async Task<List<MaterialRecognitionRequest>> GetAllAsync() =>
		await _materialRecognitionRequests.ToListAsync();

	public void Add(MaterialRecognitionRequest request) =>
		_materialRecognitionRequests.Add(request);

	public async Task<MaterialRecognitionRequest?> GetByIdAsync(int id) =>
		await _materialRecognitionRequests.FindAsync(id);
}
