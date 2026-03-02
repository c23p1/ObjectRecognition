namespace Infrastructure.Services;

public class ImageLoadingService
{
	private readonly IHttpClientFactory _httpClientFactory;

	public ImageLoadingService(IHttpClientFactory httpClientFactory)
	{
		_httpClientFactory = httpClientFactory;
	}

	public async Task<Stream> GetImageStreamByUrl(string imageUrl)
	{
		var client = _httpClientFactory.CreateClient();
		var response = await client.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead);
		response.EnsureSuccessStatusCode();

		var stream = new MemoryStream();
		await using var source = await response.Content.ReadAsStreamAsync();
		await source.CopyToAsync(stream);
		stream.Position = 0;
		return stream;
	}
}
