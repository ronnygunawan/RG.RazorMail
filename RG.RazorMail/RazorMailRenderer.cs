using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RazorLight;

namespace RG.RazorMail {
	public class RazorMailRenderer {
		private readonly RazorLightEngine _razorLightEngine;
		private readonly RazorMailRendererOptions _options;

		public RazorMailRenderer(
			RazorLightEngine razorLightEngine,
			IOptions<RazorMailRendererOptions> optionsAccessor
		) {
			_razorLightEngine = razorLightEngine;
			_options = optionsAccessor.Value;
		}

		public async Task<string> RenderAsync<TModel>(string viewName, TModel model) {
			// Retrieve template from cache
			if (_razorLightEngine.Handler.Cache.RetrieveTemplate(viewName) is {
				Success: true,
				Template: {
					Key: { } templateKey,
					TemplatePageFactory: { } templatePageFactory
				}
			}) {
				// Create template page
				ITemplatePage templatePage = templatePageFactory.Invoke();

				// Render page then return rendered page
				return await _razorLightEngine.RenderTemplateAsync(templatePage, model);
			}

			// Retrieve template from assembly resources
			Assembly viewsAssembly = _options.ViewsAssembly ?? throw new InvalidOperationException("ViewsAssemnbly not configured.");

			// Convert view name to resource name
			string resourceName
				= viewsAssembly.GetName().Name
				+ "."
				+ viewName
					.Replace(" ", "_", StringComparison.OrdinalIgnoreCase)
					.Replace("\\", ".", StringComparison.OrdinalIgnoreCase)
					.Replace("/", ".", StringComparison.OrdinalIgnoreCase);

			// Open resource stream
			using Stream resourceStream = viewsAssembly.GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException($"View {viewName} not found.");
			using StreamReader streamReader = new(resourceStream);

			// Read view template
			string viewTemplate = await streamReader.ReadToEndAsync();

			// Compile view template, cache, render page, then return rendered page
			return await _razorLightEngine.CompileRenderStringAsync(viewName, viewTemplate, model);
		}

		public string InlineCss(string html, string? css = null) {
			return PreMailer.Net.PreMailer.MoveCssInline(
				html: html,
				removeStyleElements: true,
				css: css,
				stripIdAndClassAttributes: true,
				removeComments: true
			).Html;
		}

		public async Task<string> RenderAndInlineCssAsync<TModel>(string viewName, TModel model, string? css = null) {
			string html = await RenderAsync(viewName, model);
			return InlineCss(html, css);
		}
	}
}
