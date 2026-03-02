namespace Domain.Entities;

public class Image
{
	public int Id { get; set; }
	public required string CloudId { get; set; }
	public required string Url { get; set; }
}
