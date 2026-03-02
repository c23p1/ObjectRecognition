using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<ObjectRecognitionRequest> ObjectRecognitionRequests { get; set; }
	public DbSet<RecognizedObject> RecognizedObjects { get; set; }
	public DbSet<Image> Images { get; set; }
	public DbSet<MaterialRecognitionRequest> MaterialRecognitionRequests { get; set; }
	public DbSet<MaterialRecognitionRequestObject> MaterialRecognitionRequestObjects { get; set; }
	public DbSet<Material> Materials { get; set; }
}
