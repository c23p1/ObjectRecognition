namespace Application.DTOs;

public record MainObjectsResult
{
	public int RequestId { get; init; }
	public required string[] ObjectNames { get; init; }
}
