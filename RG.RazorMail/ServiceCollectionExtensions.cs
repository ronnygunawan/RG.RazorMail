using System;
using Microsoft.Extensions.DependencyInjection;
using RazorLight;
using RazorLight.Extensions;

namespace RG.RazorMail {
	public static class ServiceCollectionExtensions {
		public static IServiceCollection AddRazorMail(this IServiceCollection services, Action<RazorMailRendererOptions> setupAction) {
			services.AddOptions<RazorMailRendererOptions>();
			services.AddRazorLight(() => new RazorLightEngineBuilder()
				.UseEmbeddedResourcesProject(typeof(string))
				.UseMemoryCachingProvider()
				.Build());
			services.Configure(setupAction);
			services.AddTransient<RazorMailRenderer>();
			return services;
		}
	}
}
