using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.HtmlRendering;
using Microsoft.Extensions.Options;
using RazorLight;

namespace RG.RazorMail {
	/// <summary>
	/// Renderer for rendering .cshtml View files and .razor Components to HTML string
	/// </summary>
	public class RazorMailRenderer {
		private readonly IRazorLightEngine _razorLightEngine;
		private readonly HtmlRenderer _htmlRenderer;
		private readonly RazorMailRendererOptions _options;

		[Obsolete("Use dependency injection for creating instance of this class")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
		public RazorMailRenderer(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
			IRazorLightEngine razorLightEngine,
			HtmlRenderer htmlRenderer,
			IOptions<RazorMailRendererOptions> optionsAccessor
		) {
			_razorLightEngine = razorLightEngine;
			_htmlRenderer = htmlRenderer;
			_options = optionsAccessor.Value;
		}

		/// <summary>
		/// Render a .cshtml View file. The View file must be located in the configured ViewsAssembly and registered as an embedded resource.
		/// </summary>
		/// <typeparam name="TModel">Type of the model</typeparam>
		/// <param name="viewName">Name of the view file, relative to its containing assembly. e.g. Views/HelloWorld.cshtml</param>
		/// <param name="model">Model to pass to the view</param>
		/// <returns>Rendered HTML string</returns>
		/// <exception cref="InvalidOperationException">ViewsAssembly not configured.</exception>
		/// <exception cref="FileNotFoundException">View not found.</exception>
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

		/// <summary>
		/// Render a .razor Component.
		/// </summary>
		/// <typeparam name="TComponent">Type of the razor component</typeparam>
		/// <param name="parameters">Component parameters</param>
		/// <returns>Rendered HTML string</returns>
		public async Task<string> RenderComponentAsync<TComponent>(IDictionary<string, object?> parameters) where TComponent : IComponent {
			return await _htmlRenderer.Dispatcher.InvokeAsync(async () => {
				ParameterView parameterView = ParameterView.FromDictionary(parameters);
				HtmlRootComponent rootComponent = await _htmlRenderer.RenderComponentAsync(typeof(TComponent), parameterView);
				return rootComponent.ToHtmlString();
			});
		}

		/// <summary>
		/// Inline CSS into HTML.
		/// </summary>
		/// <param name="html">The HTML file</param>
		/// <param name="css">Additional CSS styles to be included in inlining</param>
		/// <returns>HTML with inlined CSS</returns>
		public string InlineCss(string html, string? css = null) {
			return PreMailer.Net.PreMailer.MoveCssInline(
				html: html,
				removeStyleElements: true,
				css: css,
				stripIdAndClassAttributes: true,
				removeComments: true,
				ignoreElements: "[data-premailer=\"ignore\"]"
			).Html;
		}

		/// <summary>
		/// Render a .cshtml View file and inline CSS. The View file must be located in the configured ViewsAssembly and registered as an embedded resource.
		/// </summary>
		/// <typeparam name="TModel">Type of the model</typeparam>
		/// <param name="viewName">Name of the view file, relative to its containing assembly. e.g. Views/HelloWorld.cshtml</param>
		/// <param name="model">Model to pass to the view</param>
		/// <param name="css">Additional CSS styles to be included in inlining</param>
		/// <returns>Rendered HTML string with inlined CSS</returns>
		public async Task<string> RenderAndInlineCssAsync<TModel>(string viewName, TModel model, string? css = null) {
			string html = await RenderAsync(viewName, model);
			return InlineCss(html, css);
		}

		/// <summary>
		/// Render a .razor Component and inline CSS.
		/// </summary>
		/// <typeparam name="TComponent">Type of the razor component</typeparam>
		/// <param name="parameters">Component parameters</param>
		/// <param name="css">Additional CSS styles to be included in inlining</param>
		/// <returns>Rendered HTML string with inlined CSS</returns>
		public async Task<string> RenderComponentAndInlineCssAsync<TComponent>(IDictionary<string, object?> parameters, string? css = null) where TComponent : IComponent {
			string html = await RenderComponentAsync<TComponent>(parameters);
			return InlineCss(html, css);
		}
	}
}
