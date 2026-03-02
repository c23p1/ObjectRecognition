namespace Application.DTOs;

public record ObjectWithMaterialsResult
{
	public required string ObjectName { get; init; }
	public required string[] Materials { get; init; }
}
