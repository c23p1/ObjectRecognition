using System.Text.Json;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Services;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Data.Repositories;
using Infrastructure.Services;
using Infrastructure.Vlm.GigaChat;
using Infrastructure.Vlm.GigaChat.API;
using Infrastructure.Vlm.GigaChat.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddVlmServices(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("Default")
			?? throw new KeyNotFoundException("Не удалось найти строку подключения к базе данных");

		services.AddDbContext<ApplicationDbContext>(dbContextOptions =>
			dbContextOptions.UseNpgsql(connectionString));

		var serviceProvider = services.BuildServiceProvider();
		using (var scope = serviceProvider.CreateScope())
		{
			var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			dbContext.Database.EnsureCreated();
		}

		services.AddHttpClient();

		services.AddTransient<ImageLoadingService>();

		services.AddTransient<GigaChatAuthHeaderHandler>();

		services.AddRefitClient<ISberbankAuthApi>(new RefitSettings
			{
				ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
				{
					PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
					DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower
				})
			})
			.ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration["Vlm:Providers:GigaChat:AuthAddress"]
				?? throw new KeyNotFoundException("Не удалось найти параметр AuthAddress для провайдера GigaChat")))
			.ConfigurePrimaryHttpMessageHandler(() =>
			{
				var handler = new HttpClientHandler();

				// Обход ошибок при отправке запросов без установленных сертификатов Минцифры
				handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
				return handler;
			});

		services.AddRefitClient<IGigaChatApi>(new RefitSettings
			{
				ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
				{
					PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
					DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower
				})
			})
			.ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration["Vlm:Providers:GigaChat:BaseAddress"]
				?? throw new KeyNotFoundException("Не удалось найти параметр BaseAddress для провайдера GigaChat")))
			.ConfigurePrimaryHttpMessageHandler(() =>
			{
				var handler = new HttpClientHandler();
				handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
				return handler;
			})
			.AddHttpMessageHandler<GigaChatAuthHeaderHandler>();

		services.AddScoped<IApiProvider, GigaChatApiProvider>();

		services.AddScoped<RecognitionService>();

		services.AddScoped<IObjectRecognitionRequestsRepository, ObjectRecognitionRequestsRepository>();
		services.AddScoped<IRecognizedObjectsRepository, RecognizedObjectsRepository>();
		services.AddScoped<IImagesRepository, ImagesRepository>();
		services.AddScoped<IMaterialRecognitionRequestsRepository, MaterialRecognitionRequestsRepository>();
		services.AddScoped<IMaterialRecognitionRequestObjectsRepository, MaterialRecognitionRequestObjectsRepository>();
		services.AddScoped<IMaterialsRepository, MaterialsRepository>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();

		return services;
	}
}
