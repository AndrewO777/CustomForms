using CustomForms.Pages;

namespace CustomForms;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton<AddFormPage>();
		builder.Services.AddTransient<AddFormPage>();
		builder.Services.AddSingleton<FormsPage>();
		builder.Services.AddTransient<FormsPage>();
		builder.Services.AddSingleton<GalleryPage>();
		builder.Services.AddTransient<GalleryPage>();

		builder.Services.AddSingleton<CustomFormDB>();
		return builder.Build();
	}
}
