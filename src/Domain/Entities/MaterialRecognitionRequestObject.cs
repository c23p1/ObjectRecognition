namespace Domain.Entities;

public class MaterialRecognitionRequestObject
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public int MaterialRecognitionRequestId { get; set; }
	public required MaterialRecognitionRequest MaterialRecognitionRequest { get; set; }
}
