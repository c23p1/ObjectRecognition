namespace Domain.Entities;

public class MaterialRecognitionRequest
{
	public int Id { get; set; }
	public int ObjectRecognitionRequestId { get; set; }
	public required ObjectRecognitionRequest ObjectRecognitionRequest { get; set; }
	public DateTime CreatedAt { get; set; }
}
