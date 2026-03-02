using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IImagesRepository
{
	Task<List<Image>> GetAllAsync();
	Task<Image?> GetByIdAsync(int id);
	void Add(Image image);
}
