namespace Domain.Entities;

public class Material
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public int MaterialRecognitionRequestObjectId { get; set; }
	public required MaterialRecognitionRequestObject MaterialRecognitionRequestObject { get; set; }
}
