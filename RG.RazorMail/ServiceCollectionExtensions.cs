using System;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using RazorLight;
using RazorLight.Extensions;

namespace RG.RazorMail {
	/// <summary>
	/// Extension methods for IServiceCollection for adding RazorMailRenderer to the service collection
	/// </summary>
	public static class ServiceCollectionExtensions {
		/// <summary>
		/// Add RazorMailRenderer to the service collection
		/// </summary>
		/// <param name="services"></param>
		/// <param name="setupAction">Optional. Set ViewsAssembly here if you need to render .cshtml View files</param>
		/// <returns></returns>
		public static IServiceCollection AddRazorMail(this IServiceCollection services, Action<RazorMailRendererOptions>? setupAction = null) {
			services.AddLogging();
			services.AddOptions<RazorMailRendererOptions>();
			services.AddRazorLight(() => new RazorLightEngineBuilder()
				.UseEmbeddedResourcesProject(typeof(string))
				.UseMemoryCachingProvider()
				.Build());
			if (setupAction != null) {
				services.Configure(setupAction);
			}
			services.AddScoped<HtmlRenderer>();
			services.AddTransient<RazorMailRenderer>();
			return services;
		}
	}
}
