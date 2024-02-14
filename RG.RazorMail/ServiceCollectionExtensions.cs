using System;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using RazorLight;
using RazorLight.Extensions;

namespace RG.RazorMail {
	public static class ServiceCollectionExtensions {
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
			services.AddSingleton<HtmlRenderer>();
			services.AddTransient<RazorMailRenderer>();
			return services;
		}
	}
}
