namespace Domain.Entities;

public class ObjectRecognitionRequest
{
	public int Id { get; set; }
	public int ImageId { get; set; }
	public required Image Image { get; set; }
	public DateTime CreatedAt { get; set; }
	public List<RecognizedObject> RecognizedObjects { get; set; } = new List<RecognizedObject>();
}
