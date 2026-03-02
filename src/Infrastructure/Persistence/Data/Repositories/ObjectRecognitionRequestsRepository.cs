using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Data.Repositories;

public class ObjectRecognitionRequestsRepository : IObjectRecognitionRequestsRepository
{
	private readonly DbSet<ObjectRecognitionRequest> _recognitionRequests;

	public ObjectRecognitionRequestsRepository(ApplicationDbContext dbContext)
	{
		_recognitionRequests = dbContext.Set<ObjectRecognitionRequest>();
	}

	public async Task<List<ObjectRecognitionRequest>> GetAllAsync() =>
		await _recognitionRequests.ToListAsync();

	public async Task<ObjectRecognitionRequest?> GetByIdAsync(int id) =>
		await _recognitionRequests
				.Include(r => r.Image)
				.Include(r => r.RecognizedObjects)
				.SingleOrDefaultAsync(r => r.Id == id);

	public void Add(ObjectRecognitionRequest recognitionRequest) =>
		_recognitionRequests.Add(recognitionRequest);
}
