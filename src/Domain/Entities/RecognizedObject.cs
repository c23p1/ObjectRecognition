namespace Domain.Entities;

public class RecognizedObject
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public int ObjectRecognitionRequestId { get; set; }
	public required ObjectRecognitionRequest ObjectRecognitionRequest { get; set; }
}
