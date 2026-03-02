using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Data.Repositories;

public class ImagesRepository : IImagesRepository
{
	private readonly DbSet<Image> _images;

	public ImagesRepository(ApplicationDbContext dbContext)
	{
		_images = dbContext.Set<Image>();
	}

	public async Task<List<Image>> GetAllAsync() =>
		await _images.ToListAsync();

	public async Task<Image?> GetByIdAsync(int id) =>
		await _images.FindAsync(id);

	public void Add(Image image) =>
		_images.Add(image);
}
